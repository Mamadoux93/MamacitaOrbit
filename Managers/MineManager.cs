using MamacitaOrbit.Objects.Interfaces;
using MamacitaOrbit.Objects.Sprites.Mines;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace MamacitaOrbit.Managers
{
    internal static class MineManager
    {

        public static List<Texture2D> DamageMineFrames { get; set; }
        public static List<Texture2D> FireworkFrames { get; set; }
        public static List<Texture2D> SlowMineFrames { get; set; }
        public static List<Mine> Mines { get; set; } = new();

        private static readonly string damageMineFramesPath = Path.Combine
            (AppContext.BaseDirectory, "Content", "Sprite", "Mines", "damage_mine");

        private static readonly string fireworkFramesPath = Path.Combine
            (AppContext.BaseDirectory, "Content", "Sprite", "Mines", "firework");

        private static readonly string slowMineFramesPath = Path.Combine
            (AppContext.BaseDirectory, "Content", "Sprite", "Mines", "slow_mine");


        private static float nextDepth = LayerManager.EffectLayer;
        private const float depthStep = 0.000001f;

        public const UInt16 damageMineMax = 8;
        public const UInt16 slowMineMax = 12;
        public const UInt16 fireworksMax = 20;

        public static void InitTexture()
        {
            DamageMineFrames = Animation.LoadFrames(damageMineFramesPath);
            FireworkFrames = Animation.LoadFrames(fireworkFramesPath);
            SlowMineFrames = Animation.LoadFrames(slowMineFramesPath);
        }

        public static void AddDamageMine(int positionX, int positionY, List<Texture2D> mineFrames, int damage, int mineRadius, int width = 100, int height = 100)
        {
            Mines.Add(new DamageMine(mineFrames, 
                new(positionX - width / 2, 
                positionY - height / 2,
                width, height))
            {
                DepthLayer = nextDepth,
                MineRadius = mineRadius,
                Damage = damage,
            });
            nextDepth += depthStep;
        }

        public static void AddFirework(int positionX, int positionY, List<Texture2D> mineFrames, List<Texture2D> fireworkFrames, int width = 100, int height = 100)
        {
            Mines.Add(new Firework(mineFrames,
                new(positionX - width / 2,
                positionY - height / 2,
                width, height))
            {
                DepthLayer = nextDepth,
                CurrentFireworkExplosion = fireworkFrames,
            });
            nextDepth += depthStep;
        }

        /*public static void AddMine<T>(int positionX, int positionY, List<Texture2D> mineFrames, int width = 100, int height = 100) where T : Mine
        {
            var zaza = (T)Activator.CreateInstance(typeof(T), mineFrames,
                new Rectangle(positionX - width / 2,
                positionY - height / 2, width, height));

            Mines.Add(zaza);

            zaza.DepthLayer = nextDepth;
            nextDepth += depthStep;
        }*/

        public static void AddSlowMine(int positionX, int positionY, List<Texture2D> mineFrames, int mineRadius, int width = 100, int height = 100)
        {
            Mines.Add(new SlowMine(mineFrames,
                new(positionX - width / 2,
                positionY - height / 2,
                width, height))
            {
                DepthLayer = nextDepth,
                MineRadius = mineRadius,
            });
            nextDepth += depthStep;
        }

        public static void Update()
        {
            foreach (var mine in Mines)
            {
                mine.Update();
            }

            Mines.RemoveAll(m =>
            {
                if (m.Detonated)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            });


            if (Mines.Count == 0)
            {
                nextDepth = LayerManager.MineLayer;
            }
        }

        public static void Draw()
        {
            foreach(var mine in Mines)
            {
                mine.DrawFrames();
            }
        }
    }
}
