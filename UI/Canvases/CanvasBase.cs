using MamacitaOrbit.Main;
using MamacitaOrbit.UI.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MamacitaOrbit.Objects.Sprites.BattleStation;
using MamacitaOrbit.UI.Buttons;

namespace MamacitaOrbit.UI.Canvases
{
    internal sealed class CanvasBase : Canvas
    {
        private Icon baseIcon;
        private Text baseLevelText;
        private Text baseHpText;

        private Icon attackTurretIcon;
        private Button addAttackTurretButton, removeAttackTurretButton;

        private Icon healTurretIcon;
        private Button addHealTurretButton, removeHealTurretButton;

        private Icon creditTurretIcon;
        private Button addCreditTurretButton, removeCreditTurretButton;

        List<Button> addRemoveButtons = new List<Button>();

        private Button quitCanvasButton;
        public CanvasBase(Texture2D texture, Rectangle rect, float depthLayer, string name, bool isDraggable, bool isScissorCanvas = true) : 
            base(texture, rect, depthLayer, name, isDraggable, isScissorCanvas)
        {
            DepthLayer = depthLayer;
            this.texture = texture;
            Rect = rect;
            IsScissorCanvas = isScissorCanvas;
            IsDraggable = isDraggable;
            Name = name;

            baseIcon = AddIcon(0, 30, 200, 200, Globals.Base.BaseTexture);

            quitCanvasButton = AddButton(0, 0, 30, 17,
                Globals.Content.Load<Texture2D>("UI/canvasButtons/31"),
                Globals.Content.Load<Texture2D>("UI/canvasButtons/34"),
                (s, e) => UIManager.CallCanvas(s, e, this));

            baseLevelText = AddText(baseIcon.Rectangle.X - 20, baseIcon.Rectangle.Bottom - 200, $"{Globals.Base.CurrentLevel}", Color.White, UIManager.infoFont);
            baseHpText = AddText(baseIcon.Rectangle.X - 3, baseIcon.Rectangle.Bottom - 150, $"HP:{Globals.Base.MaxHealth}", Color.Green, UIManager.infoFont);

            attackTurretIcon = AddIcon(200, 40, 100, 100, AttackTurret.level1IdleTexture);
            addAttackTurretButton = AddButton(275, 80, 30, 30, Color.LightGreen, Color.DarkGreen, 
                Globals.Base.AddTurret<AttackTurret>, buttonText: "+", list:addRemoveButtons);
            removeAttackTurretButton = AddButton(305, 80, 30, 30, Color.Crimson, Color.DarkRed, 
                Globals.Base.RemoveTurret<AttackTurret>, buttonText: "-", list: addRemoveButtons);

            healTurretIcon = AddIcon(210, 140, 80, 80, HealTurret.healTurretTexture);
            addHealTurretButton = AddButton(275, 160, 30, 30, Color.LightGreen, Color.DarkGreen, 
                Globals.Base.AddTurret<HealTurret>, buttonText: "+", list: addRemoveButtons);
            removeHealTurretButton = AddButton(305, 160, 30, 30, Color.Crimson, Color.DarkRed, 
                Globals.Base.RemoveTurret<HealTurret>, buttonText: "-", list: addRemoveButtons);

            creditTurretIcon = AddIcon(210, 215, 80, 80, CreditTurret.creditTurretTexture);
            addCreditTurretButton = AddButton(275, 240, 30, 30, Color.LightGreen, Color.DarkGreen, 
                Globals.Base.AddTurret<CreditTurret>, buttonText: "+", list: addRemoveButtons);
            removeCreditTurretButton = AddButton(305, 240, 30, 30, Color.Crimson, Color.DarkRed, 
                Globals.Base.RemoveTurret<CreditTurret>, buttonText: "-", list: addRemoveButtons);
        }

        public override void Update()
        {
            if (isTransparent)
                Transparency = 0;
            else
                Transparency = 1;

            HandleUIItemsPositions();

            foreach (var button in CanvasButtons)
            {
                if (!addRemoveButtons.Contains(button))
                {
                    button.Update();
                    button.IsIntersectable = Called;
                }
            }

            if (Rect.X < 0)
                Rect = new(0, Rect.Y, Rect.Width, Rect.Height);

            if (Rect.Y < 0)
                Rect = new(Rect.X, 0, Rect.Width, Rect.Height);

            if (Rect.X > Globals.Bounds.X)
                Rect = new((int)Globals.Bounds.X, Rect.Y, Rect.Width, Rect.Height);

            if (Rect.Y > Globals.Bounds.Y)
                Rect = new(Rect.X, (int)Globals.Bounds.Y, Rect.Width, Rect.Height);


            baseIcon.Texture = Globals.Base.BaseTexture;
            baseLevelText.Message = $"{Globals.Base.CurrentLevel}";
            baseHpText.Message = $"HP:{Globals.Base.MaxHealth}";

            if (Globals.Base.CurrentLevel == BaseLevel.LevelMax)
            {
                baseLevelText.Color = Color.Gold;
            }
            HandleAttackTurretIconTexture();

            HandleTurretButtonBehavior<AttackTurret>(addAttackTurretButton.Update, removeAttackTurretButton.Update);
            HandleTurretButtonBehavior<HealTurret>(addHealTurretButton.Update, removeHealTurretButton.Update);
            HandleTurretButtonBehavior<CreditTurret>(addCreditTurretButton.Update, removeCreditTurretButton.Update);
        }

        private void HandleTurretButtonBehavior<T>(Action addTurretButtonDrawOrUpdate, Action removeTurretButtonDrawOrUpdate) where T : Turret 
        {
            if(Globals.Base.Turrets.Keys.OfType<T>().Any())
            {
                removeTurretButtonDrawOrUpdate();
            }

            if(Base.TurretsInventory.OfType<T>().Any() &&
                Globals.Base.Turrets.Keys.Count != 8)
            {
                addTurretButtonDrawOrUpdate();
            }
        }

        private void HandleAttackTurretIconTexture()
        {
            if (Turret.CurrentLevel == TurretLevel.Level2)
            {
                attackTurretIcon.Texture = AttackTurret.level2IdleTexture;
                return;
            }
            else if (Turret.CurrentLevel == TurretLevel.Level3)
            {
                attackTurretIcon.Texture = AttackTurret.level3IdleTexture;
            }
        }

        public override void Draw()
        {
            Globals.SpriteBatch.Draw(texture, Rect, null, Color.White * Transparency, 0, Vector2.Zero, SpriteEffects.None, DepthLayer);

            foreach(var button in CanvasButtons)
            {
                if(!addRemoveButtons.Contains(button))
                {
                    button.Draw();
                }
            }

            foreach (var text in CanvasTexts)
            {
                text.Draw();
            }

            foreach (var icon in CanvasIcons)
            {
                icon.Draw();
            }

            HandleTurretButtonBehavior<AttackTurret>(addAttackTurretButton.Draw, removeAttackTurretButton.Draw);
            HandleTurretButtonBehavior<HealTurret>(addHealTurretButton.Draw, removeHealTurretButton.Draw);
            HandleTurretButtonBehavior<CreditTurret>(addCreditTurretButton.Draw, removeCreditTurretButton.Draw);
        }
    }
}
