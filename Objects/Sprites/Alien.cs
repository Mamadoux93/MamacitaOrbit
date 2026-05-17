using MamacitaOrbit.Main;
using MamacitaOrbit.Managers;
using MamacitaOrbit.Objects.Interfaces;
using MamacitaOrbit.Objects.Sprites.InHeritance;
using MamacitaOrbit.Objects.Types;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MamacitaOrbit.Objects.Sprites
{
    internal class Alien : DestroyableSprite, IDestroyable, IShooter
    {
        public AlienType Type { get; set; }
        public float Speed { get; set; }
        public int Endurium {  get; set; }
        public int Terbium { get; set; }
        public int Prometium { get; set; }

        private float spriteRotation;

        private float slowTimer;
        private readonly float slowCooldown;
        private static readonly float slowSpeedMultiplier = 0.5f;
        public bool IsSlowed { get; set; }

        private EffecT slow1Effect;
        private EffecT slow2Effect;

        public Alien(AlienType type, Rectangle rect) : base(type, rect)
        {
            this.rect = rect;
            Type = type;

            Frames = Type.Frames;
            Health = Type.Health;
            RewardCredit = Type.RewardCredit;
            RewardUridium = Type.RewardUridium;

            Speed = Type.AlienSpeed; 
            IsRotating = Type.IsRotating;

            ProjectileTexture = Type.ProjectileTexture;

            position = new Vector2(rect.Center.X, rect.Center.Y);

            rangeRect = new(rect.X, rect.Y, rect.Width * 2, rect.Height * 2)
            {
                X = rect.Center.X - rect.Width,
                Y = rect.Center.Y - rect.Height,
            };

            rangeRectTexture = HitboxTexture;
            {
                rangeRectTexture.SetData(new[] 
                { 
                    Color.Crimson 
                });
            }

            ProjectileSpeed = Type.ProjectileSpeed;
            ProjectileLifeSpan = Type.ProjectileLifeSpan;
            ProjectileDamage = Type.ProjectileDamage;

            ProjectileSpawnTime = 1;
            projectileCooldown = ProjectileSpawnTime;

            slowTimer = 3;
            slowCooldown = slowTimer;

            MaxHealth = Health;

            Endurium = Type.Endurium;
            Terbium = Type.Terbium;
            Prometium = Type.Prometium;

            ExplosionFrames = Type.ExplosionFrames;

            projectileSounds = Type.ProjectileSounds;

            origin = new(Type.Frames[currentFrame].Width / 2, Type.Frames[currentFrame].Height / 2);

            slow1Effect = EffectManager.AddSimpleEffect(rect.Center.X, rect.Center.Y, EffectManager.Slow1Frames, isNonEndingEffect: true);
            slow2Effect = EffectManager.AddSimpleEffect(rect.Center.X, rect.Center.Y, EffectManager.Slow2Frames, isNonEndingEffect: true);
            EffectManager.Effects.Remove(slow1Effect);
            EffectManager.Effects.Remove(slow2Effect);
        }

        public override void Destroy()
        {
            EffectManager.AddExplosion(this);

            var chanceToSpawnPowerUp = Globals.Random.Next(0, 99);

            base.Destroy();

            if(AlienManager.IsNeutronBombDetonated)
                return;

            if (chanceToSpawnPowerUp < 5)
            {
                CollectableManager.AddPowerUp(rect.Center.X, rect.Center.Y);
                return;
            }
            
            if (Terbium + Endurium + Prometium != 0)
            {
                CollectableManager.AddCargoBox(this);
            }
        }

        private void Slow()
        {
            if (!IsSlowed)
                return;

            slowTimer -= Globals.DeltaTime;

            Speed = Type.AlienSpeed;
            Speed *= slowSpeedMultiplier;

            if (slowTimer <= 0)
            {
                IsSlowed = false;
                slowTimer = slowCooldown;
                Speed = Type.AlienSpeed;
            }
        }

        public override void Update()
        {
            origin = new(Type.Frames[currentFrame].Width / 2, Type.Frames[currentFrame].Height / 2);

            float moveSpeed = Speed * Globals.DeltaTime;

            Vector2 baseCenter = new(Globals.Base.rect.Center.X,Globals.Base.rect.Center.Y);

            Vector2 direction = baseCenter - position;

            if (direction != Vector2.Zero)
            {
                direction.Normalize();
            }

            Rotation = (float)Math.Atan2(direction.Y, direction.X);

            if (!IsRotating)
            {
                base.Update();
            }
            else
            {
                spriteRotation = Rotation + MathHelper.PiOver2 + MathHelper.PiOver2;

                spriteRotation %= MathHelper.TwoPi;

                currentFrame = (int)Math.Abs(Game1.ConvertRadiansToDegrees(spriteRotation) / 360 * Frames.Count);
            }

            if (Rotation < 0)
            {
                Rotation += MathHelper.TwoPi;
            }

            if (IsSlowed)
            {
                spriteRotation = Rotation + MathHelper.PiOver2 + MathHelper.PiOver2;

                spriteRotation %= MathHelper.TwoPi;

                if (IsAttacking)
                {
                    slow2Effect.Update();
                    slow2Effect.position = position;
                }
                else
                {
                    slow1Effect.Update();
                    slow1Effect.position = position;
                }
            }

            ProjectileSpawnTime -= Globals.DeltaTime;

            if (rangeRect.Intersects(Globals.Base.rect))
            {
                IsAttacking = true;
            }

            if (IsAttacking && ProjectileSpawnTime <= 0)
            {
                FireLaser();
                ProjectileSpawnTime = projectileCooldown;
                
            }
            else if(!IsAttacking) 
            {
                position += direction * moveSpeed;

                rect.X = (int)(position.X - rect.Width / 2);
                rect.Y = (int)(position.Y - rect.Height / 2);

                rangeRect.X = rect.Center.X - rangeRect.Width / 2;
                rangeRect.Y = rect.Center.Y - rangeRect.Height / 2;
            }

            if(Health <= 0)
            {
                Destroy();
            }

            Slow();
        }

        public override void Draw()
        {
            var hitboxTextureOpacity = 0.5f;

            if (IsRotating)
                base.DrawAngleCorrelatedFrames(currentFrame);
            else
                base.DrawFrames();

            if (Globals.IsDebugMode)
            {
                Globals.SpriteBatch.Draw(HitboxTexture, rect, Color.White * hitboxTextureOpacity);
                Globals.SpriteBatch.Draw(rangeRectTexture, rangeRect, Color.White * hitboxTextureOpacity);
            }

            if (IsSlowed)
            {
                if(IsAttacking)
                    slow2Effect.DrawFrames(spriteRotation, slow2Effect.DrawScale);
                else
                    slow1Effect.DrawFrames(spriteRotation, slow1Effect.DrawScale);
            }

            base.HealthBarDraw(MaxHealth);

            DrawFont($"{Type}", Color.Red, 10);
        }
    }
}
