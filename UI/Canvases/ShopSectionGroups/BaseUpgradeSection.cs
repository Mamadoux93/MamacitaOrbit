using MamacitaOrbit.Main;
using MamacitaOrbit.Objects.Sprites.BattleStation;
using MamacitaOrbit.UI.Buttons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MamacitaOrbit.UI.Canvases.ShopSectionGroups
{
    internal sealed class BaseUpgradeSection : SectionGroup
    {
        private Icon baseIcon;

        private Icon littleAttackTurretIcon;
        private Icon littleHealTurretIcon;
        private Icon littleCreditTurretIcon;
        private Button upgradeBaseButton;
        private Button upgradeTurretsButton;

        public BaseUpgradeSection(CanvasShop canvasShop, Icon buyAttackTurretIcon) : base(canvasShop)
        {
            var page1 = new Page();

            pages = new[]
            {
                page1,
            };
            currentPage = pages[0];
            baseIcon = canvasShop.AddIcon(10, 100, 50, 50, Globals.Base.BaseTexture, page1.Icons);

            SectionButton = canvasShop.AddSelectButton(80, 40, 80, 53,
            Color.White, Color.Gray,
            null, canvasShop.sectionButtons, false, "Base\nUpgrade");

            littleHealTurretIcon = canvasShop.AddIcon(-13, 200, 60, 60, HealTurret.healTurretTexture, page1.Icons);
            littleCreditTurretIcon = canvasShop.AddIcon(17, 200, 60, 60, CreditTurret.creditTurretTexture, page1.Icons);
            littleAttackTurretIcon = canvasShop.AddIcon(2, 180, 60, 60, taLevelTexturePair[TurretLevel.Level1], page1.Icons);

            littleAttackTurretIcon.DepthLayer -= canvasShop.depthSoustractionForTheseFuckingSections * 4;
            littleHealTurretIcon.DepthLayer -= canvasShop.depthSoustractionForTheseFuckingSections * 3;
            littleCreditTurretIcon.DepthLayer -= canvasShop.depthSoustractionForTheseFuckingSections * 2;

            upgradeBaseButton = canvasShop.AddButton(80, 100, 130, 60, Color.White, Color.Gray, 
                (s, e) =>{ Shop.BuyUpgradeBase(s, e); baseIcon.Texture = Globals.Base.BaseTexture; },
                list: page1.Buttons, buttonText: "Upgrade");


            upgradeTurretsButton = canvasShop.AddButton(80, 190, 130, 60, Color.White, Color.Gray, 
                (s, e) => {
                Shop.BuyUpgradeTurret(s, e);
                if (taLevelTexturePair.TryGetValue(Turret.CurrentLevel, out var newLittleIconTexture))
                {
                    littleAttackTurretIcon.Texture = newLittleIconTexture;
                    buyAttackTurretIcon.Texture = littleAttackTurretIcon.Texture;
                }
            }, list: page1.Buttons, buttonText: "Upgrade");
        }

        public override void Update()
        {
            base.Update();

            var nextBaseLevel = Globals.Base.CurrentLevel + 1;

            if (Globals.Base.CurrentLevel != BaseLevel.LevelMax && 
                Shop.BaseLevelPricePair.TryGetValue(nextBaseLevel, out var upgradeBasePrice))
            {
                if (Globals.Player.Credit < upgradeBasePrice)
                {
                    upgradeBaseButton.TextColor = Color.Red;
                }
                else if(Globals.Player.Credit >= upgradeBasePrice)
                {
                    upgradeBaseButton.TextColor = Color.Green;
                }

                var blabla = "Upgrade";

                if (blabla.Length < upgradeBasePrice.ToString().Length)
                {
                    blabla = blabla.PadLeft(upgradeBasePrice.ToString().Length);
                }
                upgradeBaseButton.ButtonText = $"{blabla}\n{upgradeBasePrice}C";

                Vector2 textSize = Button.buttonFont.MeasureString(upgradeBaseButton.ButtonText);

                if (textSize.X > upgradeBaseButton.Rect.Width)
                    upgradeBaseButton.Rect = new(upgradeBaseButton.Rect.X, upgradeBaseButton.Rect.Y,
                        (int)textSize.X,
                        upgradeBaseButton.Rect.Height);

                if (textSize.Y > upgradeBaseButton.Rect.Height)
                    upgradeBaseButton.Rect = new(upgradeBaseButton.Rect.X, upgradeBaseButton.Rect.Y,
                        (int)textSize.X,
                        upgradeBaseButton.Rect.Height);
            }
            else
            {
                upgradeBaseButton.TextColor = Color.Black;
                upgradeBaseButton.CurrentColor = Color.Gray;
                upgradeBaseButton.ButtonText = "Full";
                upgradeBaseButton.Rect = new(upgradeBaseButton.Rect.X, upgradeBaseButton.Rect.Y,
                        70, 60);
            }

            var nextTurretLevel = Turret.CurrentLevel + 1;

            if (Turret.CurrentLevel != TurretLevel.LevelMax &&
                Shop.TurretLevelPricePair.TryGetValue(nextTurretLevel, out var upgradeTurretsPrice))
            {
                if (Globals.Player.Credit < upgradeTurretsPrice)
                {
                    upgradeTurretsButton.TextColor = Color.Red;
                }
                else if (Globals.Player.Credit >= upgradeTurretsPrice)
                {
                    upgradeTurretsButton.TextColor = Color.Green;
                }

                var blabla = "Upgrade";

                if (blabla.Length < upgradeTurretsPrice.ToString().Length)
                {
                    blabla = blabla.PadLeft(upgradeTurretsPrice.ToString().Length);
                }

                upgradeTurretsButton.ButtonText = $"{blabla}\n{upgradeTurretsPrice}C";

                Vector2 textSize = Button.buttonFont.MeasureString(upgradeTurretsButton.ButtonText);

                if (textSize.X > upgradeTurretsButton.Rect.Width)
                    upgradeTurretsButton.Rect = new(upgradeTurretsButton.Rect.X, upgradeTurretsButton.Rect.Y,
                        (int)textSize.X,
                        upgradeTurretsButton.Rect.Height);

                if (textSize.Y > upgradeTurretsButton.Rect.Height)
                    upgradeTurretsButton.Rect = new(upgradeTurretsButton.Rect.X, upgradeTurretsButton.Rect.Y,
                        (int)textSize.X,
                        upgradeTurretsButton.Rect.Height);
            }
            else
            {
                upgradeTurretsButton.TextColor = Color.Black;
                upgradeTurretsButton.CurrentColor = Color.Gray;
                upgradeTurretsButton.ButtonText = "Full";
                upgradeTurretsButton.Rect = new(upgradeTurretsButton.Rect.X, upgradeTurretsButton.Rect.Y,
                        70, 60);
            }
        }
    }
}
