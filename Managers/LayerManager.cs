using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MamacitaOrbit.Managers
{
    internal static class LayerManager
    {
        public static float PlayerLayer { get; } = 0.4f;
        public static float BaseLayer { get; } = 0.6f;
        public static float AlienLayer { get; set; } = 0.55f;
        public static float TurretLayer { get; } = 0.6f;
        public static float EffectLayer { get; set; } = 0.2f;
        public static float CollectableLayer { get; } = 0.45f;
        public static float BaseCanvasLayer { get; } = 0.005f;
        public static float PlayerCanvasLayer { get; } = 0.009f;
        public static float SettingsCanvasLayer { get; } = 0.008f;
        public static float LogsCanvasLayer { get; } = 0.007f;
        public static float ShopCanvasLayer { get; } = 0.006f;
        public static float AchievementCanvasLayer { get; } = 0.003f;
        public static float InfoBarLayer { get; set; } = 0.001f;
        public static float ButtonLayer { get; set; } = 0.12f;
        public static float PauseLayer { get; set; } = 0.00000001f;

        public static float MineLayer { get; set; } = 0.46f;
    }
}
