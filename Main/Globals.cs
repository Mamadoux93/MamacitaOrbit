using MamacitaOrbit.Managers;
using MamacitaOrbit.Objects.Sprites;
using MamacitaOrbit.Objects.Sprites.BattleStation;

namespace MamacitaOrbit.Main
{
    internal static class Globals
    {
        public static Player Player { get; set; }
        public static Base Base { get; set; }
        public static float DeltaTime { get; set; }
        public static float GlobalTime { get; set; }
        public static bool ALL_Freeze { get; set; } = false;
        public static SpriteBatch SpriteBatch { get; set; }
        public static ContentManager Content { get; set; }
        public static bool Drag { get; set; }
        public static Vector2 RenderScale { get; set; } = Vector2.One;
        public static Vector2 Offset { get; set; } = Vector2.Zero;
        public static Rectangle RenderDestination { get; set; } = new Rectangle(0, 0, 3840, 2190);
        public static GraphicsDevice GraphicsDevice { get; set; }
        public static Vector2 Bounds { get; set; } = new Vector2(1920, 1080);
        public static Random Random { get; set; } = new Random();
        public static bool IsDebugMode { get; set; } = false;

        public static void Update(GameTime gameTime)
        {
            Drag = InputManager.MouseLeftDown;
            DeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            GlobalTime = (float)gameTime.TotalGameTime.TotalSeconds;
        }

        public static Vector2 GetScaledMousePosition(Vector2 rawMouse)
        {
            return new Vector2(
                (rawMouse.X - RenderDestination.X) / RenderScale.X,
                (rawMouse.Y - RenderDestination.Y) / RenderScale.Y
            );
        }
    }
}