using MamacitaOrbit.Main;
using MamacitaOrbit.Objects.Sprites.BattleStation;
using MamacitaOrbit.UI.Buttons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static MamacitaOrbit.UI.Canvases.CanvasShop;

namespace MamacitaOrbit.UI.Canvases.ShopSectionGroups
{
    internal sealed class BaseSection : SectionGroup
    {
        public Button buyAttackTurretButton;
        private Button buyHealTurretButton;
        private Button buyCreditTurretButton;

        public Icon buyAttackTurretIcon;
        private Icon buyHealTurretIcon;
        private Icon buyCreditTurretIcon;

        public BaseSection(CanvasShop canvasShop) : base(canvasShop)
        {
            var page1 = new Page();
            pages = new[]
            {
                page1,
            };
            currentPage = pages[0];

            SectionButton = canvasShop.AddSelectButton(0, 40, 80, 53,
                Color.White, Color.Gray,null, canvasShop.sectionButtons, false, "Base");

            buyAttackTurretButton = canvasShop.AddButton(70, 100, 130, 60, Color.White, Color.Gray, (s, e) => Shop.BuyTurret<AttackTurret>(s, e, Shop.attackTurretPrice), 
                list: page1.Buttons, buttonText: $"Buy\n{Shop.attackTurretPrice}C");
            buyAttackTurretIcon = canvasShop.AddIcon(0, 90, 85, 85, taLevelTexturePair[TurretLevel.Level1], page1.Icons);

            buyHealTurretButton = canvasShop.AddButton(70, 185, 130, 60, Color.White, Color.Gray, (s, e) => Shop.BuyTurret<HealTurret>(s, e, Shop.healTurretPrice), 
                list: page1.Buttons, buttonText: $"Buy\n{Shop.healTurretPrice}C");
            buyHealTurretIcon = canvasShop.AddIcon(0, 170, 85, 85, HealTurret.healTurretTexture, page1.Icons);

            buyCreditTurretButton = canvasShop.AddButton(270, 100, 130, 60, Color.White, Color.Gray, (s, e) => Shop.BuyTurret<CreditTurret>(s, e, Shop.creditTurretPrice), 
                list: page1.Buttons, buttonText: $"Buy\n{Shop.creditTurretPrice}C");
            buyCreditTurretIcon = canvasShop.AddIcon(200, 90, 85, 85, CreditTurret.creditTurretTexture, page1.Icons);
        }

        public override void Update()
        {
            base.Update();
            ManageBuyTurretButtons<AttackTurret>(buyAttackTurretButton, Shop.attackTurretPrice);
            ManageBuyTurretButtons<HealTurret>(buyHealTurretButton, Shop.healTurretPrice);
            ManageBuyTurretButtons<CreditTurret>(buyCreditTurretButton, Shop.creditTurretPrice);
        }

        private void ManageBuyTurretButtons<T>(Button button, uint price) where T : Turret
        {
            if ((Base.TurretsInventory.OfType<T>().Count() + Globals.Base.Turrets.Keys.OfType<T>().Count()) >= 8)
            {
                button.TextColor = Color.Black;
                button.CurrentColor = Color.Gray;
                button.ButtonText = "Full";
            }
            else if (Globals.Player.Credit < price)
            {
                button.TextColor = Color.Red;
            }
            else
            {
                button.ButtonText = $"Buy\n{price}C";
                button.TextColor = Color.Green;
            }
        }
    }
}
