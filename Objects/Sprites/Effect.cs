using MamacitaOrbit.Main;
using MamacitaOrbit.Objects.Sprites.InHeritance;
using Microsoft.VisualBasic.FileIO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MamacitaOrbit.Objects.Sprites
{
    internal class EffecT : Sprite
    {
        public bool IsNonEnding { get; set; }
        public float DeleteCooldown { get; set; }
        public float DrawScale { get; set; }

        public EffecT(List<Texture2D> frames, Vector2 position)
        {
            this.position = position;
            Frames = frames;
            HitboxTexture.SetData(new[] { Color.Blue });

            if (frames is not null && frames.Count > 0)
            {
                origin = new(frames[currentFrame].Width / 2, frames[currentFrame].Height / 2);
            }

            deleteCooldown = frameTime * this.Frames.Count;

            DeleteCooldown = deleteCooldown;
        }

        public override void Update()
        {
            if (IsNonEnding)
            {
                deleteCooldown = DeleteCooldown;
            }
            base.Update();
        }
    }
}
