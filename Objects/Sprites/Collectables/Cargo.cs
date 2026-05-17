using MamacitaOrbit.Main;
using MamacitaOrbit.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MamacitaOrbit.Objects.Sprites.Collectables
{
    internal class Cargo : Collectable
    {
        public int Endurium { get; set; }
        public int Prometium { get; set; }
        public int Terbium { get; set; }
        public Cargo(int endurium, int prometium, int terbium, Rectangle rect) : base()
        {
            Endurium = endurium;
            Prometium = prometium;
            Terbium = terbium;

            this.rect = rect;
            position = new(rect.Center.X, rect.Center.Y);
            HitboxTexture.SetData(new[] { Color.Blue });

            Frames = CollectableManager.ClassicCargoFrames;
            origin = new(Frames[currentFrame].Width / 2, Frames[currentFrame].Height / 2);
        }
    }
}
