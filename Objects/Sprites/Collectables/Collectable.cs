using MamacitaOrbit.Main;
using MamacitaOrbit.Managers;
using MamacitaOrbit.Objects.Sprites.InHeritance;
using MamacitaOrbit.Objects.Types;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MamacitaOrbit.Objects.Sprites.Collectables
{
    internal abstract class Collectable : Sprite
    {
        public event EventHandler OnCollection;

        private float flashTimer = 0.3f;
        private float flashCooldown;

        private float flashActivationThreshold;

        private bool isInvisible;

        public Collectable()
        {
            flashActivationThreshold = deleteCooldown / 3;
            flashCooldown = flashTimer;
        }

        public virtual void Collect()
        {
            if (Delete)
                return;
            OnCollection?.Invoke(this, EventArgs.Empty);
            Delete = true;
        }

        public override void Update()
        {
            if(Globals.Player.rect.Intersects(rect))
            {
                Collect();
            }
            base.Update();
            NearDeletionFlashing();
        }

        public override void Draw()
        {
            var hitboxTextureOpacity = 0.5f;

            if(!isInvisible)
                base.DrawFrames();

            if (Globals.IsDebugMode)
            {
                Globals.SpriteBatch.Draw(HitboxTexture, rect, null, Color.White * hitboxTextureOpacity, 0 , Vector2.Zero, 0, DepthLayer);
            }
        }

        private void NearDeletionFlashing()
        {
            if (deleteCooldown > flashActivationThreshold)
                return;

            flashTimer -= Globals.DeltaTime;

            if (flashTimer <= 0)
            {
                isInvisible = !isInvisible;
                flashTimer = flashCooldown;
            }
        }
    }
}
