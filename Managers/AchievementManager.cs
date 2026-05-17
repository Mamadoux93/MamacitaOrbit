using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MamacitaOrbit.Achievements;
using MamacitaOrbit.Objects.Sprites;
using MamacitaOrbit.Objects.Sprites.BattleStation;
using MamacitaOrbit.UI;
using MamacitaOrbit.UI.Canvases;

namespace MamacitaOrbit.Managers
{
    internal static class AchievementManager
    {
        private static Achievement<Player> firstBlood;
        private static Achievement<Player> killer;
        private static Achievement<Player> exterminator;
        private static Achievement<Base> firstUpgrade;
        private static Achievement<Base> firstTurret;
        private static Achievement<Base> immovableObject;
        private static Achievement<TurretLevel> unstoppableForce;

        public static List<IAchievement> Achievements { get; set; } = new List<IAchievement>();
        public static Queue<IAchievement> AchievementsQueue { get; set; }

        private static Achievement<Player> millionaire;
        private static Achievement<Player> jeffreyEpstein;
        private static Achievement<Player> billionaire;

        private static Achievement<Player> enduriumSoup;
        private static Achievement<Player> prometiumCocktail;
        private static Achievement<Player> iHateTerbium;

        private static Achievement<Player> demolitionist;
        private static Achievement<Player> fonctionnaireFrançais;
        private static Achievement<Player> pyrotechnist;
        public static CanvasAchievement achievementCanvas { get; set; }

        public static void Init()
        {
            firstBlood = new("First Blood", "Kill your first alien.", p => p.NumAlienDestroyed >= 1);
            killer = new("Killer", "Kill 100 aliens.", p => p.NumAlienDestroyed >= 100);
            exterminator = new("Exterminator", "Eradicate 10000 aliens, well done.", p => p.NumAlienDestroyed >= 10000);

            millionaire = new("Millionaire", "Get your first million credits.", p => p.Credit >= 1000000);
            jeffreyEpstein = new("Jeffrey Epstein", "Get 7.95 millions credits, which is the Little Saint James island's price.", p => p.Credit >= 7950000);
            billionaire = new("Billionaire", "Get your first billion credits.", p => p.Credit >= 1000000000);

            enduriumSoup = new("Endurium Soup", "Collect 10 CargoBoxes.", p => p.CargoBoxCollected >= 10);
            prometiumCocktail = new("Prometium Cocktail", "Collect 100 CargoBoxes.", p => p.CargoBoxCollected >= 100);
            iHateTerbium = new("I Hate Terbium >:(", "Collect 1000 CargoBoxes.", p => p.CargoBoxCollected >= 1000);

            demolitionist = new("Demolitionist", "Buy the damage mine in the shop.", p => p.HasDamageMine);
            fonctionnaireFrançais = new("Fonctionnaire Francais XD", "Buy the slow mine in the shop.", p => p.HasSlowMine);
            pyrotechnist = new("Pyrotechnist", "Buy the fireworks in the shop.", p => p.HasFirework);

            firstUpgrade = new("First Upgrade", "Upgrade your base for the first time.", b => b.CurrentLevel > BaseLevel.Level1);
            firstTurret = new("First Turret", "Install your first turret.", b => b.Turrets.Count > 0);

            immovableObject = new("Immovable Object", "Fully Upgrade your base, an actual citadel.", b => b.CurrentLevel == BaseLevel.LevelMax);
            unstoppableForce = new("Unstoppable Force", "Fully Upgrade your turrets, good luck aliens.", t => t == TurretLevel.LevelMax);
        }

        public static void Update()
        {
            firstBlood.Update(Globals.Player);
            killer.Update(Globals.Player);
            jeffreyEpstein.Update(Globals.Player);
            billionaire.Update(Globals.Player);
            millionaire.Update(Globals.Player);
            exterminator.Update(Globals.Player);
            enduriumSoup.Update(Globals.Player);
            prometiumCocktail.Update(Globals.Player);
            iHateTerbium.Update(Globals.Player);
            demolitionist.Update(Globals.Player);
            fonctionnaireFrançais.Update(Globals.Player);
            pyrotechnist.Update(Globals.Player);
            firstUpgrade.Update(Globals.Base);
            firstTurret.Update(Globals.Base);
            immovableObject.Update(Globals.Base);
            unstoppableForce.Update(Turret.CurrentLevel);
        }
    }
}
