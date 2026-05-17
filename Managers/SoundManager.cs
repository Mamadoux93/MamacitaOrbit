using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MamacitaOrbit.Main;
using Microsoft.Xna.Framework.Media;

namespace MamacitaOrbit.Managers
{
    internal static class SoundManager
    {
        private static float globalVolume = 1.0f;

        private static readonly List<ManagedSoundEffectInstance> activeInstances = new();
        private static readonly Dictionary<SoundEffect, int> playingCounts = new();
        public static float GlobalVolume
        {
            get
            {
                return globalVolume;
            }
            set
            {
                globalVolume = Math.Clamp(value, 0f, 1f);
                UpdateAllVolumes();
            }
        }
        private class ManagedSoundEffectInstance
        {
            public SoundEffectInstance Instance;
            public SoundEffect Source { get; set; }
            public float TagVolume;
        }

        public static SoundEffectInstance Play(SoundEffect sfx, float volume = 1.0f, bool isLooped = false)
        {
            playingCounts.TryGetValue(sfx, out int count);
            if (count >= 40)
                return null;

            var instance = sfx.CreateInstance();

            var managed = new ManagedSoundEffectInstance
            {
                Instance = instance,
                Source = sfx,
                TagVolume = Math.Clamp(volume, 0f, 1f)
            };

            instance.Volume = managed.TagVolume * globalVolume;
            instance.IsLooped = isLooped;

            activeInstances.Add(managed);
            playingCounts[sfx] = count + 1;

            try
            {
                instance.Play();
            }
            catch (InstancePlayLimitException)
            {
                activeInstances.Remove(managed);
                playingCounts[sfx] = count;
                instance.Dispose();
                return null;
            }

            return instance;
        }
        public static void StopAll()
        {
            foreach(var managed in activeInstances)
                managed.Instance.Stop();

            activeInstances.Clear();
        }
        private static void UpdateAllVolumes()
        {
            foreach(var managed in activeInstances)
                managed.Instance.Volume = managed.TagVolume * globalVolume;
        }

        public static void CleanupStoppedInstances()
        {
            activeInstances.RemoveAll(m =>
            {
                if (m.Instance.State == SoundState.Stopped)
                {
                    if (playingCounts.TryGetValue(m.Source, out int value))
                        playingCounts[m.Source] = --value;
                    return true;
                }
                return false;
            });
        }
        private static readonly string laserSoundPath = "SoundEffects/lasers/";
        private static readonly string npcSoundPath = "SoundEffects/npc_projectiles/";
        private static readonly string explosionSoundPath = "SoundEffects/explosions/";
        private static readonly string collectingSoundPath = "SoundEffects/collecting/";
        private static readonly string rocketSoundPath = "SoundEffects/rockets/";
        private static readonly string cashierSoundPath = "SoundEffects/";
        private static readonly string levelUpSoundPath = "SoundEffects/level_up/";
        private static readonly string uiSoundPath = "SoundEffects/ui/";

        private static SoundEffect redLaserSound1;
        private static SoundEffect redLaserSound2;
        private static SoundEffect redLaserSound3;

        private static SoundEffect blueLaserSound1;
        private static SoundEffect blueLaserSound2;
        private static SoundEffect blueLaserSound3;

        private static SoundEffect whiteLaserSound;

        private static SoundEffect crystalSound1;
        private static SoundEffect crystalSound2;
        private static SoundEffect crystalSound3;
        private static SoundEffect lordakiaSound1;
        private static SoundEffect lordakiaSound2;
        private static SoundEffect lordakiaSound3;
        private static SoundEffect protegitSound1;
        private static SoundEffect protegitSound2;
        private static SoundEffect protegitSound3;

        private static SoundEffect devolariumSound;
        public static SoundEffect NormalExplosionSound { get; set; }
        public static SoundEffect BaseExplosionSound { get; set; }
        public static SoundEffect NormalRocketSound { get; set; }
        public static SoundEffect AdvancedRocketSound { get; set; }
        public static SoundEffect CollectingSound { get; set; }
        private static SoundEffect fireworkExplosionSound1;
        private static SoundEffect fireworkExplosionSound2;
        private static SoundEffect fireworkExplosionSound3;

        // It includes explosions from rockets, mines and others things I forgot :/
        public static SoundEffect DamageFromExplosionSound { get; set; }
        public static SoundEffect[] RedLaserSounds { get; set; }
        public static SoundEffect[] BlueLaserSounds { get; set; }
        public static SoundEffect[] WhiteLaserSound { get; set; }
        public static SoundEffect[] CrystalSounds { get; set; }
        public static SoundEffect[] LordakiaSounds { get; set; }
        public static SoundEffect[] ProtegitSounds { get; set; }

        public static SoundEffect[] FireworkExplosionSounds { get; set; }

        public static SoundEffect[] DevolariumSound { get; set; }

        public static Dictionary<string, SoundEffect[]> Lists { get; private set; } = new();
        public static SoundEffect CashierSound { get; set; }
        public static SoundEffect NeutronBombSound { get; set; }

        public static SoundEffect AchievementUnlockedSound { get; set; }

        public static SoundEffect ButtonClickSound { get; set; }
        public static Song LevelUp { get; set; }

        public static void InitTexture()
        {
            devolariumSound = Globals.Content.Load<SoundEffect>(npcSoundPath + "41_devolarium0");

            redLaserSound1 = Globals.Content.Load<SoundEffect>(laserSoundPath + "36_laser0_0");
            redLaserSound2 = Globals.Content.Load<SoundEffect>(laserSoundPath + "35_laser0_1");
            redLaserSound3 = Globals.Content.Load<SoundEffect>(laserSoundPath + "34_laser0_2");

            blueLaserSound1 = Globals.Content.Load<SoundEffect>(laserSoundPath + "13_laser1_1");
            blueLaserSound2 = Globals.Content.Load<SoundEffect>(laserSoundPath + "14_laser1_2");
            blueLaserSound3 = Globals.Content.Load<SoundEffect>(laserSoundPath + "15_laser1_0");

            whiteLaserSound = Globals.Content.Load<SoundEffect>(laserSoundPath + "11_laser3_0");

            crystalSound1 = Globals.Content.Load<SoundEffect>(npcSoundPath + "30_crystal0");
            crystalSound2 = Globals.Content.Load<SoundEffect>(npcSoundPath + "29_crystal1");
            crystalSound3 = Globals.Content.Load<SoundEffect>(npcSoundPath + "28_crystal2");

            lordakiaSound1 = Globals.Content.Load<SoundEffect>(npcSoundPath + "27_nettel0");
            lordakiaSound2 = Globals.Content.Load<SoundEffect>(npcSoundPath + "26_nettel1");
            lordakiaSound3 = Globals.Content.Load<SoundEffect>(npcSoundPath + "25_nettel2");

            protegitSound1 = Globals.Content.Load<SoundEffect>(npcSoundPath + "24_protegit0");
            protegitSound2 = Globals.Content.Load<SoundEffect>(npcSoundPath + "23_protegit1");
            protegitSound3 = Globals.Content.Load<SoundEffect>(npcSoundPath + "22_protegit2");

            fireworkExplosionSound1 = Globals.Content.Load<SoundEffect>(explosionSoundPath + "5_firework2");
            fireworkExplosionSound2 = Globals.Content.Load<SoundEffect>(explosionSoundPath + "6_firework1");
            fireworkExplosionSound3 = Globals.Content.Load<SoundEffect>(explosionSoundPath + "7_firework0");

            NormalExplosionSound = Globals.Content.Load<SoundEffect>(explosionSoundPath + "3_explosion0_0");
            BaseExplosionSound = Globals.Content.Load<SoundEffect>(explosionSoundPath + "2_explosion2_0");

            DamageFromExplosionSound = Globals.Content.Load<SoundEffect>(explosionSoundPath + "11_explosion1_2");

            CollectingSound = Globals.Content.Load<SoundEffect>(collectingSoundPath + "12_beam0");

            NormalRocketSound = Globals.Content.Load<SoundEffect>(rocketSoundPath + "2_rocket1_0");
            AdvancedRocketSound = Globals.Content.Load<SoundEffect>(rocketSoundPath + "3_pulse");

            CashierSound = Globals.Content.Load<SoundEffect>(cashierSoundPath + "cash-register");

            NeutronBombSound = Globals.Content.Load<SoundEffect>(cashierSoundPath + "neutron-bomb");

            AchievementUnlockedSound = Globals.Content.Load<SoundEffect>(levelUpSoundPath + "15_new_achievement");

            ButtonClickSound = Globals.Content.Load<SoundEffect>(uiSoundPath + "button_click");

            LevelUp = Globals.Content.Load<Song>(levelUpSoundPath + "1_levelUp");


            DevolariumSound = new SoundEffect[1]
            {
                devolariumSound,
            };
            RedLaserSounds = new SoundEffect[3]
            {
                redLaserSound1,
                redLaserSound2,
                redLaserSound3,
            };
            BlueLaserSounds = new SoundEffect[3]
            {
                blueLaserSound1, 
                blueLaserSound2, 
                blueLaserSound3,
            };
            WhiteLaserSound = new SoundEffect[1]
            {
                whiteLaserSound,
            };
            CrystalSounds = new SoundEffect[3]
            {
                crystalSound1,
                crystalSound2,
                crystalSound3,
            };
            LordakiaSounds = new SoundEffect[3]
            {
                lordakiaSound1,
                lordakiaSound2,
                lordakiaSound3,
            };
            ProtegitSounds = new SoundEffect[3]
            {
                protegitSound1,
                protegitSound2,
                protegitSound3,
            };
            FireworkExplosionSounds = new SoundEffect[3]
            {
                fireworkExplosionSound1,
                fireworkExplosionSound2,
                fireworkExplosionSound3,
            };

            Lists = new Dictionary<string, SoundEffect[]>
            {
                { "RedLaserSounds", RedLaserSounds },
                { "BlueLaserSounds", BlueLaserSounds },
                { "WhiteLaserSound", WhiteLaserSound },
                { "CrystalSounds", CrystalSounds },
                { "LordakiaSounds", LordakiaSounds },
                { "ProtegitSounds", ProtegitSounds },
                { "DevolariumSounds", DevolariumSound }
            };
        }
        public static void Update(Game1 game1)
        {
            GlobalVolume = 0.6f;
            CleanupStoppedInstances();
        }
    }
}
