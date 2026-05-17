using MamacitaOrbit.Main;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MamacitaOrbit.Objects.Types
{
    internal record ProjectileData
    {
        public Texture2D Texture { get; set; }
        public Vector2 Position { get; set; }
        public float Rotation { get; set; }
        public float Lifespan { get; set; }
        public float Speed { get; set; }
        public int Damage { get; set; }
        public Type ShooterType { get; set; }
        public float DepthLayer { get; set; }
        public int ExplosionSize { get; set; }
        public RocketType RocketType { get; set; }
    }
}
