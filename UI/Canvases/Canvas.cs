using MamacitaOrbit.Main;
using MamacitaOrbit.Managers;
using MamacitaOrbit.UI.Buttons;
using MamacitaOrbit.UI.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace MamacitaOrbit.UI.Canvases
{
    internal abstract class Canvas : IDraggable, IUIObjects
    {
        protected float scrollOffsetY = 0f;
        protected float maxScrollOffset = 0f;
        protected const float ScrollSpeed = 25f;
        public Texture2D texture;

        // protected List<(Vector2 position, Func<string> getText, Color color)> texts = new();

        public static SpriteFont font = Globals.Content.Load<SpriteFont>("Fonts/canvas_font");
        public Rectangle Rect { get; set; }
        public bool Called { get; set; } = false;
        public string Name { get; set; }

        public Texture2D rectangleTexture = new(Globals.GraphicsDevice, 1, 1);
        public List<Button> CanvasButtons { get; set; } = new();
        protected Point previousPosition;
        public Color OutlineColor { get; set; }
        public bool IsScissorCanvas { get; set; }
        public Color NormalColor { get; set; }
        public Color InvisibleColor { get; set; } = Color.Transparent;
        public float DepthLayer { get; set; }
        protected bool IsDraggable { get; set; }
        protected float nextDepth;
        protected const float depthSoustraction = 0.000001f;
        public float ButtonsIntersctTimer { get; set; } = 0.1f;

        protected static bool isTransparent { get; set; }
        protected static int Transparency;

        protected List<Icon> CanvasIcons { get; set; } = new();
        protected List<Text> CanvasTexts {  get; set; } = new();
        //protected List<Text> CanvasBindingTexts { get; set; } = new();

        protected Text CanvasTitle;
        public Canvas(Texture2D texture, Rectangle rect, float depthLayer, string name = "Untilted Canvas", bool isDraggable = true, bool isScissorCanvas = false)
        {
            DepthLayer = depthLayer;
            nextDepth = DepthLayer;
            this.texture = texture;
            Rect = rect;
            previousPosition = rect.Location;
            IsDraggable = isDraggable;
            Name = name;
            if (IsDraggable == true)
            {
                (this as IDraggable).RegisterDraggable();
            }
            (this as IUIObjects).RegisterUIObject();
            IsScissorCanvas = isScissorCanvas;
            Transparency = 1;

            CanvasTitle = AddText(Rect.Width /2,
                10, Name, Color.Yellow);
        }
        public Button AddButton(int positionX, int positionY, int width, int height, Color normal, Color dessus, EventHandler onClickAction, List<Button> list = null,
            Texture2D buttonTexture = null, string buttonText = null)
        {
            if (!IsScissorCanvas)
            {
                if (positionX < 0) { positionX = 0; }
                if (positionY < 0) { positionY = 0; }
                if (positionX > Rect.Width) { positionX = Rect.Width - width; }
                if (positionY > Rect.Height) { positionY = Rect.Height - height; }
            }
            buttonTexture = new Texture2D(Globals.GraphicsDevice, 1, 1);
            buttonTexture.SetData(new[] { normal });
            Button button = new(buttonTexture, new(Rect.X + positionX, Rect.Y + positionY, width, height), normal, dessus, buttonText)
            {
                DepthLayer = nextDepth - depthSoustraction
            };

            button.OnClick += onClickAction;
            list?.Add(button);

            CanvasButtons.Add(button);
            return button;
        }

        public Button AddButton(int positionX, int positionY, int width, int height, Texture2D normalTexture, Texture2D dessusTexture, EventHandler onClickAction, 
            string buttonText = null)
        {
            if (!IsScissorCanvas)
            {
                if (positionX < 0) { positionX = 0; }
                if (positionY < 0) { positionY = 0; }
                if (positionX > Rect.Width) { positionX = Rect.Width - width; }
                if (positionY > Rect.Height) { positionY = Rect.Height - height; }
            }

            Button button = new(normalTexture, new(Rect.X + positionX, Rect.Y + positionY, width, height), dessusTexture, buttonText)
            {
                DepthLayer = nextDepth - depthSoustraction
            };
            button.OnClick += onClickAction;
            CanvasButtons.Add(button);
            return button;
        }
        public SelectButton AddSelectButton(int positionX, int positionY, int width, int height, Texture2D normalTexture, Texture2D selectedTexture, bool allowUnSelection = true)
        {
            if (!IsScissorCanvas)
            {
                if (positionX < 0) { positionX = 0; }
                if (positionY < 0) { positionY = 0; }
                if (positionX > Rect.Width) { positionX = Rect.Width - width; }
                if (positionY > Rect.Height) { positionY = Rect.Height - height; }
            }

            SelectButton selectButton = new(normalTexture, new(Rect.X + positionX, Rect.Y + positionY, width, height), selectedTexture)
            {
                DepthLayer = nextDepth - depthSoustraction,
                AllowUnSelection = allowUnSelection
            };
            CanvasButtons.Add(selectButton);
            return selectButton;
        }

        public SelectButton AddSelectButton(int positionX, int positionY, int width, int height, Color unselectedColor, Color selectedColor, Texture2D buttonTexture, 
            List<Button> list = null, bool allowUnSelection = true, string buttonText = null)
        {
            if (!IsScissorCanvas)
            {
                if (positionX < 0) { positionX = 0; }
                if (positionY < 0) { positionY = 0; }
                if (positionX > Rect.Width) { positionX = Rect.Width - width; }
                if (positionY > Rect.Height) { positionY = Rect.Height - height; }
            }

            if (buttonTexture == null)
            {
                buttonTexture = new Texture2D(Globals.GraphicsDevice, 1, 1);
                buttonTexture.SetData(new[] { unselectedColor });
            }

            SelectButton selectButton = new(buttonTexture, new(Rect.X + positionX, Rect.Y + positionY, width, height), unselectedColor, selectedColor, buttonText)
            {
                DepthLayer = nextDepth - depthSoustraction,
                AllowUnSelection = allowUnSelection
            };
            list?.Add(selectButton);
            CanvasButtons.Add(selectButton);
            return selectButton;
        }

        /*public Text AddBindingText(int positionX, int positionY, object text, Color fontColor)
        {
            Vector2 textSize = font.MeasureString($"text");
            if (!IsScissorCanvas)
            {
                if (positionX < 0) { positionX = 0; }
                if (positionY < 0) { positionY = 0; }
                if (positionX > Rect.Width) { positionX = (int)(Rect.Width - textSize.X); }
                if (positionY > Rect.Height) { positionY = (int)(Rect.Height - textSize.Y); }
            }
            var bindingTextToAdd = new Text(new(positionX, positionY), $"text", font, fontColor, nextDepth - depthSoustraction * 2);
            bindingTextToAdd.ParentPosition = new Vector2(Rect.X, Rect.Y);
            CanvasBindingTexts.Add(bindingTextToAdd);
            return bindingTextToAdd;
        }*/
        public Text AddText(int positionX, int positionY, string text, Color fontColor, SpriteFont font = null)
        {
            font ??= Canvas.font;

            Vector2 textSize = font.MeasureString(text);

            if (!IsScissorCanvas)
            {
                if (positionX < 0) { positionX = 0; }
                if (positionY < 0) { positionY = 0; }
                if (positionX > Rect.Width) { positionX = (int)(Rect.Width - textSize.X / 2); }
                if (positionY > Rect.Height) { positionY = (int)(Rect.Height - textSize.Y / 2); }
            }

            var textToAdd = new Text(new(positionX - textSize.X / 2, positionY - textSize.Y / 2), text, font, fontColor, nextDepth - depthSoustraction * 2);
            textToAdd.ParentPosition = new Vector2(Rect.X, Rect.Y);
            CanvasTexts.Add(textToAdd);
            return textToAdd;
        }

        public Icon AddIcon(int positionX, int positionY, int width, int height, Texture2D iconTexture, List<Icon> list = null, float iconOpacity = 1f, float iconRotation = 0)
        {
            if (!IsScissorCanvas)
            {
                if (positionX < 0) { positionX = 0; }
                if (positionY < 0) { positionY = 0; }
                if (positionX > Rect.Width) { positionX = Rect.Width - width; }
                if (positionY > Rect.Height) { positionY = Rect.Height - height; }
            }

            var iconToAdd = new Icon(iconTexture, new(Rect.X + positionX, Rect.Y + positionY, width, height),
                nextDepth - depthSoustraction, iconOpacity, iconRotation)
            {
                DepthLayer = nextDepth - depthSoustraction
            };

            list?.Add(iconToAdd);

            CanvasIcons.Add(iconToAdd);
            return iconToAdd;
            /*if (Called)
            {
                Globals.SpriteBatch.Draw(iconTexture, new Rectangle(Rect.X + positionX, Rect.Y + positionY, width, height), null, Color.White, 0, Vector2.Zero, SpriteEffects.None, nextDepth - depthSoustraction);
            }*/
        }
        public virtual void HandleScroll()
        {
            if (!IsScissorCanvas) 
                return;

            if(!Rect.Contains(InputManager.LogicalMouse.ToPoint()))
                return;

            int scrollDelta = InputManager.ScrollDelta;

            if (scrollDelta != 0)
            {
                scrollOffsetY -= scrollDelta * (ScrollSpeed / 120f);
                scrollOffsetY = Math.Clamp(scrollOffsetY, 0, maxScrollOffset);
            }
        }
        protected void HandleUIItemsPositions()
        {
            if (previousPosition == Rect.Location)
                return;
            
            int deltaX = Rect.X - previousPosition.X;
            int deltaY = Rect.Y - previousPosition.Y;

            foreach (var button in CanvasButtons)
            {
                button.Rect = new Rectangle(button.Rect.X + deltaX, button.Rect.Y + deltaY, button.Rect.Width, button.Rect.Height);
            }

            foreach (var text in CanvasTexts)
            {
                text.ParentPosition = new(Rect.X, Rect.Y);
            }
            foreach (var icon in CanvasIcons)
            {
                icon.Rectangle = new Rectangle(icon.Rectangle.X + deltaX, icon.Rectangle.Y + deltaY, 
                    icon.Rectangle.Width, icon.Rectangle.Height);
            }

            previousPosition = Rect.Location;
        }
        protected void UpdateMaxScroll(int totalContentHeight)
        {
            int visibleHeight = Rect.Height - (int)font.MeasureString(Name).Y;
            maxScrollOffset = Math.Max(0, totalContentHeight - visibleHeight);
            scrollOffsetY = Math.Clamp(scrollOffsetY, 0, maxScrollOffset);
        }
        public virtual void Update()
        {
            if (isTransparent)
                Transparency = 0;
            else 
                Transparency = 1;

            HandleUIItemsPositions();

            foreach (var button in CanvasButtons)
            {
                button.Update();
                button.IsIntersectable = Called;
            }

            if (Rect.X < 0)
                Rect = new(0, Rect.Y, Rect.Width, Rect.Height);

            if(Rect.Y < 0)
                Rect = new(Rect.X, 0, Rect.Width, Rect.Height);

            if (Rect.X > Globals.Bounds.X)
                Rect = new((int)Globals.Bounds.X, Rect.Y, Rect.Width, Rect.Height);

            if (Rect.Y > Globals.Bounds.Y)
                Rect = new(Rect.X, (int)Globals.Bounds.Y, Rect.Width, Rect.Height);
        }
        public virtual void Draw()
        {
            Globals.SpriteBatch.Draw(texture, Rect, null, Color.White * Transparency, 0, Vector2.Zero, SpriteEffects.None, DepthLayer);

            foreach (var button in CanvasButtons)
            {
                button.Draw();
            }

            foreach (var text in CanvasTexts)
            {
                text.Draw();
            }

            foreach(var icon in CanvasIcons)
            {
                icon.Draw();
            }
        }
    }
}
