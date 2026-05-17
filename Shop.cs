using MamacitaOrbit.Main;
using MamacitaOrbit.Managers;
using MamacitaOrbit.Main.enums;
using MamacitaOrbit.Objects.Sprites;
using MamacitaOrbit.Objects.Sprites.BattleStation;
using MamacitaOrbit.Objects.Sprites.InHeritance;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using MamacitaOrbit.UI.Canvases.ShopSectionGroups;
using MamacitaOrbit.UI.Bars;

namespace MamacitaOrbit
{
    internal static class Shop
    {
        public const uint baseLevel2Upgrade = 50000;
        public const uint baseLevel3Upgrade = 2000000;
        public const uint baseLevelMaxUpgrade = 8000000;

        public const uint turretLevel2Upgrade = 1000000;
        public const uint turretLevel3Upgrade = 10000000;
        public const uint turretLevelMaxUpgrade = 100000000;

        public const uint attackTurretPrice = 100000;
        public const uint healTurretPrice = 500000;
        public const uint creditTurretPrice = 500000;

        public const uint lf3AutoUpgradePrice = 100000;
        public const uint rocketAutoUpgradePrice = 500000;

        public const uint lf2UpgradePrice = 100000;
        public const uint lf3UpgradePrice = 1000000;

        public const uint plt2026UpgradePrice = 300000;
        public const uint plt3030UpgradePrice = 3000000;

        public const uint speedUpgradePrice = 500000;

        public const uint dcr250RocketPrice = 500000;
        public const uint wizRocketPrice = 500000;

        public const uint damageMinePrice = 500000;
        public const uint slowMinePrice = 500000;

        public const uint fireworkPrice = 500000;

        public static Dictionary<LaserUpgrade, uint> LaserLevelPricePair { get; set; } = new Dictionary<LaserUpgrade, uint>
        {
            { LaserUpgrade.LF2, lf2UpgradePrice },
            { LaserUpgrade.LF3, lf3UpgradePrice },
        };

        public static Dictionary<BaseLevel, uint> BaseLevelPricePair { get; set; } = new Dictionary<BaseLevel, uint>
        {
            { BaseLevel.Level2, baseLevel2Upgrade },
            { BaseLevel.Level3, baseLevel3Upgrade },
            { BaseLevel.LevelMax, baseLevelMaxUpgrade },
        };

        public static Dictionary<TurretLevel, uint> TurretLevelPricePair { get; set; } = new Dictionary<TurretLevel, uint>
        {
            { TurretLevel.Level2, turretLevel2Upgrade },
            { TurretLevel.Level3, turretLevel3Upgrade },
            { TurretLevel.LevelMax, turretLevelMaxUpgrade },
        };

        public static Dictionary<RocketUpgrade, uint> RocketLevelPricePair { get; set; } = new Dictionary<RocketUpgrade, uint>
        {
            { RocketUpgrade.PLT2026, plt2026UpgradePrice },
            { RocketUpgrade.PLT3030, plt3030UpgradePrice },
        };

        public static void BuyLaserUpgrade(object sender, EventArgs e)
        {
            var nextLaserLevel = Globals.Player.CurrentLaserUpgrade + 1;

            if (LaserLevelPricePair.TryGetValue(nextLaserLevel, out var laserUpgradePrice)&&
                Globals.Player.Credit >= laserUpgradePrice)
            {
                Globals.Player.NextLaserUpgrade();
                debitationAndSound(laserUpgradePrice);
            }
        }

        public static void BuyRocketUpgrade(object sender, EventArgs e)
        {
            var nextRocketLevel = Globals.Player.MaxRocketUpgrade + 1;
            if (RocketLevelPricePair.TryGetValue(nextRocketLevel, out var rocketUpgradePrice)&&
                Globals.Player.Credit >= rocketUpgradePrice)
            {
                Globals.Player.NextRocketUpgrade();
                debitationAndSound(rocketUpgradePrice);
            }
        }

        public static void BuyDCR250(object sender, EventArgs e)
        {
            if(Globals.Player.Credit >= dcr250RocketPrice && !Globals.Player.HasDCR250)
            {
                PlayerItemsBar.EnableRocketButton(PlayerItemsBar.DCR250Button, EquippedRocket.DCR250);
                Globals.Player.HasDCR250 = true;
                debitationAndSound(dcr250RocketPrice);
            }
        }

        public static void BuyWIZ(object sender, EventArgs e)
        {
            if (Globals.Player.Credit >= wizRocketPrice && !Globals.Player.HasWIZ)
            {
                PlayerItemsBar.EnableRocketButton(PlayerItemsBar.WIZButton, EquippedRocket.WIZ);
                Globals.Player.HasWIZ = true;
                debitationAndSound(wizRocketPrice);
            }
        }

        public static void BuyFirework(object sender, EventArgs e)
        {
            if(Globals.Player.Credit >= fireworkPrice && !Globals.Player.HasFirework)
            {
                Globals.Player.HasFirework = true;
                debitationAndSound(fireworkPrice);
            }
        }

        public static void BuySpeedUpgrade(object sender, EventArgs e)
        {
            if(Globals.Player.Credit >= speedUpgradePrice && !Globals.Player.IsSpeedUpgraded)
            {
                Globals.Player.Speed = 10;
                Globals.Player.IsSpeedUpgraded = true;
                debitationAndSound(speedUpgradePrice);
            }
        }

        public static void BuyAutoLaserUpgrade(object sender, EventArgs e)
        {
            if (Globals.Player.Credit >= lf3AutoUpgradePrice && !Globals.Player.IsLaserAuto)
            {
                Globals.Player.IsLaserAuto = true;
                debitationAndSound(lf3AutoUpgradePrice);
            }
        }

        public static void BuyDamageMine(object sender, EventArgs e)
        {
            if(Globals.Player.Credit >= damageMinePrice && !Globals.Player.HasDamageMine)
            {
                PlayerItemsBar.EnableMineButton(PlayerItemsBar.DamageMineButton, EquippedMine.DamageMine);
                Globals.Player.HasDamageMine = true;
                debitationAndSound(damageMinePrice);
            }
        }

        public static void BuySlowMine(object sender, EventArgs e)
        {
            if (Globals.Player.Credit >= slowMinePrice && !Globals.Player.HasSlowMine)
            {
                PlayerItemsBar.EnableMineButton(PlayerItemsBar.SlowMineButton, EquippedMine.SlowMine);
                Globals.Player.HasSlowMine = true;
                debitationAndSound(slowMinePrice);
            }
        }

        public static void BuyTurret<T>(object sender, EventArgs e, uint price) where T : Turret
        {
            if (Globals.Player.Credit >= price &&
                (Base.TurretsInventory.OfType<T>().Count() + Globals.Base.Turrets.Keys.OfType<T>().Count()) < 8)
            {
                debitationAndSound(price);
                Base.TurretsInventory.Add((T)Activator.CreateInstance(typeof(T)));
            }
        }

        public static void BuyUpgradeBase(object sender, EventArgs e)
        {
            foreach (var lpp in BaseLevelPricePair)
            {
                var price = lpp.Value;
                var level = lpp.Key;

                if (Globals.Base.CurrentLevel < level 
                    && Globals.Player.Credit >= price)
                {
                    debitationAndSound(price);
                    Globals.Base.CurrentLevel = level;
                    Globals.Base.UpgradeBase();
                    break;                
                }
            }
        }

        public static void BuyAutoRocketUpgrade(object sender, EventArgs e)
        {
            if (Globals.Player.Credit >= rocketAutoUpgradePrice && !Globals.Player.IsRocketAuto)
            {
                Globals.Player.IsRocketAuto = true;
                debitationAndSound(rocketAutoUpgradePrice);
            }
        }

        public static void BuyUpgradeTurret(object sender, EventArgs e)
        {
            foreach (var lpp in TurretLevelPricePair)
            {
                var price = lpp.Value;
                var level = lpp.Key;

                if (Turret.CurrentLevel < level
                        && Globals.Player.Credit >= price)
                {
                    debitationAndSound(price);
                    Turret.CurrentLevel = level;

                    foreach(var tpp in Globals.Base.Turrets)
                    {
                        var turret = tpp.Key;

                        turret.UpgradeTurret();
                    }

                    break;
                }
            }
        }

        private static void debitationAndSound<T>(T price)
        {
            SoundManager.Play(SoundManager.CashierSound);
            Globals.Player.Credit -= (dynamic)price;
        }
    }
}
