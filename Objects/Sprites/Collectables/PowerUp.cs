using MamacitaOrbit.Managers;
using MamacitaOrbit.UI;

namespace MamacitaOrbit.Objects.Sprites.Collectables
{
    internal class PowerUp : Collectable
    {
        public PowerUp(Rectangle rect) : base()
        {
            this.rect = rect;
            position = new(rect.Center.X, rect.Center.Y);
            HitboxTexture.SetData(new[] { Color.Blue });
            Frames = CollectableManager.BonusBoxFrames;
            origin = new(Frames[currentFrame].Width / 2, Frames[currentFrame].Height / 2);
        }

        public override void Collect()
        {
            base.Collect();
        }
    }
}
