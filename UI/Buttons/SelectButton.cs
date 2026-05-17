using MamacitaOrbit.Managers;
using MamacitaOrbit.UI.Interfaces;

namespace MamacitaOrbit.UI.Buttons
{
    internal class SelectButton : Button, IUIObjects
    {
        private Texture2D selectedTexture;
        public bool IsSelected { get; set; }
        public bool AllowUnSelection { get; set; }

        public SelectButton(Texture2D texture, Rectangle rect, Texture2D selectedTexture) : 
            base(texture, rect, selectedTexture)
        { 
            this.selectedTexture = selectedTexture;
        }

        public SelectButton(Texture2D texture, Rectangle rect, Color selectedColor, Color unselectedColor, string buttonText = null) : 
            base(texture, rect, selectedColor, unselectedColor, buttonText) 
        {
            selectedTexture = texture;
        }

        public override void Update()
        {
            if (Rect.Contains(InputManager.LogicalMouse.ToPoint()) && InputManager.MouseLeftClicked)
            {
                Click();
            }
        }

        protected override void Click()
        {
            if (AllowUnSelection)
            {
                IsSelected = !IsSelected;
            }
            base.Click();
        }

        public override void Draw()
        {
            if (IsSelected)
            {
                Globals.SpriteBatch.Draw(selectedTexture, Rect, null, CurrentColor, 0, Vector2.Zero, 0, DepthLayer);
            }
            else
            {
                Globals.SpriteBatch.Draw(texture, Rect, null, CurrentColor, 0, Vector2.Zero, 0, DepthLayer);
            }

            base.Draw();
        }
    }
}
