using System;
using MamacitaOrbit.UI.Buttons;

namespace MamacitaOrbit.UI.Canvases.ShopSectionGroups
{
    internal sealed class MiscellaneousSection : SectionGroup
    {
        private Button buyFireworkButton;
        private Icon buyFireworkIcon;

        public MiscellaneousSection(CanvasShop canvasShop) : base(canvasShop)
        {
            var page1 = new Page();

            pages = new[]
            {
                page1,
            };
            currentPage = pages[0];
            SectionButton = canvasShop.AddSelectButton(240, 40, 150, 53,
            Color.White, Color.Gray,
            null, canvasShop.sectionButtons, false, "Miscellaneous");

            buyFireworkButton = canvasShop.AddButton((int)upLeftButtonPosition.X, (int)upLeftButtonPosition.Y, 130, 65, Color.White, Color.Gray, Shop.BuyFirework, 
                page1.Buttons, buttonText:$"{Shop.fireworkPrice}C");

            buyFireworkIcon = canvasShop.AddIcon((int)upLeftIconPosition.X, (int)upLeftIconPosition.Y, 60, 60, IconsTextures.Firework, page1.Icons);
        }

        public override void Update()
        {
            base.Update();
            CanvasShop.OneTimeUpgradeButtonBehavior(Globals.Player.HasFirework, Shop.fireworkPrice, buyFireworkButton);
        }

    }
}
