using MamacitaOrbit.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MamacitaOrbit.UI
{
    internal class Icon
    {
        public Rectangle Rectangle { get; set; }
        public Texture2D Texture { get; set; }
        public float Opacity { get; set; }
        public float DepthLayer { get; set; }
        public float Rotation { get; set; }

        protected Texture2D hitboxTexture = new Texture2D(Globals.GraphicsDevice, 1, 1);

        private Vector2 origin;
        public Icon(Texture2D texture, Rectangle rectangle, float depthlayer, float opacity = 1f, float rotation = 0) 
        { 
            Texture = texture;
            Rectangle = rectangle;
            Opacity = opacity;
            DepthLayer = depthlayer;
            Rotation = rotation;

            origin = new(Texture.Width / 2, Texture.Height / 2);

            hitboxTexture.SetData(new[] { Color.Blue });
        }

        public void Draw()
        {
            Vector2 position = new(
                Rectangle.X + Rectangle.Width / 2f,
                Rectangle.Y + Rectangle.Height / 2f
            );

            Vector2 scale = new(
                (float)Rectangle.Width / Texture.Width,
                (float)Rectangle.Height / Texture.Height
            );

            Globals.SpriteBatch.Draw(
                Texture,
                position,
                null,
                Color.White * Opacity,
                Rotation,
                origin,
                scale,
                SpriteEffects.None,
                DepthLayer
            );

            if (Globals.IsDebugMode)
            {
                Globals.SpriteBatch.Draw(hitboxTexture, Rectangle, Color.Blue * 0.3f);
            }
        }
    }
}
