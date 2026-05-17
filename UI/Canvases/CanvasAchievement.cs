using MamacitaOrbit.Managers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MamacitaOrbit.UI.Canvases
{
    internal class CanvasAchievement : Canvas
    {
        private float displayTimer = 0;

        private Text description;

        private readonly float incrementDisplayTimer = 5;

        public CanvasAchievement(Texture2D texture, Rectangle rect, float depthLayer, string name = "Untilted Canvas", bool isDraggable = true, bool isScissorCanvas = false) : 
            base(texture, rect, depthLayer, name, isDraggable, isScissorCanvas)
        {
            description = AddText(100, 70, "", Color.White, font);
            description.ParentPosition = new Vector2(Rect.X, Rect.Y);
        }

        public void ChangeNameDescription(string name, string description)
        {
            Called = true;
            Name = name;
            CanvasTitle.Message = name;
            Vector2 textSize = font.MeasureString(description);

            if (textSize.X > Rect.Width)
            {
                Rect = new Rectangle(Rect.X, Rect.Y, (int)(textSize.X * 1.5), Rect.Height);
            }

            this.description.Message = description;

            CanvasTitle.Position = new Vector2(
                Rect.Width / 2 - font.MeasureString(CanvasTitle.Message).X / 2,
                10
            );

            this.description.Position = new Vector2(
                Rect.Width / 2f - textSize.X / 2f,
                Rect.Height / 2f - textSize.Y / 2f
            );

            displayTimer = incrementDisplayTimer;

            SoundManager.Play(SoundManager.AchievementUnlockedSound, 5);
        }

        public override void Update()
        {
            base.Update();
            if (displayTimer > 0)
            {
                displayTimer -= Globals.DeltaTime;
            }
            else 
            {
                Called = false;
            }
        }

        public override void Draw()
        {
            base.Draw();
        }
    }
}
