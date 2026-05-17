using MamacitaOrbit.Main;
using MamacitaOrbit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MamacitaOrbit.Managers;

namespace MamacitaOrbit.UI
{
    internal static class IconsTextures
    {
        public static Texture2D R310 { get; } = Globals.Content.Load<Texture2D>("UI/rocketIcons/r-310_100x100");
        public static Texture2D PLT2026 { get; } = Globals.Content.Load<Texture2D>("UI/rocketIcons/plt-2026_100x100");
        public static Texture2D PLT3030 { get; } = Globals.Content.Load<Texture2D>("UI/rocketIcons/plt-3030_100x100");
        public static Texture2D DCR250 { get; } = Globals.Content.Load<Texture2D>("UI/rocketIcons/dcr-250_100x100");
        public static Texture2D WIZ { get; } = Globals.Content.Load<Texture2D>("UI/rocketIcons/wiz-x_100x100");

        public static Texture2D LF1 { get; } = Globals.Content.Load<Texture2D>("UI/laserIcons/lf-1_100x100");
        public static Texture2D LF2 { get; } = Globals.Content.Load<Texture2D>("UI/laserIcons/lf-2_100x100");
        public static Texture2D LF3 { get; } = Globals.Content.Load<Texture2D>("UI/laserIcons/lf-3_100x100");

        public static Texture2D DamageMine { get; } = Globals.Content.Load<Texture2D>("UI/mineIcons/ddm-01_100x100");
        public static Texture2D SlowMine { get; } = Globals.Content.Load<Texture2D>("UI/mineIcons/slm-01_100x100");
        public static Texture2D Firework { get; } = Globals.Content.Load<Texture2D>("UI/mineIcons/fwx-s_100x100");
        public static Texture2D RocketCPU { get; } = Globals.Content.Load<Texture2D>("UI/cpuIcons/arol-x_100x100");
        public static Texture2D NormalGenerator { get; } = Globals.Content.Load<Texture2D>("UI/generatorIcons/g3n-6900_100x100");
        public static Texture2D SuperGenerator { get; } = Globals.Content.Load<Texture2D>("UI/generatorIcons/g3n-7900_100x100");

        public static Dictionary<Texture2D, Texture2D> RocketIconPair { get; set; }= new Dictionary<Texture2D, Texture2D>()
        {
            { ProjectileManager.R310Texture , R310 },
            { ProjectileManager.PLT2026Texture , PLT2026 },
            { ProjectileManager.PLT3030Texture , PLT3030 },
            { ProjectileManager.DCR250Texture , DCR250 },
            { ProjectileManager.WIZTexture , WIZ },
        };

        public static Dictionary<Texture2D, Texture2D> MineIconPair { get; set; } = new Dictionary<Texture2D, Texture2D>()
        {
            { MineManager.DamageMineFrames[0] , DamageMine },
            { MineManager.SlowMineFrames[0] , SlowMine },
        };
    }
}
