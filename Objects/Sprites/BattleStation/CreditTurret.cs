using MamacitaOrbit.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MamacitaOrbit.Objects.Sprites.BattleStation
{
    internal sealed class CreditTurret : Turret, IAlly
    {
        public decimal CreditBonus { get; private set; }

        public static Texture2D creditTurretTexture = Globals.Content.Load<Texture2D>("Sprite/BattleStation/modules/module_credit_boost/1");

        public CreditTurret(Rectangle rect) : base(rect)
        {
            this.rect = rect;
            Texture = creditTurretTexture;
            origin = new(Texture.Width / 2, Texture.Height / 2);

            UpgradeTurret();
            GlobalsBonuses.CreditBonus += CreditBonus;
        }

        public CreditTurret()
        {

        }

        public override void UpgradeTurret()
        {
            switch(CurrentLevel)
            {
                case TurretLevel.Level1:
                    CreditBonus = 0.05m;
                    break;
                case TurretLevel.Level2:
                    CreditBonus = 0.20m;
                    break;
                case TurretLevel.Level3:
                    CreditBonus = 0.50m;
                    break;
                case TurretLevel.LevelMax:
                    CreditBonus = 1.0m;
                    break;
            }
        }
    }
}
