using MamacitaOrbit.Main;
using MamacitaOrbit.Managers;
using MamacitaOrbit.Objects.Interfaces;
using MamacitaOrbit.Objects.Types;
using MamacitaOrbit.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace MamacitaOrbit.Objects.Sprites.InHeritance
{
    internal abstract class DestroyableSprite : AttackingSprite, IDestroyable
    {
        public int Health { get; set; }
        public int RewardCredit { get; set; }
        public int RewardUridium { get; set; }
        public bool IsDead { get; set; } = false;
        public bool IsGettingDestroyed { get; set; } = false;
        public List<Texture2D> ExplosionFrames { get; set; }
        public int MaxHealth { get; set; }

        // For Aliens
        public DestroyableSprite(AlienType type, Rectangle rect) : base(rect)
        {
            Texture = type.Texture;
            origin = new(type.Frames[currentFrame].Width / 2, type.Frames[currentFrame].Height / 2);
            Font.Spacing = 0.5f;
            position = new(rect.Center.X, rect.Center.Y);
            FontPosition = new(rect.X, rect.Y);
            HitboxTexture.SetData(new[] { Color.DarkRed });
            MaxHealth = Health;
        }

        public DestroyableSprite(Rectangle rect) : base(rect)
        {
            this.rect = rect;
            FontPosition = new(rect.X, rect.Y);
            position = new(rect.Center.X - Font.Texture.Width / 2, rect.Center.Y - Font.Texture.Height / 2);
            HitboxTexture.SetData(new[] { Color.CadetBlue });
            MaxHealth = Health;
        }

        public void TakeDamage(int damage)
        {
            Health -= damage;
            if (Health < 0)
            {
                Health = 0;
            }

            var damageText = UIManager.AddText(rect.Right, rect.Top - 10, $"{damage}", Color.Red, 
                UIManager.infoFont, DepthLayer - 0.000001f, UIManager.VolatileTexts);

            Vector2 hitPosition = new(Globals.Random.Next(rect.Left, rect.Right), Globals.Random.Next(rect.Top, rect.Bottom));

            EffectManager.AddSimpleEffect((int)hitPosition.X, (int)hitPosition.Y, EffectManager.HitFrames, 1);
        }

        public void TakeHealth(int healthTaken)
        {
            Health += healthTaken;
            if(Health > MaxHealth)
            {
                Health = MaxHealth;
            }

            var healthText = UIManager.AddText(rect.Right, rect.Top - 10, $"+{healthTaken}", Color.Green,
                UIManager.infoFont, DepthLayer - 0.0000001f, UIManager.VolatileTexts);
        }
        public virtual void Destroy()
        {
            Globals.Player.AddCredits(RewardCredit);
            Globals.Player.Uridium += RewardUridium;
            SoundManager.Play(SoundManager.NormalExplosionSound, SoundManager.GlobalVolume);
            IsGettingDestroyed = true;
        }

        protected virtual void HealthBarDraw(int maxHealth, int blackBarWidth = 100, int barsHeight = 10)
        {
            try
            {
                var healthBarWidth = blackBarWidth * Health / maxHealth;

                Rectangle blackBar = new(rect.Center.X - blackBarWidth / 2, rect.Top - barsHeight - 10, blackBarWidth, barsHeight);

                Rectangle healthBar = new(blackBar.X, rect.Top - barsHeight - 10, healthBarWidth, barsHeight);

                Globals.SpriteBatch.Draw(HitboxTexture, blackBar, null, Color.Black, 0, Vector2.Zero, 0, DepthLayer);
                Globals.SpriteBatch.Draw(HitboxTexture, healthBar, null, Color.Red, 0, Vector2.Zero, 0, DepthLayer - 0.01f);

                UIManager.DrawOutlines(blackBar, Color.Black, 2, DepthLayer);
            }
            catch
            {

            }
        }
    }
}
