using MamacitaOrbit.Main;
using MamacitaOrbit.Objects.Sprites;
using MamacitaOrbit.Objects.Sprites.BattleStation;
using MamacitaOrbit.Objects.Sprites.Projectiles;
using MamacitaOrbit.Objects.Types;
using MamacitaOrbit.UI;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MamacitaOrbit.Managers
{
    internal static class ProjectileManager
    {
        public static Texture2D RedLaserTexture { get; set; }
        public static Texture2D BigRedLaser { get; set; }
        public static Texture2D BigBlueLaser { get; set; }
        public static Texture2D BigWhiteLaser { get; set; }
        public static Texture2D R310Texture { get; set; }
        public static Texture2D PLT2026Texture { get; set; }
        public static Texture2D PLT3030Texture { get; set; }
        public static Texture2D WIZTexture { get; set; }
        public static Texture2D DCR250Texture { get; set; }
        public static List<Projectile> Projectiles { get; } = new();

        public static void Init()
        {

        }

        public static void InitTexture()
        {
            var laserPath = "Projectiles/lasers/";
            var rocketPath = "Projectiles/rockets/";
            RedLaserTexture = Globals.Content.Load<Texture2D>(laserPath + "red_laser");
            BigRedLaser = Globals.Content.Load<Texture2D>(laserPath + "big_red_laser");
            BigBlueLaser = Globals.Content.Load<Texture2D>(laserPath + "big_blue_laser");
            BigWhiteLaser = Globals.Content.Load<Texture2D>(laserPath + "big_white_laser");

            R310Texture = Globals.Content.Load<Texture2D>(rocketPath + "R310");
            PLT2026Texture = Globals.Content.Load<Texture2D>(rocketPath + "plt-2026");
            PLT3030Texture = Globals.Content.Load<Texture2D>(rocketPath + "plt-3030");
            DCR250Texture = Globals.Content.Load<Texture2D>(rocketPath + "dcr-250");
            WIZTexture = Globals.Content.Load<Texture2D>(rocketPath + "wiz-x");
        }

        public static void AddLaser(ProjectileData data)
        {
            Projectiles.Add(new Laser(data));
        }

        public static void AddRocket(ProjectileData data)
        {
            Projectiles.Add(new Rocket(data));
        }

        public static void Update()
        {
            foreach (var projectile in Projectiles)
            {
                projectile.Update();
            }

            Projectiles.RemoveAll(p => 
            { 
                if(p.Lifespan <= 0)
                {
                    p.Destroy();
                    return true;
                }
                else
                {
                    return false;
                }
            });
        }

        public static void Draw()
        {
            foreach (var projectile in Projectiles)
            {
                projectile.Draw();
            }
        }
    }
}
