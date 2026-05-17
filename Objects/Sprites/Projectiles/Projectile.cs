using MamacitaOrbit.Main;
using MamacitaOrbit.Managers;
using MamacitaOrbit.Objects.Interfaces;
using MamacitaOrbit.Objects.Sprites.InHeritance;
using MamacitaOrbit.Objects.Types;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MamacitaOrbit.Objects.Sprites.Projectiles
{
    internal abstract class Projectile : Sprite
    {
        public Vector2 Direction { get; set; }
        public float Lifespan { get; set; }
        public float Speed { get; set; }
        public int Damage { get; set; }
        public Type ShooterType { get; set; }
        public bool IsFromAlly => typeof(IAlly).IsAssignableFrom(ShooterType);

        protected float textureScale = 0.8f;

        public Projectile(ProjectileData data) : base(data)
        {
            Speed = data.Speed;
            Rotation = data.Rotation;
            Direction = new((float)Math.Cos(Rotation), (float)Math.Sin(Rotation));
            Lifespan = data.Lifespan;
            Damage = data.Damage;
            ShooterType = data.ShooterType;
        }

        public override void Update()
        {
            position += Direction * Speed * Globals.DeltaTime;
            Lifespan -= Globals.DeltaTime;

            foreach (var alien in AlienManager.Aliens)
            {
                if (!alien.rect.Contains(position))
                    continue;

                if (alien.IsGettingDestroyed)
                    continue;

                if (!IsFromAlly)
                    continue;
                
                Destroy();
                alien.TakeDamage(Damage);
                break;     
            }
        }

        public virtual void Destroy()
        {
            Lifespan = 0;
        }

        public override void Draw()
        {
            Globals.SpriteBatch.Draw(Texture, position, null, Color.White, Rotation, origin, textureScale, SpriteEffects.FlipHorizontally, DepthLayer);
        }
    }
}
