using MamacitaOrbit.Main;
using MamacitaOrbit.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MamacitaOrbit.Objects.Sprites.Mines
{
    internal class Firework : Mine
    {
        public List<Texture2D> CurrentFireworkExplosion { get; set; }
        public Firework(List<Texture2D> frames, Rectangle rectangle) : base(frames, rectangle)
        {

        }

        public override void Explode()
        {
            var fireworkSound = Globals.Random.Next(0, SoundManager.FireworkExplosionSounds.Length);

            SoundManager.Play(SoundManager.FireworkExplosionSounds[fireworkSound], 2f);

            EffectManager.AddSimpleEffect(rect.Center.X, rect.Center.Y, 
                CurrentFireworkExplosion, 1.5f);

            Detonated = true;
        }
    }
}
