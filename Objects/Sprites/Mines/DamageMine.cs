using MamacitaOrbit.Main;
using MamacitaOrbit.Managers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MamacitaOrbit.Objects.Sprites.Mines
{
    internal class DamageMine : Mine
    {
        public int Damage { get; set; }
        public int MineRadius { get; set; }

        public DamageMine(List<Texture2D> frames, Rectangle rectangle) : base(frames, rectangle)
        {

        }

        public override void Update()
        {
            base.Update();
            foreach(var alien in AlienManager.Aliens)
            {
                if(alien.rect.Intersects(rect))
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

            Rectangle explosionRadius = new Rectangle((int)(rect.X - MineRadius / 2), (int)(rect.Y - MineRadius / 2), 
                MineRadius, MineRadius);

            foreach (var alien in AlienManager.Aliens.Where(alien => alien.rect.Intersects(explosionRadius)))
            {
                alien.TakeDamage(Damage);
            }

            Detonated = true;

            SoundManager.Play(SoundManager.DamageFromExplosionSound, 0.8f);
        }

        public override void Draw()
        {
            base.Draw();
        }
    }
}
