using MamacitaOrbit.Objects.Sprites;
using MamacitaOrbit.Objects.Sprites.Collectables;
using MamacitaOrbit.UI;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace MamacitaOrbit.Managers
{
    internal static class CollectableManager
    {
        public static List<Texture2D> ClassicCargoFrames { get; set; }
        public static List<Texture2D> BonusBoxFrames { get; set; }
        public static List<Collectable> Collectables { get; set; } = new List<Collectable>();

        private static float minigunBonusTimer = 0;
        private static float doubleCreditBonusTimer = 0;

        private static event EventHandler onDoubleCreditBonusEnd;

        private static bool isDoubleCreditActive;

        private static EventHandler[] powerUps =
        {
            NeutronBomb,
            MinigunPowerUp,
            UridiumPowerUp,
            DoubleCreditPowerUp,
        };


        private static void NeutronBomb(object sender, EventArgs e)
        {
            AlienManager.ExterminateAllAliens(sender, e);
        }

        private static void DoubleCreditBonusEnd(object sender, EventArgs e)
        {
            GlobalsBonuses.CreditBonus /= 2;
        }

        private static void MinigunPowerUp(object sender, EventArgs e)
        {
            minigunBonusTimer = 10;
            Globals.Player.IsMinigunBonusActive = true;
            Globals.Player.ProjectileDamage *= 10;
        }

        private static void UridiumPowerUp(object sender, EventArgs e)
        {
            Globals.Player.Uridium += Globals.Player.Uridium / 10;
        }

        private static void DoubleCreditPowerUp(object sender, EventArgs e)
        {
            doubleCreditBonusTimer = 10;

            if (isDoubleCreditActive)
                return;

            isDoubleCreditActive = true;
            GlobalsBonuses.CreditBonus *= 2;
        }

        public static void Init()
        {
            onDoubleCreditBonusEnd += DoubleCreditBonusEnd;
        }

        public static void InitTexture()
        {
            ClassicCargoFrames = Animation.LoadFrames(Path.Combine
            (AppContext.BaseDirectory, "Content", "Collectables", "classic_cargo"));

            BonusBoxFrames = Animation.LoadFrames(Path.Combine
            (AppContext.BaseDirectory, "Content", "Collectables", "bonus_box"));
        }

        private static float nextDepth = LayerManager.CollectableLayer;
        private const float depthStep = 0.000001f;

        public static void AddCargoBox(Alien alien)
        {
            var collectableWidth = 50;
            var collectableHeight = 50;

            var cargo = new Cargo(
                alien.Endurium,
                alien.Prometium,
                alien.Terbium,
                new Rectangle(
                    alien.rect.Center.X - collectableWidth / 2,
                    alien.rect.Center.Y - collectableHeight / 2,
                    collectableWidth,
                    collectableHeight
                ))
            {
                DepthLayer = nextDepth
            };

            cargo.OnCollection += (s, e) =>
            {
                SoundManager.Play(SoundManager.CollectingSound, 2);
                Globals.Player.Endurium += cargo.Endurium;
                Globals.Player.Prometium += cargo.Prometium;
                Globals.Player.Terbium += cargo.Terbium;
                Globals.Player.CargoBoxCollected++;
            };

            Collectables.Add(cargo);

            nextDepth += depthStep;
        }

        public static void AddPowerUp(int positionX, int positionY)
        {
            var collectableWidth = 50;
            var collectableHeight = 50;

            var powerUp = powerUps[Globals.Random.Next(0, powerUps.Length)];

            var powerUpObject = new PowerUp(new Rectangle(positionX - collectableWidth / 2, positionY - collectableHeight / 2,
                collectableWidth, collectableHeight))
            {
                DepthLayer = nextDepth,
            };

            powerUpObject.OnCollection += (s, e) =>
            {
                powerUp(s, e);

                UIManager.AddText(
                    Globals.Player.rect.Right + 50,
                    Globals.Player.rect.Top - 5,
                    $"{powerUp.Method.Name}",
                    Color.DarkBlue,
                    UIManager.infoFont,
                    Globals.Player.DepthLayer - 0.0000001f,
                    UIManager.VolatileTexts,
                    7);
            };

            Collectables.Add(powerUpObject);

            nextDepth += depthStep;
        }

        public static void Update()
        {
            minigunBonusTimer -= Globals.DeltaTime;
            doubleCreditBonusTimer -= Globals.DeltaTime;
            foreach (var collectable in Collectables)
            {
                collectable.Update();
            }

            Collectables.RemoveAll(c =>
            {
                if (c.Delete)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            });

            if (Collectables.Count == 0)
            {
                nextDepth = LayerManager.CollectableLayer;
            }

            if (minigunBonusTimer <= 0 && Globals.Player.IsMinigunBonusActive)
            {
                Globals.Player.IsMinigunBonusActive = false;
                Globals.Player.ProjectileDamage /= 10;
            }

            if (doubleCreditBonusTimer <= 0 && isDoubleCreditActive)
            {
                isDoubleCreditActive = false;
                onDoubleCreditBonusEnd?.Invoke(null, EventArgs.Empty);
            }
        }

        public static void Draw()
        {
            foreach (var collectable in Collectables)
            {
                collectable.Draw();
            }
        }
    }
}