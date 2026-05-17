using MamacitaOrbit.Main;
using MamacitaOrbit.Managers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MamacitaOrbit.Objects.Sprites.BattleStation
{
    internal sealed class AttackTurret : Turret
    {
        private enum TurretState
        {
            Idle,
            Attack
        }

        private int rangeMultiplier;
        private int rangeWidth;
        private int rangeHeight;

        private float attackAnimationTime;

        private TurretState currentState;

        private Texture2D attackTexture;
        public static readonly Texture2D level1IdleTexture = Globals.Content.Load<Texture2D>("Sprite/BattleStation/modules/module_laser1/1");
        private readonly Texture2D level1AttackTexture = Globals.Content.Load<Texture2D>("Sprite/BattleStation/modules/module_laser1/2");

        public static readonly Texture2D level2IdleTexture = Globals.Content.Load<Texture2D>("Sprite/BattleStation/modules/module_laser2/1");
        private readonly Texture2D level2AttackTexture = Globals.Content.Load<Texture2D>("Sprite/BattleStation/modules/module_laser2/2");

        public static readonly Texture2D level3IdleTexture = Globals.Content.Load<Texture2D>("Sprite/BattleStation/modules/module_laser3/1");
        private readonly Texture2D level3AttackTexture = Globals.Content.Load<Texture2D>("Sprite/BattleStation/modules/module_laser3/2");

        public AttackTurret(Rectangle rect) : base(rect)
        {
            projectileSounds = SoundManager.RedLaserSounds;

            projectileCooldown = ProjectileSpawnTime;
            rangeMultiplier = 2;
            rangeWidth = this.rect.Width * rangeMultiplier;
            rangeHeight = this.rect.Height * rangeMultiplier;

            rangeRect = new(
                this.rect.Center.X - rangeWidth / 2,
                this.rect.Center.Y - rangeHeight / 2,
                rangeWidth, rangeHeight
                );
            UpgradeTurret();

            origin = new(Texture.Width / 2, Texture.Height / 2);
        }

        public AttackTurret()
        {

        }

        public override void UpgradeTurret()
        {
            switch(CurrentLevel)
            {
                case TurretLevel.Level1:
                    ProjectileDamage = 100000;
                    ProjectileLifeSpan = 2;
                    ProjectileSpeed = 2000;
                    ProjectileSpawnTime = 5f;
                    rangeMultiplier = 2;
                    Texture = level1IdleTexture;
                    attackTexture = level1AttackTexture;
                    ProjectileTexture = ProjectileManager.RedLaserTexture;
                    break;

                case TurretLevel.Level2:
                    ProjectileDamage = 100000;
                    ProjectileSpeed = 4000;
                    ProjectileLifeSpan = 3;
                    ProjectileSpawnTime = 3f;
                    rangeMultiplier = 3;
                    Texture = level2IdleTexture;
                    attackTexture = level2AttackTexture;
                    ProjectileTexture = ProjectileManager.RedLaserTexture;
                    break;

                case TurretLevel.Level3:
                    ProjectileDamage = 300000;
                    ProjectileSpeed = 4000;
                    ProjectileLifeSpan = 4;
                    ProjectileSpawnTime = 1f;
                    rangeMultiplier = 3;
                    Texture = level3IdleTexture;
                    attackTexture = level3AttackTexture;
                    ProjectileTexture = ProjectileManager.RedLaserTexture;
                    break;

                case TurretLevel.LevelMax:
                    ProjectileDamage = 1000000;
                    ProjectileLifeSpan = 5;
                    ProjectileSpeed = 6000;
                    ProjectileSpawnTime = 1f;
                    rangeMultiplier = 4;
                    Texture = level3IdleTexture;
                    attackTexture = level3AttackTexture;
                    ProjectileTexture = ProjectileManager.BigRedLaser;
                    break;
            }

            projectileCooldown = ProjectileSpawnTime;

            rangeWidth = rect.Width * rangeMultiplier;
            rangeHeight = rect.Height * rangeMultiplier;

            rangeRect = new(
                rect.Center.X - rangeWidth / 2,
                rect.Center.Y - rangeHeight / 2,
                rangeWidth, rangeHeight
                );
        }


        private void AttackAlien()
        {
            if (ProjectileSpawnTime > 0)
                return;

            if (AlienManager.Aliens.Count == 0)
                return;

            foreach (var alien in AlienManager.Aliens.Where(alien => alien.rect.Intersects(rangeRect)))
            {
                Vector2 alienCenter = new(alien.rect.Center.X, alien.rect.Center.Y);
                Vector2 direction = alienCenter - position;
                if (direction != Vector2.Zero)
                {
                    direction.Normalize();
                }

                Rotation = (float)Math.Atan2(direction.Y, direction.X);

                FireLaser();
                ProjectileSpawnTime = projectileCooldown;
                attackAnimationTime = ProjectileSpawnTime / 2;
                break;
            }
        }

        public override void Update()
        {
            ProjectileSpawnTime -= Globals.DeltaTime;
            attackAnimationTime -= Globals.DeltaTime;

            base.Update();
            AttackAlien();
            if (attackAnimationTime > 0)
            {
                currentState = TurretState.Attack;
            }
            else
            {
                currentState = TurretState.Idle;
            }
        }

        public override void Draw()
        {

            var textureScale = 0.8f;

            if (currentState == TurretState.Attack)
            {
                Globals.SpriteBatch.Draw(attackTexture, position, null, Color.White, 0f, origin, textureScale, SpriteEffects.None, 0.6f);
            }
            else
            {
                Globals.SpriteBatch.Draw(Texture, position, null, Color.White, 0f, origin, textureScale, SpriteEffects.None, 0.6f);
            }

            if (Globals.IsDebugMode)
            {
                var hitboxTextureOpacity = 0.5f;
                Globals.SpriteBatch.Draw(rangeRectTexture, rangeRect, null, Color.White * hitboxTextureOpacity);
            }
        }
    }
}
