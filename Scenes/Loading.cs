using MamacitaOrbit.Main;
using MamacitaOrbit.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using MamacitaOrbit.UI;

namespace MamacitaOrbit.Scenes
{
    internal class Loading : Scene
    {
        private Texture2D barRectangle;
        private SpriteFont loadingFont;

        private Queue<Dictionary<Action, string>> loadingLogicQueue = new();

        private Queue<Action> loadingTextureQueue = new();
        private short total;
        private string methodName;
        private Game game;

        private float sceneChangeTimer = 2f;
        private Texture2D background;
        private bool isLoadingFinished;

        private bool shouldExecuteTextureMethod = false;
        private bool isLoadingTaskRunning = false;
        private Rectangle blackBar;
        private Rectangle loadedBar;

        private Texture2D loadingIcon;
        private float loadingIconRotation;

        Vector2 loadingIconOrigin; 

        public Loading(Game game)
        {
            UIManager.InitTexture();
            loadingTextureQueue.Enqueue(SoundManager.InitTexture);
            loadingTextureQueue.Enqueue(ProjectileManager.InitTexture);
            loadingTextureQueue.Enqueue(CollectableManager.InitTexture);
            loadingTextureQueue.Enqueue(MineManager.InitTexture);

            loadingLogicQueue.Enqueue(new() { { EffectManager.Init, "Loading Effects"} });
            loadingLogicQueue.Enqueue(new() { { ProjectileManager.Init, "Loading Projectiles" } });
            loadingLogicQueue.Enqueue(new() { { WaveManager.Init, "Loading Waves" } });
            loadingLogicQueue.Enqueue(new() { { CollectableManager.Init, "Loading Collectables" } });
            loadingLogicQueue.Enqueue(new() { { AlienManager.Init, "Loading Aliens" } });


            total = (short)(loadingLogicQueue.Count + loadingTextureQueue.Count);
            this.game = game;
            loadingIconRotation = 0;
        }


        private void DrawLoadStepFont()
        {
            var offset = 10;
            if (isLoadingFinished)
                return;

            if (String.IsNullOrEmpty(methodName))
            {
                methodName = "Loading...";
            }
            var infoVector = loadingFont.MeasureString(methodName);

            Globals.SpriteBatch.DrawString(
                loadingFont,
                methodName,
                new Vector2(blackBar.Center.X - infoVector.X / 2, blackBar.Top - infoVector.Y - offset),
                Color.White, 0, Vector2.Zero, 1, 0, LayerManager.InfoBarLayer
            );

        }

        private void DrawLoadingBar(int width, int height)
        {
            var loaded = total - loadingLogicQueue.Count;
            var loadedBarWidth = (int)(loaded * width / total);

            blackBar = new(
                (int)Globals.Bounds.X / 2 - width / 2,
                (int)Globals.Bounds.Y - height / 2 - 100,
                width,
                height
            );

            loadedBar = new(
                blackBar.X,
                blackBar.Y,
                loadedBarWidth,
                height
            );

            Globals.SpriteBatch.Draw(barRectangle, blackBar, null, Color.Black, 0, Vector2.Zero, 0, LayerManager.InfoBarLayer);
            Globals.SpriteBatch.Draw(barRectangle, loadedBar, null, Color.Gold, 0, Vector2.Zero, 0, LayerManager.InfoBarLayer);       
            UIManager.DrawOutlines(blackBar, Color.GhostWhite, 5, LayerManager.InfoBarLayer);
        }

        public override void InitTexture()
        {
            barRectangle = new Texture2D(Globals.GraphicsDevice, 1, 1);
            barRectangle.SetData(new[] { Color.White });

            background = Globals.Content.Load<Texture2D>("Backgrounds/new/background200");
            loadingIcon = Globals.Content.Load<Texture2D>("Sprite/Player/UFO/1");

            loadingIconOrigin = new(loadingIcon.Width / 2, loadingIcon.Height / 2);
        }

        private void LoadTexture()
        {
            if (shouldExecuteTextureMethod && loadingTextureQueue.Count > 0)
            {
                shouldExecuteTextureMethod = false;

                var method = loadingTextureQueue.Dequeue();
                method.Invoke();
            }
        }
        private void LoadLogic()
        {
            if (loadingLogicQueue.Count > 0)
            {
                var dict = loadingLogicQueue.Dequeue();
                var method = dict.Keys.First();
                methodName = dict[method];
                method.Invoke();
            }
        }

        public override void Update()
        {

            LoadLogic();

            loadingIconRotation += 0.0001f;


            LoadTexture();


            if (loadingLogicQueue.Count == 0 &&
                loadingTextureQueue.Count == 0 &&
                !isLoadingTaskRunning)
            {
                isLoadingFinished = true;
                sceneChangeTimer -= Globals.DeltaTime;

                if (sceneChangeTimer <= 0)
                    SceneManager.AddScene(new Menu(game));
            }
        }

        public override void DrawUI()
        {
            if (!shouldExecuteTextureMethod && loadingTextureQueue.Count > 0)
                shouldExecuteTextureMethod = true;

            DrawLoadingBar(800, 50);
            DrawLoadStepFont();
            if (!isLoadingFinished)
                LoadingIconRotating(loadingIconRotation);
            if (isLoadingFinished)
            {
                string finishText = "Fully loaded";
                var v = loadingFont.MeasureString(finishText);

                Globals.SpriteBatch.DrawString(
                    loadingFont,
                    finishText,
                    new Vector2(blackBar.Center.X - v.X / 2, blackBar.Center.Y - v.Y / 2),
                    Color.Black
                );
            }
        }

        private void LoadingIconRotating(float rotation)
        {
            Globals.SpriteBatch.Draw(loadingIcon, 
                new Vector2(blackBar.Center.X, blackBar.Center.Y), 
                null, Color.White, rotation, 
                loadingIconOrigin, 0.43f, 0, LayerManager.InfoBarLayer / 10
                );
        }

        public override void Draw()
        {
            Globals.SpriteBatch.Draw(
                background,
                new Rectangle(0, 0, (int)Globals.Bounds.X, (int)Globals.Bounds.Y),
                Color.White
            );
        }

        public override void InitUI()
        {
            loadingFont = Globals.Content.Load<SpriteFont>("Fonts/LoadingFont");
        }
    }
}
