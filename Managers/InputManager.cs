using MamacitaOrbit.Main;
using MamacitaOrbit.UI;
using MamacitaOrbit.UI.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MamacitaOrbit.Managers
{
    public enum ControlScheme
    {
        WASD,
        ZQSD
    }
    public static class InputManager
    {
        private static KeyboardState lastkeyboardState;
        private static KeyboardState currentkeyboardState;
        private static MouseState currentmouseState;
        private static MouseState lastmouseState;
        public static Vector2 MousePosition => Mouse.GetState().Position.ToVector2();

        private static int previousScroll;
        public static int ScrollDelta { get; private set; }
        public static bool MouseLeftClicked { get; private set; }
        public static bool MouseRightClicked { get; private set; }
        public static bool MouseLeftReleased { get; private set; }
        public static bool MouseRightReleased { get; private set; }
        public static bool MouseLeftDown { get; private set; }
        public static bool MouseRightDown { get; private set; }
        public static Vector2 LogicalMouse { get; set; } = Globals.GetScaledMousePosition(MousePosition);
        public static bool GoLeft { get; private set; }
        public static bool GoRight { get; private set; }
        public static bool GoUp { get; private set; }
        public static bool GoDown { get; private set; }

        public static void WASDSettings(object sender, EventArgs e)
        {
            CurrentScheme = ControlScheme.WASD;
        }

        public static void ZQSDSettings(object sender, EventArgs e)
        {
            CurrentScheme = ControlScheme.ZQSD;
        }

        public static bool IsKeyPressed(Keys key)
        {
            return currentkeyboardState.IsKeyDown(key) && lastkeyboardState.IsKeyUp(key);
        }

        public static bool IsKeyDown(Keys key)
        {
            return currentkeyboardState.IsKeyDown(key);
        }


        public static ControlScheme CurrentScheme { get; set; } = ControlScheme.ZQSD;

        public static void Update()
        {
            lastkeyboardState = currentkeyboardState;
            currentkeyboardState = Keyboard.GetState();

            lastmouseState = currentmouseState;
            currentmouseState = Mouse.GetState();

            MouseLeftDown = currentmouseState.LeftButton == ButtonState.Pressed;

            MouseRightDown = currentmouseState.RightButton == ButtonState.Pressed;


            MouseLeftClicked =
                currentmouseState.LeftButton == ButtonState.Pressed &&
                lastmouseState.LeftButton == ButtonState.Released;

            MouseRightClicked =
                currentmouseState.RightButton == ButtonState.Pressed &&
                lastmouseState.RightButton == ButtonState.Released;

            MouseLeftReleased =
                currentmouseState.LeftButton == ButtonState.Released &&
                lastmouseState.LeftButton == ButtonState.Pressed;

            MouseRightReleased = currentmouseState.RightButton == ButtonState.Released &&
                lastmouseState.RightButton == ButtonState.Pressed;

            LogicalMouse = (MousePosition - Globals.Offset) / Globals.RenderScale;

            if (IsKeyPressed(Keys.B))
            {
                Globals.IsDebugMode = !Globals.IsDebugMode;
            }

            if (CurrentScheme == ControlScheme.WASD)
            {
                GoLeft = currentkeyboardState.IsKeyDown(Keys.A) || currentkeyboardState.IsKeyDown(Keys.Left);
                GoRight = currentkeyboardState.IsKeyDown(Keys.D) || currentkeyboardState.IsKeyDown(Keys.Right);
                GoUp = currentkeyboardState.IsKeyDown(Keys.W) || currentkeyboardState.IsKeyDown(Keys.Up);
                GoDown = currentkeyboardState.IsKeyDown(Keys.S) || currentkeyboardState.IsKeyDown(Keys.Down);
            }
            else if (CurrentScheme == ControlScheme.ZQSD)
            {
                GoLeft = currentkeyboardState.IsKeyDown(Keys.Q) || currentkeyboardState.IsKeyDown(Keys.Left);
                GoRight = currentkeyboardState.IsKeyDown(Keys.D) || currentkeyboardState.IsKeyDown(Keys.Right);
                GoUp = currentkeyboardState.IsKeyDown(Keys.Z) || currentkeyboardState.IsKeyDown(Keys.Up);
                GoDown = currentkeyboardState.IsKeyDown(Keys.S) || currentkeyboardState.IsKeyDown(Keys.Down);
            }

            ScrollDelta = lastmouseState.ScrollWheelValue - previousScroll;
            previousScroll = lastmouseState.ScrollWheelValue;
        }
    }
}
