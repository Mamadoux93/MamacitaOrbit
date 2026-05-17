using MamacitaOrbit.Main;
using MamacitaOrbit.Objects.Sprites;
using MamacitaOrbit.Objects.Sprites.BattleStation;
using MamacitaOrbit.Objects.Sprites.InHeritance;
using MamacitaOrbit.Scenes;
using MamacitaOrbit.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;

namespace MamacitaOrbit.Managers
{
    internal class GameManager
    {
        private readonly Game game;
        private readonly GraphicsDeviceManager graphics;
        private readonly Texture2D background;
        public static readonly Texture2D pauseBackground =  new Texture2D(Globals.GraphicsDevice, 1, 1);
        public static readonly Texture2D neutronBombFlash = new Texture2D(Globals.GraphicsDevice, 1, 1);
        private RenderTarget2D renderTarget;
        private bool isResizing;
        public GameManager(Game game, GraphicsDeviceManager graphics)
        {
            this.game = game;
            this.graphics = graphics;
            background = game.Content.Load<Texture2D>("Backgrounds/old/19");
            pauseBackground.SetData(new[] { Color.Black });
            neutronBombFlash.SetData(new[] { Color.DodgerBlue });

            renderTarget = new RenderTarget2D(
                Globals.GraphicsDevice,
                (int)Globals.Bounds.X,
                (int)Globals.Bounds.Y);

            SceneManager.Init();

            SceneManager.AddScene(new Loading(this.game));

            CalculateRenderDestination();

            game.Window.ClientSizeChanged += OnClientSizeChanged;
        }

        private void CalculateRenderDestination()
        {
            Point windowSize = new(graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height);

            float scaleX = (float)windowSize.X / Globals.Bounds.X;
            float scaleY = (float)windowSize.Y / Globals.Bounds.Y;
            float scale = Math.Min(scaleX, scaleY);

            Globals.RenderScale = new Vector2(scale, scale);

            Globals.Offset = new Vector2(
                (windowSize.X - Globals.Bounds.X * scale) / 2,
                (windowSize.Y - Globals.Bounds.Y * scale) / 2
            );

            Globals.RenderDestination = new Rectangle(
                (int)Globals.Offset.X,
                (int)Globals.Offset.Y,
                (int)(Globals.Bounds.X * scale),
                (int)(Globals.Bounds.Y * scale)
            );
        }

        private void OnClientSizeChanged(object sender, EventArgs e)
        {
            if (!isResizing && game.Window.ClientBounds.Width > 0 && game.Window.ClientBounds.Height > 0)
            {
                isResizing = true;
                CalculateRenderDestination();
                isResizing = false;
            }
        }

        public void Update(Game1 game)
        {
            if(InputManager.IsKeyPressed(Keys.Escape))
                Globals.ALL_Freeze = !Globals.ALL_Freeze;

            if (game.IsActive)
            {
                InputManager.Update();
                SoundManager.Update(game);
                UIManager.Update();
                SceneManager.GetCurrentScene().Update();
            }
            else if (!game.IsActive)
            {
                Globals.ALL_Freeze = true;
                SoundManager.StopAll();
                if (SceneManager.GetCurrentScene() is Loading)
                {
                    SceneManager.GetCurrentScene().Update();
                }
            }
        }

        public void Draw()
        {
            graphics.GraphicsDevice.SetRenderTarget(renderTarget);
            Globals.SpriteBatch.Begin(blendState: BlendState.NonPremultiplied, sortMode: SpriteSortMode.BackToFront);


            SceneManager.GetCurrentScene().Draw();
            UIManager.Draw();
            Globals.SpriteBatch.End();

            graphics.GraphicsDevice.SetRenderTarget(null);

            Globals.SpriteBatch.Begin();
            Globals.SpriteBatch.Draw(renderTarget, Globals.RenderDestination, Color.White);
            Globals.SpriteBatch.End();
        }
    }
}
