using MamacitaOrbit.Main;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MamacitaOrbit.Managers;
using System.Diagnostics;
using MamacitaOrbit.UI.Interfaces;
using MamacitaOrbit.UI.Canvases;

namespace MamacitaOrbit.UI.Buttons
{
    internal class Button : IUIObjects
    {
        public static SpriteFont buttonFont = Globals.Content.Load<SpriteFont>("Fonts/info_font");
        protected Texture2D texture;
        public bool IsIntersectable { get; set; }
        public Rectangle Rect { get; set; }
        private Color ButtonColor { get; set; }

        public event EventHandler OnClick;
        public Color QuandTuPassesAuDessusAvecTonCurseur { get; set; }
        public Color CurrentColor { get; set; }
        private Texture2D dessusTexture;

        private Texture2D currentTexture;

        private bool isTextureOnly = false;
        public float DepthLayer { get; set; }
        public string ButtonText { get; set; }
        public Color? TextColor { get; set; }

        private bool isHovered;

        public Icon Icon { get; set; }

        public Button(Texture2D texture, Rectangle rect, Color normalColor, Color dessusColor, string buttonText = null, Icon icon = null)
        {
            this.texture = texture;
            Rect = rect;
            ButtonColor = normalColor;
            QuandTuPassesAuDessusAvecTonCurseur = dessusColor;
            CurrentColor = ButtonColor;
            (this as IUIObjects).RegisterUIObject();
            ButtonText = buttonText;
            if(TextColor == null)
            {
                TextColor = Color.Black;
            }
        }

        public Button(Texture2D texture, Rectangle rect, Texture2D dessusTexture, string buttonText = null, Icon icon = null)
        {
            this.texture = texture;
            Rect = rect;
            this.dessusTexture = dessusTexture;
            isTextureOnly = true;
            CurrentColor = Color.White;
            (this as IUIObjects).RegisterUIObject();
            ButtonText = buttonText;
            currentTexture = this.texture;
            if (TextColor == null)
            {
                TextColor = Color.Black;
            }

            Icon = icon;
            Icon?.Rectangle = new(rect.Center.X - Icon.Rectangle.Width / 2, rect.Center.Y - Icon.Rectangle.Height / 2, 
                Icon.Rectangle.Width, Icon.Rectangle.Height);
        }

        private void HandlingClickAndTexture()
        {
            isHovered = Rect.Contains(InputManager.LogicalMouse.ToPoint());

            if (isHovered && InputManager.MouseLeftClicked)
            {
                Click();
            }

            if (!isTextureOnly)
            {
                CurrentColor = isHovered ? QuandTuPassesAuDessusAvecTonCurseur : ButtonColor;
            }
            else if (isHovered)
            {

            }

            if (isTextureOnly)
            {
                currentTexture = isHovered ? dessusTexture : texture;
            }
        }

        public virtual void Update()
        {
            IsIntersectable = true;
            HandlingClickAndTexture();

            Icon?.Rectangle = new(Rect.Center.X - Icon.Rectangle.Width / 2, Rect.Center.Y - Icon.Rectangle.Height / 2,
                Icon.Rectangle.Width, Icon.Rectangle.Height);
        }

        protected virtual void Click()
        {
            OnClick?.Invoke(this, EventArgs.Empty);
            SoundManager.Play(SoundManager.ButtonClickSound, 5);
        }

        public virtual void Draw()
        {
            if (isTextureOnly)
            {
                Globals.SpriteBatch.Draw(currentTexture, Rect, null, CurrentColor, 0, Vector2.Zero, 0, DepthLayer);
            }
            else
            {
                Globals.SpriteBatch.Draw(texture, Rect, null, CurrentColor, 0, Vector2.Zero, 0, DepthLayer);
            }

            if (ButtonText != null)
            {
                var textLength = buttonFont.MeasureString(ButtonText);

                if(TextColor == null)
                {
                    TextColor = Color.Black;
                }

                Globals.SpriteBatch.DrawString(buttonFont, ButtonText, 
                    new Vector2((int)(Rect.Center.X - textLength.X / 2), Rect.Center.Y - textLength.Y / 2), 
                    (Color)TextColor, 0, Vector2.Zero, 1, 0, DepthLayer / 1.1f);
            }

            Icon?.Draw();
        }
    }
}
