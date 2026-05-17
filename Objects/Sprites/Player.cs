using MamacitaOrbit.Managers;
using MamacitaOrbit.Objects.Sprites.InHeritance;
using MamacitaOrbit.UI;
using System.IO;
using System.Linq;
using MamacitaOrbit.Objects.Sprites.Mines;
using MamacitaOrbit.Objects.Sprites.Projectiles;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework.Media;

namespace MamacitaOrbit.Objects.Sprites
{
    internal sealed class Player : AttackingSprite, IAlly
    {
        private long uridium = 0;
        private long credit = 0;
        public string Name { get; set; }
        public float FireCooldown { get; set; }
        private float fireCooldownLeft;
        public bool IsLaserAuto { get; set; }
        public bool IsRocketAuto { get; set; }
        public float RocketCooldown { get; set; }

        private float rocketCooldownLeft;
        public long Uridium 
        { 
            get
            {
                return uridium;
            }
            set 
            {
                if (uridium != value && value > uridium)
                {
                    UIManager.AddText(rect.Right + 50, rect.Top - 25, $"+{value - uridium}U", Color.HotPink,
                        UIManager.infoFont, DepthLayer - 0.0000001f, UIManager.VolatileTexts);
                }
                else
                {
                    UIManager.AddText(rect.Right + 50, rect.Top - 25, $"{value - uridium}U", Color.DeepPink,
                        UIManager.infoFont, DepthLayer - 0.0000001f, UIManager.VolatileTexts);
                }

                uridium = value;

                if (uridium < 0)
                {
                    uridium = 0;
                }
            } 
        }
        public long Credit
        {
            get
            {
                return credit;
            }
            set
            {
                long oldCredit = credit;
                long newCredit = value;

                if (oldCredit != newCredit && newCredit > oldCredit)
                {
                    UIManager.AddText(
                        rect.Right + 50,
                        rect.Top - 5,
                        $"+{newCredit - oldCredit}C",
                        Color.Gold,
                        UIManager.infoFont,
                        DepthLayer - 0.0000001f,
                        UIManager.VolatileTexts
                    );
                }
                else if (oldCredit != newCredit)
                {
                    UIManager.AddText(
                        rect.Right + 50,
                        rect.Top - 5,
                        $"{newCredit - oldCredit}C",
                        Color.IndianRed,
                        UIManager.infoFont,
                        DepthLayer - 0.0000001f,
                        UIManager.VolatileTexts
                    );
                }

                credit = newCredit;

                if (credit < 0)
                    credit = 0;
            }
        }
        public int Endurium { get; set; } = 0;
        public int Terbium { get; set; } = 0;
        public int Prometium { get; set; } = 0;
        public float Speed { get; set; }

        private float spriteRotation;
        public LaserUpgrade CurrentLaserUpgrade { get; set; }
        public RocketUpgrade MaxRocketUpgrade { get; set; }
        public EquippedRocket EquippedRocket { get; set; }
        public EquippedMine EquippedMine { get; set; }
        public bool HasDCR250 { get; set; }
        public bool HasWIZ { get; set; }
        public bool IsSpeedUpgraded { get; set; }
        public bool HasDamageMine { get; set; }
        public bool HasSlowMine { get; set; }
        public bool HasFirework { get; set; }
        public int NumAlienDestroyed { get; set; }
        public List<Texture2D> ChosenFireworkExplosion { get; set; } = EffectManager.FireworkLargeRedDetonationFrames;
        public bool IsMinigunBonusActive { get; set; }
        public static Waves WaveRecord { get; set; } = Waves.Wave1;

        private EffecT waveRecordBrokenEffect;
        public int CargoBoxCollected { get; set; }

        private float damageMineCooldown = 0;

        public Player(Rectangle rect) : base (rect)
        {
            CurrentLaserUpgrade = LaserUpgrade.LF1;
            MaxRocketUpgrade = RocketUpgrade.R310;
            EquippedRocket = EquippedRocket.R310;
            credit = 0;
            Name = "Mamadex";
            this.rect = rect;
            Frames = Animation.LoadFrames(Path.Combine(AppContext.BaseDirectory, "Content/Sprite/Player/goliath"));

            origin = new(Frames[currentFrame].Width / 2, Frames[currentFrame].Height / 2);

            ApplyLaserStats();
            ApplyRocketStats();
            fireCooldownLeft = 0;
            rocketCooldownLeft = 0;

            IsRotating = true;
            FontPosition = new(Frames[currentFrame].Width / 2, Frames[currentFrame].Height / 2);

            RocketLifeSpan = 1;
            ProjectileLifeSpan = 1;
            Speed = 6f;

            RocketTexture = ProjectileManager.R310Texture;
            DepthLayer = LayerManager.PlayerLayer;

            waveRecordBrokenEffect = EffectManager.AddSimpleEffect(rect.Center.X, rect.Center.Y, EffectManager.LevelUpFrames);
            waveRecordBrokenEffect.DepthLayer = DepthLayer - 0.000001f;
            EffectManager.Effects.Remove(waveRecordBrokenEffect);

            WaveManager.OnWaveRecordBroken += (s, e) =>
            {
                if (!WaveManager.IsWaveRecordBroken)
                {
                    MediaPlayer.Play(SoundManager.LevelUp);
                    UIManager.AddText(rect.Right + 50, rect.Top - 25, $"New Record", Color.Gold,
                        UIManager.infoFont, DepthLayer - 0.0000001f, UIManager.VolatileTexts);
                }
            };

        }

        public override void FireLaser()
        {
            if (fireCooldownLeft > 0)
                return;

            if (IsMinigunBonusActive)
            {
                fireCooldownLeft = FireCooldown / 10;
            }
            else
            {
                fireCooldownLeft = FireCooldown;
            }

            base.FireLaser();
        }

        public void AddCredits(int credits)
        {
            Credit += (long)(credits * GlobalsBonuses.CreditBonus);
        }

        private void LaserInputSystem()
        {
            if (UIManager.IsIntersectUI() || UIManager.CanvasButtonsIntersctTimer > 0)
                return;

            if (IsLaserAuto)
            {
                if (InputManager.MouseLeftDown)
                {
                    FireLaser();
                }
            }
            else
            {
                if (InputManager.MouseLeftClicked)
                {
                    FireLaser();
                }
            }
        }

        public override void FireRocket()
        {
            if(rocketCooldownLeft > 0) 
                return;

            rocketCooldownLeft = RocketCooldown;
            base.FireRocket();
        }

        public override void Update()
        {
            if (fireCooldownLeft > 0)
            {
                fireCooldownLeft -= Globals.DeltaTime;
            }

            if(rocketCooldownLeft > 0)
            {
                rocketCooldownLeft -= Globals.DeltaTime;
            }

            Vector2 playerCenter = new(rect.Center.X, rect.Center.Y);
            Vector2 distance = InputManager.LogicalMouse - playerCenter;
            Rotation = (float)Math.Atan2(distance.Y, distance.X);

            if (IsRotating)
            {
                if (Rotation < 0)
                {
                    Rotation += MathHelper.TwoPi;
                }

                spriteRotation = Rotation + MathHelper.PiOver2 + MathHelper.PiOver2;

                spriteRotation %= MathHelper.TwoPi;

                currentFrame = (int)Math.Abs(Game1.ConvertRadiansToDegrees(spriteRotation) / 360 * Frames.Count);
            }
            else
            {
                base.Update();
            }

            float moveSpeed = Speed * Globals.DeltaTime;

            if (InputManager.GoUp)
            {
                velocity.Y -= moveSpeed;
            }
            if (InputManager.GoDown)
            {
                velocity.Y += moveSpeed;
            }
            if (InputManager.GoLeft)
            {
                velocity.X -= moveSpeed;
            }
            if (InputManager.GoRight)
            {
                velocity.X += moveSpeed;
            }

            if (WaveManager.IsWaveRecordBroken && !waveRecordBrokenEffect.Delete)
            {
                waveRecordBrokenEffect.position = rect.Center.ToVector2();
                waveRecordBrokenEffect.Update();
            }

            LaserInputSystem();

            if (IsRocketAuto)
            {
                if (InputManager.IsKeyDown(Keys.Space))
                {
                    FireRocket();
                }
            }
            else
            {
                if (InputManager.IsKeyPressed(Keys.Space))
                {
                    FireRocket();
                }
            }

            if (InputManager.IsKeyPressed(Keys.X))
            {
                foreach (var firework in MineManager.Mines.OfType<Firework>())
                {
                    firework.Explode();
                }
            }

            damageMineCooldown -= Globals.DeltaTime;

            if (InputManager.IsKeyPressed(Keys.V) && EquippedMine == EquippedMine.DamageMine && HasDamageMine
                && MineManager.Mines.OfType<DamageMine>().Count() < MineManager.damageMineMax && damageMineCooldown <= 0)
            {
                MineManager.AddDamageMine(rect.Center.X, rect.Center.Y, MineManager.DamageMineFrames, 1000000, 400);
                damageMineCooldown = 3;
            }

            if (InputManager.IsKeyPressed(Keys.C) && EquippedMine == EquippedMine.SlowMine && HasSlowMine 
                && MineManager.Mines.OfType<SlowMine>().Count() < MineManager.slowMineMax)
            {
                MineManager.AddSlowMine(rect.Center.X, rect.Center.Y, MineManager.SlowMineFrames, 400);
            }

            if (InputManager.IsKeyPressed(Keys.F) && HasFirework && MineManager.Mines.OfType<Firework>().Count() < MineManager.fireworksMax)
            {
                MineManager.AddFirework(rect.Center.X, rect.Center.Y, MineManager.FireworkFrames, ChosenFireworkExplosion);
            }

            velocity.X = Math.Max(-50, Math.Min(50, velocity.X));
            velocity.Y = Math.Max(-50, Math.Min(50, velocity.Y));

            velocity.X *= 0.95f;
            velocity.Y *= 0.95f;

            position += velocity;


            position.X = MathHelper.Clamp(position.X, 0, Globals.Bounds.X);
            position.Y = MathHelper.Clamp(position.Y, 0, Globals.Bounds.Y);

            rect.X = (int)(position.X - rect.Width / 2);
            rect.Y = (int)(position.Y - rect.Height / 2);

            
            origin.X = Frames[currentFrame].Width / 2;
            origin.Y = Frames[currentFrame].Height / 2;
        }

        public void NextLaserUpgrade()
        {
            if ((int)CurrentLaserUpgrade >= 2)
                return;

            CurrentLaserUpgrade += 1;
            ApplyLaserStats();
        }

        public void NextRocketUpgrade()
        {
            if ((int)MaxRocketUpgrade >= 2)
                return;
            
            MaxRocketUpgrade++;
            EquippedRocket = (EquippedRocket)MaxRocketUpgrade;
            ApplyRocketStats();
        }

        public void ApplyRocketStats()
        {
            switch (EquippedRocket)
            {
                case EquippedRocket.R310:
                    RocketDamage = 1000;
                    RocketCooldown = 3;
                    RocketTexture = ProjectileManager.R310Texture;
                    RocketSpeed = 500;
                    RocketExplosionSize = 400;
                    CurrentRocketType = RocketType.Normal;
                    break;

                case EquippedRocket.PLT2026:
                    RocketDamage = 10000;
                    RocketCooldown = 2;
                    RocketTexture = ProjectileManager.PLT2026Texture;
                    RocketSpeed = 1000;
                    RocketExplosionSize = 800;
                    CurrentRocketType = RocketType.Normal;
                    break;

                case EquippedRocket.PLT3030:
                    RocketDamage = 100000;
                    RocketCooldown = 0.5f;
                    RocketTexture = ProjectileManager.PLT3030Texture;
                    RocketSpeed = 1000;
                    RocketExplosionSize = 1000;
                    CurrentRocketType = RocketType.Normal;
                    break;

                case EquippedRocket.DCR250:
                    RocketDamage = 0;
                    RocketCooldown = 2;
                    RocketTexture = ProjectileManager.DCR250Texture;
                    RocketSpeed = 1000;
                    RocketExplosionSize = 1000;
                    CurrentRocketType = RocketType.DCR250;
                    break;

                case EquippedRocket.WIZ:
                    RocketDamage = 0;
                    RocketCooldown = 2;
                    RocketTexture = ProjectileManager.WIZTexture;
                    RocketSpeed = 1000;
                    CurrentRocketType = RocketType.WIZ;
                    break;
            }
        }

        public void ApplyLaserStats()
        {
            switch (CurrentLaserUpgrade)
            {
                case LaserUpgrade.LF1:
                    ProjectileDamage = 400;
                    FireCooldown = 0.5f;
                    ProjectileTexture = ProjectileManager.BigRedLaser;
                    projectileSounds = SoundManager.RedLaserSounds;
                    ProjectileSpeed = 1000;
                    break;

                case LaserUpgrade.LF2:
                    ProjectileDamage = 2000;
                    FireCooldown = 0.3f;
                    ProjectileTexture = ProjectileManager.BigBlueLaser;
                    projectileSounds = SoundManager.BlueLaserSounds;
                    ProjectileSpeed = 1500;
                    break;

                case LaserUpgrade.LF3:
                    ProjectileDamage = 10000;
                    FireCooldown = 0.2f;
                    ProjectileTexture = ProjectileManager.BigWhiteLaser;
                    projectileSounds = SoundManager.WhiteLaserSound;
                    ProjectileSpeed = 2000;
                    break;
            }
        }

        public override void Draw()
        {
            var hitboxTextureOpacity = 0.3f;

            if (IsRotating)
                DrawAngleCorrelatedFrames(currentFrame);
            else
                DrawFrames();

            if (Globals.IsDebugMode)
            {
                Globals.SpriteBatch.Draw(HitboxTexture, rect, null, Color.White * hitboxTextureOpacity, 0, origin, 0, DepthLayer);
            }
            DrawFont(Name, Color.White, 15);

            if (WaveManager.IsWaveRecordBroken && !waveRecordBrokenEffect.Delete)
            {
                waveRecordBrokenEffect.DrawFrames();
            }
        }
    }
}
