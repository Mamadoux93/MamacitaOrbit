using MamacitaOrbit.Main;
using MamacitaOrbit.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace MamacitaOrbit.Objects.Sprites.BattleStation
{
    internal sealed class HealTurret : Turret, IAlly
    {
        private float healthTime;
        private float healthCooldown;

        private int healBonus;

        public static Texture2D healTurretTexture = Globals.Content.Load<Texture2D>("Sprite/BattleStation/modules/module_heal/1");

        private readonly List<Texture2D> healEffectFrames = Animation.LoadFrames(Path.Combine(AppContext.BaseDirectory, "Content/Effects/healBase"));

        public HealTurret(Rectangle rect) : base(rect)
        {
            Texture = healTurretTexture;

            origin = new(Texture.Width / 2, Texture.Height / 2);
            UpgradeTurret();
        }

        public HealTurret()
        {

        }
        private void HealBase()
        {
            if (Globals.Base.Health >= Globals.Base.MaxHealth)
                return;

            Globals.Base.TakeHealth(healBonus);

            EffectManager.AddSimpleEffect(rect.Center.X, rect.Center.Y, healEffectFrames, 1.2f);
        }
        public override void UpgradeTurret()
        {
            switch (CurrentLevel)
            {
                case TurretLevel.Level1:
                    healthCooldown = 5;
                    healBonus = 50000;
                    break;

                case TurretLevel.Level2:
                    healthCooldown = 3;
                    healBonus = 100000;
                    break;

                case TurretLevel.Level3:
                    healthCooldown = 1;
                    healBonus = 300000;
                    break;

                case TurretLevel.LevelMax:
                    healthCooldown = 1;
                    healBonus = 300000;
                    break;
            }

            healthTime = healthCooldown;
        }
        public override void Update()
        {
            healthTime -= Globals.DeltaTime;

            if (healthTime <= 0f)
            {
                HealBase();
                healthTime = healthCooldown;
            }

            base.Update();
        }
    }
}
