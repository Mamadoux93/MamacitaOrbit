using MamacitaOrbit.Main;
using MamacitaOrbit.UI.Buttons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MamacitaOrbit.UI.Bars
{
    internal class ItemsBar
    {
        public List<Button> Buttons { get; set; }
        public bool IsHidden { get; set; }

        public ItemsBar(Vector2 position, params Button[] buttons) 
        {
            Buttons = new List<Button>(buttons);
            int buttonSize = 50;

            for (int i = 0; i < Buttons.Count; i++)
            {
                var button = Buttons[i];
                button.Rect = new((int)position.X + buttonSize * i, (int)position.Y, buttonSize, buttonSize);
            }
        }

        public ItemsBar CreateSubItemsBar(Vector2 position, params Button[] buttons)
        {
            return new ItemsBar(position, buttons);
        }

        public void Update()
        {
            if (IsHidden)
                return;

            foreach (var button in Buttons)
            {
                button.Update();
            }
        }

        public void Draw()
        {
            if (IsHidden)
                return;

            foreach (var button in Buttons)
            {
                button.Draw();
            }
        }
    }
}
