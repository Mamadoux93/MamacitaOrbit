using MamacitaOrbit.Main;
using MamacitaOrbit.UI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace MamacitaOrbit.UI
{
    internal class Text
    {
        public string Message { get; set; }
        private SpriteFont font;
        private float depthLayer;
        public Color Color { get; set; }
        public Vector2 Position { get; set; }

        // The ParentPosition is for canvas's texts that need to follow the canvas position.
        // In that case, the property Position becomes an offset :).

        public Vector2 ParentPosition { get; set; }
        public float DeleteTimer { get; set; }
        public float BullshitDividerForFuckingVolatileTexts { get; private set; }
        public float Opacity { get; set; }

        public Text(Vector2 position, string message, SpriteFont font, Color color, float depth, float deleteTimer = 3) 
        { 
            Position = position;
            Message = message;
            Color = color;
            depthLayer = depth;
            this.font = font;
            Opacity = 1;
            DeleteTimer = deleteTimer;
            BullshitDividerForFuckingVolatileTexts = DeleteTimer;
        }

        public void Draw()
        {
            Vector2 finalPosition = Position + ParentPosition;

            Globals.SpriteBatch.DrawString(font, Message,
                    finalPosition,
                    Color * Opacity, 0, Vector2.Zero, 1, 0, depthLayer);
        }
    }
}
