using MamacitaOrbit.Main;
using MamacitaOrbit.Managers;
using MamacitaOrbit.Objects.Sprites.InHeritance;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MamacitaOrbit.Objects.Sprites.BattleStation
{
    internal abstract class Turret : AttackingSprite, IAlly
    {
        public static TurretLevel CurrentLevel { get; set; } = TurretLevel.Level1;

        public Turret(Rectangle rect) : base(rect)
        {
            this.rect = rect;
            position = new(this.rect.Center.X, this.rect.Center.Y);
        }

        public Turret()
        { 

        }

        public abstract void UpgradeTurret();

        public override void Update()
        {
            base.Update();
        }

        public override void Draw()
        {
            var textureScale = 0.8f;

            Globals.SpriteBatch.Draw(Texture, position, null, Color.White, 0f, origin, textureScale, SpriteEffects.None, 0.6f);
            if (Globals.IsDebugMode)
            {
                var hitboxTextureOpacity = 0.5f;
                Globals.SpriteBatch.Draw(rangeRectTexture, rangeRect, null, Color.White * hitboxTextureOpacity);
            }
        }
    }

    public enum TurretLevel
    {
        Level1,
        Level2,
        Level3,
        LevelMax,
    }
}
