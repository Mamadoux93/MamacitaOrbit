using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MamacitaOrbit.Objects.Interfaces
{
    internal interface IShooter
    {
        public int ProjectileSpeed { get; set; }
        public int ProjectileLifeSpan { get; set; }
        public int ProjectileDamage { get; set; }

        public void FireLaser();
    }
}
