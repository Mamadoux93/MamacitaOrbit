using MamacitaOrbit.Main;
using MamacitaOrbit.Managers;
using MamacitaOrbit.Scenes;
using MamacitaOrbit.UI.Buttons;
using MamacitaOrbit.UI.Canvases;
using MamacitaOrbit.UI.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;


namespace MamacitaOrbit.UI
{
    internal static class UIManager
    {
        private static Texture2D rectangleTexture = new(Globals.GraphicsDevice, 1, 1);

        public static SpriteFont infoFont = Globals.Content.Load<SpriteFont>("Fonts/info_font");
        private static List<string> infos;
        static float infosCooldown = 1;
        private static Texture2D outlineTexture;
        private const float depthStep = 0.0001f;

        public static float CanvasButtonsIntersctTimer { get; set; } = 0.1f;
        public static List<Text> VolatileTexts { get; set; } = new();

        public static void InitTexture()
        {
            outlineTexture = new Texture2D(Globals.GraphicsDevice, 1, 1);
            outlineTexture.SetData(new[] { Color.White });
        }

        public static Button AddButton(int positionX, int positionY, int width, int height, Texture2D texture, Texture2D textureDessus, string buttonText = null, Color? textColor = null, List < Button> list = null)
        {
            Button button = new(texture, new(positionX - width / 2, positionY - height, width, height), textureDessus, buttonText)
            {
                DepthLayer = LayerManager.ButtonLayer,
                TextColor = textColor,
            };
            if (list != null)
            {
                list.Add(button);
            }
            else
            {
                SceneManager.GetCurrentScene().SceneButtons.Add(button);
            }
            return button;
        }

        public static Button AddButton(int positionX, int positionY, int width, int height, Color normal, Color dessus, string buttonText = null, Color? textColor = null, List<Button> list = null, Texture2D buttonTexture = null)
        {
            if (buttonTexture == null)
            {
                buttonTexture = new Texture2D(Globals.GraphicsDevice, 1, 1);
                buttonTexture.SetData(new[] { normal });
            }
            Button button = new(buttonTexture, new(positionX - width / 2, positionY - height, width, height), normal, dessus, buttonText)
            {
                DepthLayer = LayerManager.ButtonLayer,
                TextColor = textColor,
            };
            if (list != null)
            {
                list.Add(button);
            }
            else
            {
                SceneManager.GetCurrentScene().SceneButtons.Add(button);
            }

            return button;
        }

        public static T AddCanvas<T>(int positionX, int positionY, int width, int height, float canvasLayer, 
            Color canvasColor, Color outlineColor, string name = "Untilted Canvas", bool isDraggable = true, bool isCisored = false, List<Canvas> list = null) where T : Canvas 
        {
            var texture = new Texture2D(Globals.GraphicsDevice, 1, 1);
            texture.SetData(new[] { canvasColor });
            T canvas = (T)Activator.CreateInstance(typeof(T), texture, new Rectangle(positionX - width, positionY - height, width, height), 
                canvasLayer, name ,isDraggable, isCisored
                );
            canvas.OutlineColor = outlineColor;
            if (list != null)
            {
                list.Add(canvas);
            }
            else
            {
                SceneManager.GetCurrentScene().SceneCanvas.Add(canvas);
            }
            return canvas;
        }

        public static void CallCanvas(object sender, EventArgs e, Canvas canvasToCall)
        {
            if(sender is not Button)
                return;

            canvasToCall.Called = !canvasToCall.Called;
            CanvasButtonsIntersctTimer = 0.1f;
        }

        public static void CallPauseUI(object sender, EventArgs e)
        {
            Globals.ALL_Freeze = !Globals.ALL_Freeze;
        }

        public static bool IsIntersectUI()
        {
            foreach (IUIObjects UIObject in SceneManager.GetCurrentScene().UIObjects)
            {
                if (UIObject is Canvas canvas)
                {
                    if (!canvas.Called)
                        continue;

                    if (canvas.Rect.Contains(InputManager.LogicalMouse.ToPoint()))
                    {
                        return true;
                    }
                }

                if (UIObject is Button button)
                {
                    if (!button.IsIntersectable)
                        continue;
                }

                if (UIObject.Rect.Contains(InputManager.LogicalMouse.ToPoint()))
                {
                    return true;
                }
            }
            return false;
        }

        public static void DrawOutlines(Rectangle rect, Color outlineColor, int thickness, float depthLayer)
        {
            Globals.SpriteBatch.Draw(outlineTexture, new Rectangle(rect.X - thickness, rect.Y - thickness, rect.Width + thickness * 2, thickness), null,
                outlineColor, 0, Vector2.Zero, 0, depthLayer);
            Globals.SpriteBatch.Draw(outlineTexture, new Rectangle(rect.X - thickness, rect.Y + rect.Height, rect.Width + thickness * 2, thickness), null,
                outlineColor, 0, Vector2.Zero, 0, depthLayer);
            Globals.SpriteBatch.Draw(outlineTexture, new Rectangle(rect.X - thickness, rect.Y, thickness, rect.Height), null,
                outlineColor, 0, Vector2.Zero, 0, depthLayer);
            Globals.SpriteBatch.Draw(outlineTexture, new Rectangle(rect.X + rect.Width, rect.Y, thickness, rect.Height), null,
                outlineColor, 0, Vector2.Zero, 0, depthLayer);
        }

        public static void Update()
        {
            DragDropManager.Update();

            if (Globals.ALL_Freeze)
                return;
            CanvasButtonsIntersctTimer -= Globals.DeltaTime;

            foreach (var uiObject in SceneManager.GetCurrentScene().UIObjects)
            {
                if(uiObject is Button button)
                {
                    button.IsIntersectable = false;
                }
            }

            foreach(var damageText in VolatileTexts)
            {
                damageText.DeleteTimer -= Globals.DeltaTime;
                damageText.Position = new Vector2(damageText.Position.X, damageText.Position.Y - 25f * Globals.DeltaTime);

                damageText.Opacity = damageText.DeleteTimer / damageText.BullshitDividerForFuckingVolatileTexts;
            }

            VolatileTexts.RemoveAll(damageText => damageText.DeleteTimer <= 0);

            if (VolatileTexts.Count > 100)
            {
                VolatileTexts.RemoveAt(VolatileTexts.Count - 50);
            }
        }

        public static Text AddText(int positionX, int positionY, string text, Color fontColor, SpriteFont font, float depth, List<Text> textList = null, float DeleteTimer = 3) 
        {
            Vector2 textSize = font.MeasureString(text);

            var textToAdd = new Text(new(positionX - textSize.X / 2, positionY - textSize.Y / 2), text, font, fontColor, depth, DeleteTimer);

            textList.Add(textToAdd);
            return textToAdd;
        }

        public static Icon AddIcon(int positionX, int positionY, int width, int height, Texture2D iconTexture,float depth, List<Icon> iconList)
        {
            var iconToAdd = new Icon(iconTexture, new(positionX, positionY, width, height), depth);
            iconList?.Add(iconToAdd);
            return iconToAdd;
        }

        private static Texture2D CreateCircleTexture(int radius)
        {
            Texture2D texture = new Texture2D(Globals.GraphicsDevice, radius, radius);
            Color[] colorData = new Color[radius * radius];

            float diam = radius / 2f;
            float diamsq = diam * diam;

            for (int x = 0; x < radius; x++)
            {
                for (int y = 0; y < radius; y++)
                {
                    int index = x * radius + y;
                    Vector2 pos = new Vector2(x - diam, y - diam);
                    if (pos.LengthSquared() <= diamsq)
                    {
                        colorData[index] = Color.White;
                    }
                    else
                    {
                        colorData[index] = Color.Transparent;
                    }
                }
            }

            texture.SetData(colorData);
            return texture;
        }
        public static void DrawInfo()
        {
            infosCooldown -= Globals.DeltaTime;

            var fps = 1 / Globals.DeltaTime;

            if (infosCooldown <= 0)
            {
                infos = new()
                {
                    $"{AlienManager.Aliens.Where(a => !a.IsGettingDestroyed).Count()} Aliens",
                    $"{Math.Round(fps)} Fps",
                    $"{Globals.Player.rect.X} {Globals.Player.rect.Y} (X,Y)",
                    WaveManager.activeWave.ToString(),
                };
                infosCooldown = 1;
            }
            var infoBarOpacity = 1f;
            var infoBarColor = Color.Red;
            var infoFontColor = Color.White;

            try
            {
                var info = infos.Aggregate((a, b) => a + " | " + b);


                Vector2 textSize = infoFont.MeasureString(info);

                rectangleTexture.SetData(new[] { infoBarColor });

                Globals.SpriteBatch.Draw(rectangleTexture, new Rectangle(0, 0, (int)textSize.X, (int)textSize.Y), null, infoBarColor * infoBarOpacity, 0, Vector2.Zero, 0, LayerManager.InfoBarLayer);
                Globals.SpriteBatch.DrawString(infoFont, info, new Vector2(0, 0), infoFontColor, 0, Vector2.Zero, 1, 0, LayerManager.InfoBarLayer - depthStep);
            }
            catch
            {
                var info = "Loading infos... :)";

                Vector2 textSize = infoFont.MeasureString(info);

                rectangleTexture.SetData(new[] { infoBarColor });

                Globals.SpriteBatch.Draw(rectangleTexture, new Rectangle(0, 0, (int)textSize.X, (int)textSize.Y), null, infoBarColor * infoBarOpacity, 0, Vector2.Zero, 0, LayerManager.InfoBarLayer);
                Globals.SpriteBatch.DrawString(infoFont, info, new Vector2(0, 0), infoFontColor, 0, Vector2.Zero, 1, 0, LayerManager.InfoBarLayer - depthStep);
            }
        }
        public static void Draw()
        {
            Globals.SpriteBatch.End();
            Globals.SpriteBatch.Begin(SpriteSortMode.BackToFront);
            SceneManager.GetCurrentScene().DrawUI();

            foreach(var damageText in VolatileTexts)
            {
                damageText.Draw();
            }


            Globals.SpriteBatch.End();
            Globals.SpriteBatch.Begin(blendState: BlendState.NonPremultiplied, sortMode: SpriteSortMode.BackToFront);
        }
    }
}
