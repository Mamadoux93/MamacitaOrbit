using MamacitaOrbit.Main;
using MamacitaOrbit.Managers;
using MamacitaOrbit.UI;
using MamacitaOrbit.UI.Buttons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MamacitaOrbit.Scenes
{
    internal sealed class Menu : Scene
    {

        private Game game;
        private Texture2D background;


        private List<Button> mainButtons;

        private Button playButton;

        private Button quitButton;


        public Menu(Game game)
        {
            this.game = game;
        }

        public override void InitTexture()
        {
            background = Globals.Content.Load<Texture2D>("Backgrounds/new/background200");
        }
        public override void InitUI()
        {
            mainButtons = new List<Button>();

            playButton = UIManager.AddButton((int)Globals.Bounds.X / 2,
                (int)Globals.Bounds.Y / 2 - 100,
                300, 40,
                Color.White, Color.Gray,
                "Play", null,mainButtons);

            playButton.OnClick += (s, e) =>
            {
                foreach (var button in mainButtons)
                {
                    button.IsIntersectable = true;
                }
                SceneManager.AddScene(new DefendBase());
            };

            quitButton = UIManager.AddButton((int)Globals.Bounds.X / 2,
                (int)Globals.Bounds.Y / 2 + 100,
                300, 40,
                Color.Red, Color.DarkRed,
                "Quit", Color.White,mainButtons);

            quitButton.OnClick += (s, e) =>
            {
                game.Exit();
            };

        }

        public override void DrawUI()
        {
            foreach (var button in mainButtons)
            {
                button.Draw();
            }
        }

        public override void Update()
        {
            foreach (var button in mainButtons)
            {
                button.Update();
            }
        }
        public override void Draw()
        {
            Globals.SpriteBatch.Draw(background, new Rectangle(0, 0, (int)Globals.Bounds.X, (int)Globals.Bounds.Y), 
                null, Color.White, 0, Vector2.Zero, 0, 1);
        }
    }
}
