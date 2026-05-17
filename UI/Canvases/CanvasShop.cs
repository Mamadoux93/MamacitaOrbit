using MamacitaOrbit.Main;
using MamacitaOrbit.Managers;
using MamacitaOrbit.UI.Buttons;
using System.Collections.Generic;
using System.Linq;
using MamacitaOrbit.UI.Canvases.ShopSectionGroups;
using System.Diagnostics;

namespace MamacitaOrbit.UI.Canvases
{
    internal sealed class CanvasShop : Canvas
    {
        public List<Button> sectionButtons = new();
        private Button quitCanvasButton;
        public List<SectionGroup> SectionGroups { get; set; } = new();

        public float depthSoustractionForTheseFuckingSections;
        public CanvasShop(Texture2D texture, Rectangle rect, float depthLayer, string name, bool isDraggable = true, bool isScissorCanvas = false)
            : base(texture, rect, depthLayer, name, isDraggable, isScissorCanvas)
        {
            depthSoustractionForTheseFuckingSections = depthSoustraction;
            DepthLayer = depthLayer;
            this.texture = texture;
            Rect = rect;
            IsScissorCanvas = isScissorCanvas;
            IsDraggable = isDraggable;
            Name = name;

            BaseSection baseSection = new BaseSection(this);
            BaseUpgradeSection baseUpgradeSection = new BaseUpgradeSection(this, baseSection.buyAttackTurretIcon);
            ShipSection shipSection = new ShipSection(this);
            MiscellaneousSection misSection = new MiscellaneousSection(this);

            baseSection.SectionButton.IsSelected = true;

            quitCanvasButton = AddButton(0, 0, 30, 17,
                Globals.Content.Load<Texture2D>("UI/canvasButtons/31"),
                Globals.Content.Load<Texture2D>("UI/canvasButtons/34"),
                (s, e) => UIManager.CallCanvas(s, e, this));

            foreach(var sectionButton in sectionButtons.OfType<SelectButton>())
            {
                sectionButton.OnClick += (s, e) =>
                {
                    foreach(var sectinhoButton in sectionButtons.OfType<SelectButton>())
                    {
                        sectinhoButton.IsSelected = false;
                    }
                    sectionButton.IsSelected = true;
                };
            }
        }

        public static void OneTimeUpgradeButtonBehavior(bool isUpgraded, uint upgradePrice, Button upgradeButton)
        {
            if (isUpgraded)
            {
                upgradeButton.ButtonText = "Purchased";
                upgradeButton.CurrentColor = Color.Gray;
                upgradeButton.TextColor = Color.Black;
            }
            else if (Globals.Player.Credit < upgradePrice)
            {
                upgradeButton.TextColor = Color.Red;
            }
            else if (Globals.Player.Credit >= upgradePrice)
            {
                upgradeButton.TextColor = Color.Green;
            }
        }

        public override void Update()
        {
            if (isTransparent)
                Transparency = 0;
            else
                Transparency = 1;

            HandleUIItemsPositions();

            quitCanvasButton.Update();

            foreach(var sectionGroup in SectionGroups.Where(sg => sg.SectionButton.IsSelected))
            {
                sectionGroup.Update();
            }

            foreach(var sectionButton in sectionButtons)
            {
                sectionButton.Update();
            }

            if (Rect.X < 0)
                Rect = new(0, Rect.Y, Rect.Width, Rect.Height);

            if (Rect.Y < 0)
                Rect = new(Rect.X, 0, Rect.Width, Rect.Height);

            if (Rect.X > Globals.Bounds.X)
                Rect = new((int)Globals.Bounds.X, Rect.Y, Rect.Width, Rect.Height);

            if (Rect.Y > Globals.Bounds.Y)
                Rect = new(Rect.X, (int)Globals.Bounds.Y, Rect.Width, Rect.Height);

            if (Rect.Contains(InputManager.LogicalMouse.ToPoint()))
                scrollOffsetY -= InputManager.ScrollDelta / 5;
        }

        public override void Draw()
        {
            Globals.SpriteBatch.Draw(texture, Rect, null, Color.White * Transparency, 0, Vector2.Zero, SpriteEffects.None, DepthLayer);

            CanvasTitle.Draw();

            foreach (var sectionButton in sectionButtons.OfType<SelectButton>())
            {
                sectionButton.Draw();
                if(sectionButton.IsSelected)
                {
                    UIManager.DrawOutlines(sectionButton.Rect, Color.Gold, 2, sectionButton.DepthLayer - sectionButton.DepthLayer / 10);
                }
            }

            foreach (var sectionGroup in SectionGroups.Where(sg => sg.SectionButton.IsSelected))
            {
                sectionGroup.Draw();
            }

            quitCanvasButton.Draw();
        }
    }
}
