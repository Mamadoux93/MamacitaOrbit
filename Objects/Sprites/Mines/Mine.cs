using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MamacitaOrbit.Objects.Sprites.InHeritance;

namespace MamacitaOrbit.Objects.Sprites.Mines
{
    internal abstract class Mine : Sprite
    {
        public bool Detonated { get; set; } = false;
        public Mine(List<Texture2D> frames, Rectangle rectangle) 
        { 
            this.rect = rectangle;
            this.Frames = frames;

            HitboxTexture.SetData(new[] { Color.Orange });
            if (frames.Count > 0 && frames is not null)
            {
                origin = new(frames[currentFrame].Width / 2, frames[currentFrame].Height / 2);
            }
            position = new(rect.Center.X, rect.Center.Y);
        }
        public abstract void Explode();
    }
}
