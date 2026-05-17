using MamacitaOrbit.Main;
using MamacitaOrbit.Managers;
using MamacitaOrbit.UI.Buttons;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MamacitaOrbit.UI.Canvases
{
    internal sealed class CanvasSettings : Canvas
    {

        private Texture2D unselectTexture = Globals.Content.Load<Texture2D>("UI/iconsAndShii/716");
        private Texture2D selectTexture = Globals.Content.Load<Texture2D>("UI/iconsAndShii/718");

        private List<Texture2D> backgrounds = new List<Texture2D>();

        private readonly string backgroundFolderPath = Path.Combine
            (AppContext.BaseDirectory, "Content", "Backgrounds", "old");

        private SelectButton WASDButton;
        private SelectButton ZQSDButton;

        private SelectButton TransparentCanvasButton;

        private Button previousBackground;
        private Button nextBackground;

        public Texture2D CurrentBackground { get; private set; }

        public CanvasSettings(Texture2D texture, Rectangle rect, float depthLayer, string name, bool isDraggable, bool isScissorCanvas = true) : 
            base(texture, rect, depthLayer, name, isDraggable, isScissorCanvas)
        {
            DepthLayer = depthLayer;
            this.texture = texture;
            Rect = rect;
            IsScissorCanvas = isScissorCanvas;
            IsDraggable = isDraggable;
            Name = name;

            WASDButton = AddSelectButton(positionX: 10, positionY: 50, width: 25, height: 25, unselectTexture, selectTexture, false);

            if (!WASDButton.IsSelected)
            {
                WASDButton.OnClick += (s, e) =>
                {
                    InputManager.WASDSettings(s, e);
                    ZQSDButton.IsSelected = false;
                    WASDButton.IsSelected = true;
                };
            }

            ZQSDButton = AddSelectButton(positionX: 10, positionY: 100, width: 25, height: 25, unselectTexture, selectTexture, false);
            if (!ZQSDButton.IsSelected)
            {
                ZQSDButton.OnClick += (s, e) =>
                {
                    InputManager.ZQSDSettings(s, e);
                    WASDButton.IsSelected = false;
                    ZQSDButton.IsSelected = true;
                };
            }

            TransparentCanvasButton = AddSelectButton(positionX: 10, positionY: 150, width: 25, height: 25, unselectTexture, selectTexture);

            TransparentCanvasButton.OnClick += (s, e) =>
            {
                isTransparent = !isTransparent;
            };

            AddButton(0, 0, 30, 17,
                Globals.Content.Load<Texture2D>("UI/canvasButtons/31"),
                Globals.Content.Load<Texture2D>("UI/canvasButtons/34"), 
                (s, e) => UIManager.CallCanvas(s, e, this));

            ZQSDButton.IsSelected = true;
            WASDButton.IsSelected = false;

            AddText(80, 65, "WASD Keyboard", Color.White);
            AddText(80, 115, "ZQSD Keyboard", Color.White);
            AddText(90, 165, "Transparent Canvas", Color.White);

            try
            {
                foreach (var file in Directory.GetFiles(backgroundFolderPath))
                {
                    string assetName =
                        "Backgrounds/old/" + Path.GetFileNameWithoutExtension(file);

                    backgrounds.Add(
                        Globals.Content.Load<Texture2D>(assetName)
                    );
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            CurrentBackground = backgrounds[0];

            AddText(250, 65, "Backgrounds", Color.AntiqueWhite);

            previousBackground = AddButton(310, 50, 30, 30, Color.White, Color.Gray, PreviousBackground, buttonText: "<=");
            nextBackground = AddButton(340, 50, 30, 30, Color.White, Color.Gray, NextBackground, buttonText: "=>");
        }

        private void NextBackground(object sender, EventArgs e)
        {
            if (backgrounds.IndexOf(CurrentBackground) == backgrounds.Count - 2)
            {
                CurrentBackground = backgrounds[0];
                Debug.WriteLine(backgrounds.IndexOf(CurrentBackground));
            }
            else
            {
                CurrentBackground = backgrounds[backgrounds.IndexOf(CurrentBackground) + 1];
                Debug.WriteLine(backgrounds.IndexOf(CurrentBackground));
            }
        }

        private void PreviousBackground(object sender, EventArgs e)
        {
            if (backgrounds.IndexOf(CurrentBackground) == 0)
            {
                CurrentBackground = backgrounds[backgrounds.Count - 1];
            }
            else
            {
                CurrentBackground = backgrounds[backgrounds.IndexOf(CurrentBackground) - 1];
            }
        }

        public override void Update()
        {
            base.Update();
        }

        public override void Draw()
        {
            base.Draw();
        }
    }
}
