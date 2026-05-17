using MamacitaOrbit.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace MamacitaOrbit.Objects.Sprites.Mines
{
    internal class SlowMine : DamageMine
    {
        public SlowMine(List<Texture2D> frames, Rectangle rectangle) : base(frames, rectangle)
        {

        }
        public override void Update()
        {
            base.Update();
            foreach (var alien in AlienManager.Aliens)
            {
                if (alien.rect.Intersects(rect))
                {
                    Explode();
                    break;
                }
            }
        }

        public override void Explode()
        {
            if (Detonated)
                return;

            Rectangle explosionRadius = new Rectangle((rect.X - MineRadius / 2), (rect.Y - MineRadius / 2),
                MineRadius, MineRadius);

            foreach (var alien in AlienManager.Aliens.Where(alien => alien.rect.Intersects(explosionRadius)))
            {
                alien.IsSlowed = true;
            }

            Detonated = true;

            SoundManager.Play(SoundManager.DamageFromExplosionSound, 0.8f);
        }
    }
}
