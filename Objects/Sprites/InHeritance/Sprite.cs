using MamacitaOrbit.Main;
using MamacitaOrbit.Objects.Types;
using MamacitaOrbit.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MamacitaOrbit.Objects.Sprites.InHeritance
{
    internal abstract class Sprite
    {
        public Texture2D Texture { get; set; }
        protected Vector2 origin;
        protected Vector2 FontPosition { get; set; }
        public Vector2 position;
        public Rectangle rect;
        protected Vector2 velocity;
        protected float Rotation { get; set; }
        public float DepthLayer { get; set; }

        public int currentFrame = 0;
        protected float frameTime = 0.05f;
        protected float timer = 0f;
        protected float deleteCooldown = 10;
        public List<Texture2D> Frames { get; set; }
        public bool Delete { get; set; } = false;
        public bool IsRotating { get; set; }
        protected SpriteFont Font { get; } = Globals.Content.Load<SpriteFont>("Fonts/basic_font");
        protected Texture2D HitboxTexture { get; } = new Texture2D(Globals.GraphicsDevice, 1, 1);

        // For Projectiles
        public Sprite(ProjectileData data)
        {
            Texture = data.Texture;
            position = data.Position;
        }
        //For Collectables... Maybe IDK
        public Sprite(Rectangle rect)
        {
            this.rect = rect;
            position = new(rect.Center.X, rect.Center.Y);
            HitboxTexture.SetData(new[] { Color.Blue });
        }

        public Sprite()
        {

        }

        public virtual void Update()
        {
            deleteCooldown -= Globals.DeltaTime;

            CurrentFrameTimer(Frames, currentFrame);

            if (deleteCooldown <= 0)
            {
                Delete = true;
            }
        }

        protected void CurrentFrameTimer(List<Texture2D> frames, int currentFrame)
        {
            timer += Globals.DeltaTime;
            if (timer >= frameTime && frames is not null)
            {
                timer = 0f;
                this.currentFrame = (currentFrame + 1) % frames.Count;
            }
        }

        public virtual void Draw()
        {
            if (rect == Rectangle.Empty)
            {
                Globals.SpriteBatch.Draw(Texture, position, Color.White);
            }
        }

        public virtual void DrawFrames(float textureScale = 0.8f)
        {
            Globals.SpriteBatch.Draw(Frames[currentFrame], position, null, Color.White, 0, origin, textureScale, SpriteEffects.None, DepthLayer);
        }

        public virtual void DrawFrames(float rotation, float textureScale = 0.8f)
        {
            Globals.SpriteBatch.Draw(Frames[currentFrame], position, null, Color.White, rotation, origin, textureScale, SpriteEffects.None, DepthLayer);
        }

        public virtual void DrawAngleCorrelatedFrames(int currentAngle)
        {
            var textureScale = 0.8f;

            Globals.SpriteBatch.Draw(Frames[currentAngle], position, null, Color.White, 0, origin, textureScale, SpriteEffects.None, DepthLayer);
        }

        public virtual void DrawFont(string Name, Color color, int offSet = 0)
        {
            Vector2 textSize = Font.MeasureString(Name);

            FontPosition = new((int)(rect.Center.X - textSize.X / 2),
                rect.Bottom + offSet);

            Globals.SpriteBatch.DrawString(Font, Name, FontPosition, color, 0, Vector2.Zero, 1f, SpriteEffects.None, DepthLayer);
        }
    }
}
