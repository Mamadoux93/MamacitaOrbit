using MamacitaOrbit.Main;
using MamacitaOrbit.Objects.Interfaces;
using MamacitaOrbit.Objects.Sprites;
using MamacitaOrbit.Objects.Sprites.InHeritance;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MamacitaOrbit.Managers
{
    internal static class EffectManager
    {
        public static List<Texture2D> FireworkLargeRedDetonationFrames { get; set; }
        public static List<Texture2D> FireworkLargeBlueDetonationFrames { get; set; }
        public static List<Texture2D> FireworkLargeGreenDetonationFrames { get; set; }

        public static List<List<Texture2D>> FireworkDetonationFramesList { get; set; }

        public static List<Texture2D> HitFrames { get; set; }
        public static List<Texture2D> Slow1Frames { get; set; }
        public static List<Texture2D> Slow2Frames { get; set; }
        public static List<Texture2D> LevelUpFrames { get; set; }

        private static readonly string hitFramesPath = Path.Combine
            (AppContext.BaseDirectory, "Content", "Effects", "projectile_hit", "laser_flash");

        private static readonly string fireworkLargeRedPath = Path.Combine
            (AppContext.BaseDirectory, "Content", "Effects", "fireworks", "firework_large_red");

        private static readonly string fireworkLargeBluePath = Path.Combine
            (AppContext.BaseDirectory, "Content", "Effects", "fireworks", "firework_large_blue");

        private static readonly string fireworkLargeGreenPath = Path.Combine
            (AppContext.BaseDirectory, "Content", "Effects", "fireworks", "firework_large_green");

        private static readonly string slowEffect1Path = Path.Combine
            (AppContext.BaseDirectory, "Content", "Effects", "slow", "slow1");

        private static readonly string slowEffect2Path = Path.Combine
            (AppContext.BaseDirectory, "Content", "Effects", "slow", "slow2");

        private static readonly string levelUpEffectPath = Path.Combine
            (AppContext.BaseDirectory, "Content", "Effects", "level_up");

        public static List<EffecT> Effects { get; set; } = new List<EffecT>();

        private static float nextDepth = LayerManager.EffectLayer;
        private const float depthStep = 0.000001f;

        public static void Init()
        {
            HitFrames = Animation.LoadFrames(hitFramesPath);
            FireworkDetonationFramesList = new();
            FireworkLargeRedDetonationFrames = Animation.LoadFrames(fireworkLargeRedPath);
            FireworkLargeBlueDetonationFrames = Animation.LoadFrames(fireworkLargeBluePath);
            FireworkLargeGreenDetonationFrames = Animation.LoadFrames(fireworkLargeGreenPath);

            FireworkDetonationFramesList.Add(FireworkLargeRedDetonationFrames);
            FireworkDetonationFramesList.Add(FireworkLargeBlueDetonationFrames);
            FireworkDetonationFramesList.Add(FireworkLargeGreenDetonationFrames);
            Slow1Frames = Animation.LoadFrames(slowEffect1Path);
            Slow2Frames = Animation.LoadFrames(slowEffect2Path);
            LevelUpFrames = Animation.LoadFrames(levelUpEffectPath);
        }

        public static void AddExplosion<T>(T destroyable, float drawScale = 1.0f) where T : DestroyableSprite
        { 
            Effects.Add(new(destroyable.ExplosionFrames, new(destroyable.rect.Center.X,
                destroyable.rect.Center.Y))
            {
                DepthLayer = nextDepth,
                DrawScale = drawScale,
            });
            nextDepth += depthStep;
        }
        public static EffecT AddSimpleEffect(int positionX, int positionY, List<Texture2D> effectFrames, float drawScale = 0.8f, bool isNonEndingEffect = false)
        {
            var effect = new EffecT(effectFrames, new(positionX, positionY))
            {
                DepthLayer = nextDepth,
                IsNonEnding = isNonEndingEffect,
                DrawScale = drawScale,
            };

            Effects.Add(effect);
            nextDepth += depthStep;

            return effect;
        }

        public static void Update()
        {
            foreach(var effect in Effects)
            {
                effect.Update();
            }

            Effects.RemoveAll(e =>
            {
                if (e.Delete)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            });


            if (Effects.Count == 0)
            {
                nextDepth = LayerManager.EffectLayer;
            }
        }

        public static void Draw()
        {
            foreach (var effect in Effects)
            {
                effect.DrawFrames(effect.DrawScale);
            }
        }
    }  
}
