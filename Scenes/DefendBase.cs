using MamacitaOrbit.Managers;
using MamacitaOrbit.Objects.Sprites;
using MamacitaOrbit.Objects.Sprites.BattleStation;
using MamacitaOrbit.UI;
using MamacitaOrbit.UI.Bars;
using MamacitaOrbit.UI.Buttons;
using MamacitaOrbit.UI.Canvases;
using System.Collections.Generic;

namespace MamacitaOrbit.Scenes
{
    internal sealed class DefendBase : Scene
    {
        private CanvasBase battleStationCanvas;
        private CanvasSettings settingsCanvas;
        private CanvasShop shopCanvas;
        private CanvasPlayer playerCanvas;

        private Button battleStationButton;
        private Button shopButton;
        private Button settingsButton;
        private Button playerButton;

        private Button pauseButton;
        private Button nextWaveButton;

        private Button unpauseButton;
        private Button goToMenuButton;

        private float pauseButtonIntersectCooldown = 0.1f;

        private List<Button> pauseButtons;

        private List<Button> buttonsThatOnlyAppearWhenIWantTo;

        private List<Text> deathTexts;
        private Text youAreDeadText;
        private Text numberOfAlienKilled;
        private Text waveReached;

        private Player player;
        private Base baseuh;

        private List<Icon> topButtonsIcons;  

        private float neutronBombFlashTimer = 0.2f;
        private float neutronBombFlashStayTimer = 1.5f;
        private float neutronBombFlashDecayTimer = 1.5f;

        private float neutronBombFlashOpacity = 0;
        public static bool IsNeutronFlashDetonated { get; set; } = false;

        private Texture2D itemBarsButtonTextureNormal;
        private Texture2D itemBarsButtonTextureHovered;

        private PlayerItemsBar playerItemsBar;

        public DefendBase()
        {
            player = new Player(new Rectangle((int)Globals.Bounds.X / 2, (int)Globals.Bounds.Y / 2, 50, 50));
            baseuh = new Base(new Rectangle((int)Globals.Bounds.X / 2, (int)Globals.Bounds.Y / 2, 200, 200));

            Globals.Player = player;
            Globals.Base = baseuh;

            WaveManager.Init();

            Turret.CurrentLevel = TurretLevel.Level1;

            AchievementManager.Init();
        }

        public override void InitTexture()
        {
            itemBarsButtonTextureNormal = Globals.Content.Load<Texture2D>("UI/itemsControl/114");
            itemBarsButtonTextureHovered = Globals.Content.Load<Texture2D>("UI/itemsControl/18");
        }

        public override void InitUI()
        {
            pauseButtons = new List<Button>();
            deathTexts = new List<Text>();
            buttonsThatOnlyAppearWhenIWantTo = new List<Button>();
            topButtonsIcons = new List<Icon>();

            shopCanvas = UIManager.AddCanvas<CanvasShop>(780, 100, 500, 300, LayerManager.ShopCanvasLayer, Color.Black, Color.DarkSlateGray, "Shop");

            playerCanvas = UIManager.AddCanvas<CanvasPlayer>(860, 100, 300, 100, LayerManager.PlayerCanvasLayer, Color.Black, Color.DarkSlateGray, "Player");

            AchievementManager.achievementCanvas = UIManager.AddCanvas<CanvasAchievement>(305, 200, 300, 100, LayerManager.AchievementCanvasLayer,
               Color.Black, Color.DarkSlateGray, "", false);

            foreach (var achievement in AchievementManager.Achievements)
            {
                achievement.OnUnlocked += (s, e) =>
                {
                    AchievementManager.achievementCanvas.ChangeNameDescription(achievement.Name, achievement.Description);
                };
            }

            battleStationButton = UIManager.AddButton(
                Globals.Base.rect.Center.X,
                Globals.Base.rect.Top,
                70, 120,
                Globals.Content.Load<Texture2D>("UI/clickBubbles/162"),
                Globals.Content.Load<Texture2D>("UI/clickBubbles/160"),
                null);

            battleStationCanvas = UIManager.AddCanvas<CanvasBase>(500, 500, 400, 300, LayerManager.BaseCanvasLayer, Color.Black, Color.DarkSlateGray, "BattleStation");

            shopButton = UIManager.AddButton(
                750,
                50,
                50,
                50,
                itemBarsButtonTextureNormal,
                itemBarsButtonTextureHovered);

            settingsButton = UIManager.AddButton(
                700,
                50,
                50,
                50,
                itemBarsButtonTextureNormal,
                itemBarsButtonTextureHovered);

            playerButton = UIManager.AddButton(
                800,
                50,
                50,
                50,
                itemBarsButtonTextureNormal,
                Globals.Content.Load<Texture2D>("UI/itemsControl/18"));

            var settingButtonIcon = UIManager.AddIcon(settingsButton.Rect.Center.X - 15, settingsButton.Rect.Center.Y - 15, 30, 30, 
                Globals.Content.Load<Texture2D>("UI/iconsAndShii/28_repair"), 
                settingsButton.DepthLayer - 0.000001f, topButtonsIcons);

            var shopButtonIcon = UIManager.AddIcon(shopButton.Rect.Center.X - 15, shopButton.Rect.Center.Y - 15, 30, 30, 
                Globals.Content.Load<Texture2D>("UI/iconsAndShii/11_trade"),
                settingsButton.DepthLayer - 0.000001f, topButtonsIcons);

            var playerButtonIcon = UIManager.AddIcon(playerButton.Rect.Center.X - 10, playerButton.Rect.Center.Y - 10, 20, 20,
                Globals.Content.Load<Texture2D>("UI/iconsAndShii/160_clan"),
                settingsButton.DepthLayer - 0.000001f, topButtonsIcons);

            settingsCanvas = UIManager.AddCanvas<CanvasSettings>(730, 30, 500, 200, 
                LayerManager.SettingsCanvasLayer, Color.Black, Color.DarkSlateGray, "Settings");

            pauseButton = UIManager.AddButton(1770,
                40,
                300, 40,
                Color.White, Color.Gray,
                "Pause");

            nextWaveButton = UIManager.AddButton(1470,
                40,
                300, 40,
                Color.White, Color.Gray,
                "Next Wave",
                list:buttonsThatOnlyAppearWhenIWantTo);

            unpauseButton = UIManager.AddButton((int)Globals.Bounds.X / 2, 
                (int)Globals.Bounds.Y / 2 - 150, 
                300, 40, 
                Color.White, Color.Gray,
                "Resume", null, pauseButtons);

            goToMenuButton = UIManager.AddButton((int)Globals.Bounds.X / 2,
                (int)Globals.Bounds.Y / 2 + 150,
                300, 40,
                Color.Red, Color.DarkRed,
                "Go to Menu", null, pauseButtons);

            foreach(var button in pauseButtons)
            {
                button.DepthLayer = LayerManager.PauseLayer / 10;
            }

            youAreDeadText = UIManager.AddText(goToMenuButton.Rect.Center.X, goToMenuButton.Rect.Y - 200, 
                "YOU ARE DEAD LOL X)", Color.White, UIManager.infoFont, goToMenuButton.DepthLayer, deathTexts);

            numberOfAlienKilled = UIManager.AddText(goToMenuButton.Rect.Center.X, goToMenuButton.Rect.Y - 130, 
                $"You killed {Globals.Player.NumAlienDestroyed} aliens", Color.White, UIManager.infoFont, goToMenuButton.DepthLayer, deathTexts);

            waveReached = UIManager.AddText(goToMenuButton.Rect.Center.X, goToMenuButton.Rect.Y - 80,
                $"You reached {WaveManager.activeWave}", Color.White, UIManager.infoFont, goToMenuButton.DepthLayer, deathTexts);


            battleStationButton.OnClick += (s, e) => UIManager.CallCanvas(s, e, battleStationCanvas);
            settingsButton.OnClick += (s, e) => UIManager.CallCanvas(s, e, settingsCanvas);
            playerButton.OnClick += (s, e) => UIManager.CallCanvas(s, e, playerCanvas);
            pauseButton.OnClick += UIManager.CallPauseUI;
            unpauseButton.OnClick += (s, e) => 
            {
                Globals.ALL_Freeze = false;
            };
            shopButton.OnClick += (s, e) => UIManager.CallCanvas(s, e, shopCanvas);
            goToMenuButton.OnClick += (s, e) =>
            {
                Wipe();
            };

            nextWaveButton.OnClick += WaveManager.NextRound;

            nextWaveButton.OnClick += (s, e) =>
            {
                pauseButtonIntersectCooldown = 0.1f;
            };

            playerItemsBar = new();

        }

        private void Wipe()
        {
            Globals.ALL_Freeze = false;
            CollectableManager.Collectables.Clear();
            EffectManager.Effects.Clear();
            AlienManager.Aliens.Clear();
            WaveManager.activeWave = Waves.Wave1;
            ProjectileManager.Projectiles.Clear();
            UIManager.VolatileTexts.Clear();
            MineManager.Mines.Clear();
            SceneCanvas.Clear();
            SceneButtons.Clear();
            Base.TurretsInventory.Clear();
            SceneManager.RemoveScene();
        }

        public override void Update()
        {
            if (pauseButtonIntersectCooldown <= 0)
            {
                foreach (var button in pauseButtons)
                {
                    button.IsIntersectable = false;
                }

                nextWaveButton.IsIntersectable = false;
            }
            else
            {
                foreach (var button in pauseButtons)
                {
                    button.IsIntersectable = true;
                }

                nextWaveButton.IsIntersectable = true;
            }

            if(WaveManager.IsWaveEnded)
            {
                nextWaveButton.Update();
            }


            if (Globals.ALL_Freeze && Globals.Base.IsDead)
            {
                pauseButtonIntersectCooldown = 0.1f;
                goToMenuButton.Update();
                numberOfAlienKilled.Message = $"You killed {Globals.Player.NumAlienDestroyed} aliens";
                if(WaveManager.activeWave < Waves.Wave19)
                    waveReached.Message = $"You reached {WaveManager.activeWave}";
                else
                    waveReached.Message = $"You reached Wave {WaveManager.activeWave}";
            }
            else if (Globals.ALL_Freeze && !Globals.Base.IsDead)
            {
                pauseButtonIntersectCooldown = 0.1f;
                foreach(var button in pauseButtons)
                {
                    button.Update();
                }
            }
            else
            {
                pauseButtonIntersectCooldown -= Globals.DeltaTime;

                foreach (var button in SceneButtons)
                {
                    if (button == battleStationButton && !WaveManager.IsWaveEnded)
                        continue;

                    button.Update();
                }
                foreach (var canvas in SceneCanvas)
                {
                    if (!canvas.Called)
                        continue;

                    canvas.Update();
                    canvas.HandleScroll();
                }
                AlienManager.Update();
                CollectableManager.Update();
                Globals.Player.Update();
                Globals.Base.Update();
                EffectManager.Update();
                WaveManager.Update();
                ProjectileManager.Update();
                MineManager.Update();

                playerItemsBar.Update();

            }

            if (!WaveManager.IsWaveEnded)
            {
                battleStationCanvas.Called = false;
            }

            AchievementManager.Update();
        }

        public override void Draw()
        {
            Globals.SpriteBatch.Draw(settingsCanvas.CurrentBackground, new Rectangle(0, 0, (int)Globals.Bounds.X, (int)Globals.Bounds.Y), null, Color.White, 0, Vector2.Zero, 0, 1);
            if (Globals.ALL_Freeze)
            {
                Globals.SpriteBatch.Draw(GameManager.pauseBackground,
                    new Rectangle(0, 0, (int)Globals.Bounds.X, (int)Globals.Bounds.Y), null,
                    Color.White * 0.5f, 0, Vector2.Zero, 0, LayerManager.PauseLayer);
            }
            Globals.Base.Draw();
            CollectableManager.Draw();
            AlienManager.Draw();
            ProjectileManager.Draw();
            Globals.Player.Draw();
            EffectManager.Draw();
            MineManager.Draw();
        }

        public override void DrawUI()
        {
            UIManager.DrawInfo();
            if (Globals.ALL_Freeze && Globals.Base.IsDead)
            {
                goToMenuButton.Draw();

                foreach (var text in deathTexts)
                {
                    text.Draw();
                }

                Globals.SpriteBatch.Draw(GameManager.pauseBackground,
                    new Rectangle(0, 0, (int)Globals.Bounds.X, (int)Globals.Bounds.Y), null,
                    Color.White * 0.5f, 0, Vector2.Zero, 0, LayerManager.PauseLayer);
            }
            else if(Globals.ALL_Freeze && !Globals.Base.IsDead)
            {
                foreach (var button in pauseButtons)
                {
                    button.Draw();
                }

                Globals.SpriteBatch.Draw(GameManager.pauseBackground,
                    new Rectangle(0, 0, (int)Globals.Bounds.X, (int)Globals.Bounds.Y), null,
                    Color.White * 0.5f, 0, Vector2.Zero, 0, LayerManager.PauseLayer);
            }
            foreach (var button in SceneButtons)
            {
                if (button == battleStationButton && !WaveManager.IsWaveEnded)
                    continue;

                button.Draw();
            }
            foreach (var canvas in SceneCanvas)
            {
                if(!canvas.Called)
                    continue;

                canvas.Draw();
                UIManager.DrawOutlines(canvas.Rect, Color.DarkBlue, 5, canvas.DepthLayer);
            }
            foreach (var icons in topButtonsIcons)
            {
                icons.Draw();
            }
            if (WaveManager.IsWaveEnded)
            {
                nextWaveButton.Draw();
            }

            if (IsNeutronFlashDetonated)
            {
                NeutronBombFlashEffect();

                Globals.SpriteBatch.Draw(GameManager.neutronBombFlash,
                        new Rectangle(0, 0, (int)Globals.Bounds.X, (int)Globals.Bounds.Y), null,
                        Color.White * neutronBombFlashOpacity, 0, Vector2.Zero, 0, LayerManager.PauseLayer);
            }

            playerItemsBar.Draw();

        }

        public void NeutronBombFlashEffect()
        {
            if (neutronBombFlashTimer > 0)
            {
                neutronBombFlashTimer -= Globals.DeltaTime;
                neutronBombFlashOpacity = -5 * neutronBombFlashTimer + 1;
            }
            else if (neutronBombFlashTimer <= 0 && neutronBombFlashStayTimer > 0)
            {
                neutronBombFlashStayTimer -= Globals.DeltaTime;
                neutronBombFlashOpacity = 1;
            }
            else if (neutronBombFlashStayTimer <= 0 && neutronBombFlashDecayTimer > 0)
            {
                neutronBombFlashDecayTimer -= Globals.DeltaTime;
                neutronBombFlashOpacity -= Globals.DeltaTime / 1.2f;
            }
            else if (neutronBombFlashDecayTimer <= 0)
            {
                IsNeutronFlashDetonated = false;
                neutronBombFlashTimer = 0.2f;
                neutronBombFlashStayTimer = 1.2f;
                neutronBombFlashDecayTimer = 1.2f;
                neutronBombFlashOpacity = 0;
            }
        }
    }
}
