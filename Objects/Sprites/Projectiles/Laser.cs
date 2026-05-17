using MamacitaOrbit.Managers;
using MamacitaOrbit.Objects.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MamacitaOrbit.Objects.Sprites.Projectiles
{
    internal class Laser : Projectile
    {
        public Laser(ProjectileData data) : base(data)
        {
            Speed = data.Speed;
            Rotation = data.Rotation;
            Direction = new((float)Math.Cos(Rotation), (float)Math.Sin(Rotation));
            Lifespan = data.Lifespan;
            Damage = data.Damage;
            ShooterType = data.ShooterType;
        }
    }
}
