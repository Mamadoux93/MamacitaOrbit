using MamacitaOrbit.Main;
using MamacitaOrbit.Managers;
using MamacitaOrbit.Objects.Interfaces;
using MamacitaOrbit.Objects.Types;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MamacitaOrbit.Objects.Sprites.InHeritance
{
    internal abstract class AttackingSprite : Sprite, IShooter
    {
        public int ProjectileSpeed { get; set; }
        public int ProjectileLifeSpan { get; set; }
        public int ProjectileDamage { get; set; }
        public int RocketDamage { get; set; }
        public int RocketSpeed { get; set; }
        public int RocketLifeSpan { get; set; }
        public bool IsAttacking { get; set; } = false;
        public Texture2D ProjectileTexture { get; set; }
        public Texture2D RocketTexture { get; set; }

        public Rectangle rangeRect;

        protected Texture2D rangeRectTexture;

        protected float projectileSpawnTime;

        protected SoundEffect[] projectileSounds;
        public RocketType CurrentRocketType { get; set; }
        public int RocketExplosionSize { get; set; }
        
        public float ProjectileSpawnTime
        {
            get
            {
                return projectileSpawnTime;
            }
            set
            {
                if (projectileSpawnTime <= 0)
                    projectileSpawnTime = 0;
                projectileSpawnTime = value;
            }
        }
        protected float projectileCooldown;

        // Player
        public AttackingSprite(Texture2D texture, Rectangle rect) : base(rect)
        {
            this.rect = rect;
            this.Texture = texture;

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

        }
        // Alien
        public AttackingSprite(Rectangle rect)
        {
            this.rect = rect;

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
        }

        public AttackingSprite()
        {

        }


        public virtual void FireLaser()
        {
            if (projectileSounds != null && projectileSounds.Length > 0)
            {
                var laserSound = Globals.Random.Next(0, projectileSounds.Length);

                SoundManager.Play(projectileSounds[laserSound], SoundManager.GlobalVolume);
            }

            ProjectileData projectileData = new ProjectileData()
            {
                Texture = ProjectileTexture,
                Position = rect.Center.ToVector2(),
                Rotation = Rotation,
                Lifespan = ProjectileLifeSpan,
                Speed = ProjectileSpeed,
                Damage = ProjectileDamage,
                ShooterType = GetType(),
                DepthLayer = DepthLayer,
            };

            ProjectileManager.AddLaser(projectileData);
        }

        public virtual void FireRocket()
        {
            if (projectileSounds != null && projectileSounds.Length > 0)
            {
                if (Globals.Player.MaxRocketUpgrade == RocketUpgrade.PLT3030)
                {
                    SoundManager.Play(SoundManager.AdvancedRocketSound, SoundManager.GlobalVolume);
                }
                else
                {
                    SoundManager.Play(SoundManager.NormalRocketSound, SoundManager.GlobalVolume);
                }
            }

            ProjectileData projectileData = new ProjectileData()
            {
                Texture = RocketTexture,
                Position = rect.Center.ToVector2(),
                Rotation = Rotation,
                Lifespan = RocketLifeSpan,
                Speed = RocketSpeed,
                Damage = RocketDamage,
                ShooterType = GetType(),
                DepthLayer = DepthLayer,
                ExplosionSize = RocketExplosionSize,
                RocketType = CurrentRocketType,
            };

            ProjectileManager.AddRocket(projectileData);
        }

        public override void Update()
        {
            base.Update();
        }
    }
}
