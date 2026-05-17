using MamacitaOrbit.Main;
using MamacitaOrbit.Managers;
using MamacitaOrbit.Objects.Types;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MamacitaOrbit.Objects.Sprites.Projectiles
{
    internal class Rocket : Projectile
    {
        private readonly int explosionSize;
        public RocketType RocketType { get; set; }
        public Rocket(ProjectileData data) : base(data)
        {
            explosionSize = data.ExplosionSize;
            RocketType = data.RocketType;
            
            textureScale = 1;
        }

        public override void Destroy()
        {
            if (Lifespan == 0)
                return;

            Rectangle explosionRadius = new Rectangle(
                (int)(position.X - explosionSize / 2),
                (int)(position.Y - explosionSize / 2),
                explosionSize,
                explosionSize
            );

            var aliens = AlienManager.Aliens.Where(a => a.rect.Intersects(explosionRadius));

            switch (RocketType)
            {
                case RocketType.Normal:
                    foreach (var alien in aliens)
                        alien.TakeDamage(Damage);
                    break;

                case RocketType.DCR250:
                    foreach (var alien in aliens)
                        alien.IsSlowed = true;
                    break;

                case RocketType.WIZ:
                    foreach (var alien in aliens)
                    {
                        var alienTextures = AlienManager.AlienTexturesForWIZ[Globals.Random.Next(0, AlienManager.AlienTexturesForWIZ.Count - 1)];

                        if (alienTextures.Count > alien.Frames.Count)
                            continue;

                        alien.currentFrame = 0;
                        alien.Frames = alienTextures;
                    }
                    break;
            }

            SoundManager.Play(SoundManager.DamageFromExplosionSound, 0.8f);

            base.Destroy();
        }
    }
}