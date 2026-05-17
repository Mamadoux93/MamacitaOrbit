using MamacitaOrbit.UI.Buttons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MamacitaOrbit.Objects.Sprites;
using System.Diagnostics;

namespace MamacitaOrbit.UI.Canvases.ShopSectionGroups
{
    internal sealed class ShipSection : SectionGroup
    {

        private Texture2D[] laserTextures = new Texture2D[3]
        {
            IconsTextures.LF1,
            IconsTextures.LF2,
            IconsTextures.LF3,
        };

        private Texture2D[] rocketTextures = new Texture2D[3]
        {
            IconsTextures.R310,
            IconsTextures.PLT2026,
            IconsTextures.PLT3030,
        };

        private Texture2D currentLaserTexture;
        private Texture2D currentRocketTexture;

        private Button buyLaserUpgradeButton;
        private Button buyRocketUpgradeButton;
        private Button buyRocketAutoUpgradeButton;
        private Button buyLF3AutoUpgradeButton;
        private Button buyGeneratorUpgradeButton;

        private Button buyDCR250Button;
        private Button buyWIZButton;

        private Button buyDamageMineButton;
        private Button buySlowMineButton;


        private Icon buyDamageMineIcon;
        private Icon buySlowMineIcon;
        private Icon buyDCR250Icon;
        private Icon buyWIZIcon;
        private Icon buyRocketAutoUpgradeIcon;
        private Icon buyLaserIcon;
        private Icon buyRocketIcon;

        private Icon generatorIcon;

        public ShipSection(CanvasShop canvasShop) : base(canvasShop)
        {
            var page1 = new Page();
            var page2 = new Page();
            var page3 = new Page();

            pages = new Page[]
            {
                page1,
                page2,
                page3,
            };

            currentPage = pages[0];

            generatorIcon = canvasShop.AddIcon((int)upLeftIconPosition.X, (int)upLeftIconPosition.Y, 65, 65, IconsTextures.NormalGenerator, page2.Icons);

            buyGeneratorUpgradeButton = canvasShop.AddButton((int)upLeftButtonPosition.X, (int)upLeftButtonPosition.Y, 130, 60, Color.White, Color.Gray, (s, e) =>
            {
                Shop.BuySpeedUpgrade(s, e);
                if (Globals.Player.Credit >= Shop.speedUpgradePrice) 
                {
                    generatorIcon.Texture = IconsTextures.SuperGenerator;
                };
            }, page2.Buttons, buttonText: $"Buy\n{Shop.lf2UpgradePrice}C");

            buyDCR250Button = canvasShop.AddButton((int)downLeftButtonPosition.X, (int)downLeftButtonPosition.Y, 130, 60, Color.White, Color.Gray, Shop.BuyDCR250, page2.Buttons,
                buttonText: $"Buy\n{Shop.dcr250RocketPrice}C");

            buyDCR250Icon = canvasShop.AddIcon((int)downLeftIconPosition.X, (int)downLeftIconPosition.Y, 65, 65, IconsTextures.DCR250, page2.Icons);

            buyWIZButton = canvasShop.AddButton((int)upRightButtonPosition.X, (int)upRightButtonPosition.Y, 130, 60, Color.White, Color.Gray, Shop.BuyWIZ, page2.Buttons,
                buttonText: $"Buy\n{Shop.wizRocketPrice}C");

            buyWIZIcon = canvasShop.AddIcon((int)upRightIconPosition.X, (int)upRightIconPosition.Y, 65, 65, IconsTextures.WIZ, page2.Icons);

            currentLaserTexture = laserTextures[0];
            currentRocketTexture = rocketTextures[0];

            SectionButton = canvasShop.AddSelectButton(160, 40, 80, 53,
            Color.White, Color.Gray, null, canvasShop.sectionButtons, false, "Ship");

            buyLaserIcon = canvasShop.AddIcon((int)upLeftIconPosition.X, (int)upLeftIconPosition.Y, 65, 65, currentLaserTexture, page1.Icons);
            buyLaserUpgradeButton = canvasShop.AddButton((int)upLeftButtonPosition.X, (int)upLeftButtonPosition.Y, 130, 60, Color.White, Color.Gray, (s, e) =>
            {
                if (Shop.LaserLevelPricePair.TryGetValue(Globals.Player.CurrentLaserUpgrade + 1, out var laserUpgradePrice) &&
                    Globals.Player.Credit >= laserUpgradePrice)
                {
                    buyLaserIcon.Texture = FindNextTexture(laserTextures, currentLaserTexture);
                    currentLaserTexture = FindNextTexture(laserTextures, currentLaserTexture); 
                }
                Shop.BuyLaserUpgrade(s, e);
            }, page1.Buttons, buttonText: $"Buy\n{Shop.lf2UpgradePrice}C");

            buyRocketIcon = canvasShop.AddIcon((int)downRightIconPosition.X, (int)downRightIconPosition.Y, 65, 65, currentRocketTexture, page1.Icons);
            buyRocketUpgradeButton = canvasShop.AddButton((int)downRightButtonPosition.X, (int)downRightButtonPosition.Y, 130, 60, Color.White, Color.Gray, (s, e) =>
            {
                if (Shop.RocketLevelPricePair.TryGetValue(Globals.Player.MaxRocketUpgrade + 1, out var rocketUpgradePrice) &&
                    Globals.Player.Credit >= rocketUpgradePrice) 
                {
                    buyRocketIcon.Texture = FindNextTexture(rocketTextures, currentRocketTexture);
                    currentRocketTexture = FindNextTexture(rocketTextures, currentRocketTexture); 
                }
                Shop.BuyRocketUpgrade(s, e);
            }, page1.Buttons, buttonText: $"Buy\n{Shop.plt2026UpgradePrice}C");

            var buyAutoLaserShoot1 = canvasShop.AddIcon(0, 210, 63, 63, IconsTextures.LF3, 
                page1.Icons, iconRotation: MathHelper.ToRadians(355));

            var buyAutoLaserShoot2 = canvasShop.AddIcon(3, 200, 60, 60, IconsTextures.LF3,
                page1.Icons, iconRotation: MathHelper.ToRadians(330));

            var buyAutoLaserShoot3 = canvasShop.AddIcon(0, 193, 55, 55, IconsTextures.LF3, 
                page1.Icons, iconRotation: MathHelper.ToRadians(310));

            var buyAutoLaserShoot4 = canvasShop.AddIcon(-0, 189, 50, 50, IconsTextures.LF3, 
                page1.Icons, iconRotation: MathHelper.ToRadians(290));

            buyAutoLaserShoot1.DepthLayer -= canvasShop.depthSoustractionForTheseFuckingSections * 4;
            buyAutoLaserShoot2.DepthLayer -= canvasShop.depthSoustractionForTheseFuckingSections * 3;
            buyAutoLaserShoot3.DepthLayer -= canvasShop.depthSoustractionForTheseFuckingSections * 2;
            buyAutoLaserShoot4.DepthLayer -= canvasShop.depthSoustractionForTheseFuckingSections * 1;

            buyLF3AutoUpgradeButton = canvasShop.AddButton((int)downLeftButtonPosition.X, (int)downLeftButtonPosition.Y, 130, 60, Color.White, Color.Gray, Shop.BuyAutoLaserUpgrade, page1.Buttons,
                buttonText: $"Buy\n{Shop.lf3AutoUpgradePrice}C");

            buyRocketAutoUpgradeIcon = canvasShop.AddIcon((int)upRightIconPosition.X, (int)upRightIconPosition.Y, 65, 65, IconsTextures.RocketCPU, page1.Icons);

            buyRocketAutoUpgradeButton = canvasShop.AddButton((int)upRightButtonPosition.X, (int)upRightButtonPosition.Y, 130, 60, Color.White, Color.Gray, Shop.BuyAutoRocketUpgrade, page1.Buttons,
                buttonText: $"Buy\n{Shop.rocketAutoUpgradePrice}C");

            buyDamageMineButton = canvasShop.AddButton((int)upLeftButtonPosition.X, (int)upLeftButtonPosition.Y, 130, 60, Color.White, Color.Gray, Shop.BuyDamageMine, 
                page3.Buttons, buttonText: $"{Shop.damageMinePrice}C");

            buyDamageMineIcon = canvasShop.AddIcon((int)upLeftIconPosition.X, (int)upLeftIconPosition.Y, 60, 60, IconsTextures.DamageMine, page3.Icons);

            buySlowMineButton = canvasShop.AddButton((int)upRightButtonPosition.X, (int)upRightButtonPosition.Y, 130, 60, Color.White, Color.Gray, Shop.BuySlowMine, 
                page3.Buttons, buttonText: $"{Shop.slowMinePrice}C");

            buySlowMineIcon = canvasShop.AddIcon((int)upRightIconPosition.X, (int)upRightIconPosition.Y, 60, 60, IconsTextures.SlowMine, page3.Icons);
        }

        private static Texture2D FindNextTexture(Texture2D[] textureArray, Texture2D currentTexture)
        {
            var nextTextureIndex = Array.FindIndex(textureArray, i => i == currentTexture) + 1;

            if (nextTextureIndex > 2)
                return currentTexture;

            currentTexture = textureArray[nextTextureIndex];

            return currentTexture;
        }

        private void HandleBuyLaserButton()
        {
            if (Globals.Player.CurrentLaserUpgrade == LaserUpgrade.LF3)
            {
                buyLaserUpgradeButton.ButtonText = "Full";
                buyLaserUpgradeButton.CurrentColor = Color.Gray;
                buyLaserUpgradeButton.TextColor = Color.Black;
                return;
            }
            if (Shop.LaserLevelPricePair.TryGetValue(Globals.Player.CurrentLaserUpgrade + 1, out var laserUpgradePrice))
            {
                buyLaserUpgradeButton.ButtonText = $"Buy\n{laserUpgradePrice}C";

                if (Globals.Player.Credit < laserUpgradePrice)
                {
                    buyLaserUpgradeButton.TextColor = Color.Red;
                }
                else
                {
                    buyLaserUpgradeButton.TextColor = Color.Green;
                }
            }
        }

        private void HandleBuyRocketButton()
        {
            if (Globals.Player.MaxRocketUpgrade == RocketUpgrade.PLT3030)
            {
                buyRocketUpgradeButton.ButtonText = "Full";
                buyRocketUpgradeButton.CurrentColor = Color.Gray;
                buyRocketUpgradeButton.TextColor = Color.Black;
                return;
            }
            if (Shop.RocketLevelPricePair.TryGetValue(Globals.Player.MaxRocketUpgrade + 1, out var rocketUpgradePrice))
            {
                buyRocketUpgradeButton.ButtonText = $"Buy\n{rocketUpgradePrice}C";

                if (Globals.Player.Credit < rocketUpgradePrice)
                {
                    buyRocketUpgradeButton.TextColor = Color.Red;
                }
                else
                {
                    buyRocketUpgradeButton.TextColor = Color.Green;
                }
            }
        }

        public override void Update()
        {
            base.Update();
            HandleBuyLaserButton();
            HandleBuyRocketButton();

            CanvasShop.OneTimeUpgradeButtonBehavior(Globals.Player.IsLaserAuto, Shop.lf3AutoUpgradePrice, buyLF3AutoUpgradeButton);

            CanvasShop.OneTimeUpgradeButtonBehavior(Globals.Player.IsRocketAuto, Shop.rocketAutoUpgradePrice, buyRocketAutoUpgradeButton);

            CanvasShop.OneTimeUpgradeButtonBehavior(Globals.Player.IsSpeedUpgraded, Shop.speedUpgradePrice, buyGeneratorUpgradeButton);

            CanvasShop.OneTimeUpgradeButtonBehavior(Globals.Player.HasWIZ, Shop.wizRocketPrice, buyWIZButton);

            CanvasShop.OneTimeUpgradeButtonBehavior(Globals.Player.HasDCR250, Shop.dcr250RocketPrice, buyDCR250Button);

            CanvasShop.OneTimeUpgradeButtonBehavior(Globals.Player.HasDamageMine, Shop.damageMinePrice, buyDamageMineButton);

            CanvasShop.OneTimeUpgradeButtonBehavior(Globals.Player.HasSlowMine, Shop.slowMinePrice, buySlowMineButton);
        }
    }
}
