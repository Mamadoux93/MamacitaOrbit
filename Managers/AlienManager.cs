using MamacitaOrbit.Main;
using MamacitaOrbit.Objects.Sprites;
using MamacitaOrbit.Objects.Sprites.InHeritance;
using MamacitaOrbit.Objects.Types;
using MamacitaOrbit.Scenes;
using MamacitaOrbit.UI;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MamacitaOrbit.Managers
{
    internal static class AlienManager
    {
        public static List<Alien> Aliens { get; set; } = new();

        private static float alienSpawnTime;
        private static float alienSpawnCooldown;

        private static Dictionary<string, AlienType> alienTypes = [];

        private static FileSystemWatcher watcher;

        private static readonly string alienJsonFilePath = Path.Combine(AppContext.BaseDirectory, "resources", "alien_types.json");

        public static bool IsNeutronBombDetonated { get; set; } = false;
        public static List<List<Texture2D>> AlienTexturesForWIZ { get; set; } = new();

        private static void LoadAlienTypes()
        {
            alienTypes.Clear();
            try
            {
                using FileStream openStream = File.OpenRead(alienJsonFilePath);
                using var document = JsonDocument.Parse(openStream);

                foreach (var alienElement in document.RootElement.EnumerateObject())
                {
                    string name = alienElement.Name;
                    var obj = alienElement.Value;
                    string texture = obj.GetProperty("Texture").GetString();
                    string explosionFrames = obj.GetProperty("ExplosionFrames").GetString();
                    string projectileTexture = obj.GetProperty("ProjectileTexture").GetString();
                    string projectileSound = obj.TryGetProperty("ProjectileSound", out var soundProp) ? soundProp.GetString() : null;

                    int health = obj.TryGetProperty("Health", out var h) ? h.GetInt32() : 100;
                    int rewardCredit = obj.TryGetProperty("RewardCredit", out var rc) ? rc.GetInt32() : 0;
                    int rewardUridium = obj.TryGetProperty("RewardUridium", out var ru) ? ru.GetInt32() : 0;
                    int speed = obj.TryGetProperty("Speed", out var sp) ? sp.GetInt32() : 50;
                    int projectileSpeed = obj.TryGetProperty("ProjectileSpeed", out var ps) ? ps.GetInt32() : 200;
                    int projectileLifeSpan = obj.TryGetProperty("ProjectileLifeSpan", out var pl) ? pl.GetInt32() : 3;
                    int projectileDamage = obj.TryGetProperty("ProjectileDamage", out var pd) ? pd.GetInt32() : 10;
                    int endurium = obj.TryGetProperty("Endurium", out var en) ? en.GetInt32() : 0;
                    int terbium = obj.TryGetProperty("Terbium", out var tb) ? tb.GetInt32() : 0;
                    int prometium = obj.TryGetProperty("Prometium", out var pr) ? pr.GetInt32() : 0;
                    float fireCooldown = obj.TryGetProperty("FireCooldown", out var fc) ? fc.GetSingle() : 0;
                    bool isRotating = obj.TryGetProperty("IsRotating", out var ir) && ir.GetBoolean();

                    List<Texture2D> frames = null;//Animation.LoadFrames(Path.Combine(AppContext.BaseDirectory, texture));
                    List<Texture2D> explosionFramesList = null;//Animation.LoadFrames(Path.Combine(AppContext.BaseDirectory, explosionFrames));
                    Texture2D projectileTex = null;//Globals.Content.Load<Texture2D>(projectileTexture);

                    SoundEffect[] projectileSoundArray = null;

                    if (!string.IsNullOrEmpty(projectileSound) && SoundManager.Lists.TryGetValue(projectileSound, out var sounds))
                    {
                        projectileSoundArray = sounds;
                    }


                    alienTypes[name] = new AlienType(frames, projectileTex, isRotating, health,
                        rewardCredit, rewardUridium, speed,
                        endurium, terbium, prometium,
                        projectileDamage, projectileLifeSpan, projectileSpeed)
                    {
                        Name = name,
                        ExplosionFrames = explosionFramesList,
                        ProjectileSounds = projectileSoundArray,
                        ProjectileTexture = projectileTex,
                        IsRotating = isRotating,
                        Health = health,
                        RewardCredit = rewardCredit,
                        RewardUridium = rewardUridium,
                        AlienSpeed = speed,
                        Endurium = endurium,
                        Terbium = terbium,
                        Prometium = prometium,
                        Frames = frames,
                        ProjectileDamage = projectileDamage,
                        ProjectileLifeSpan = projectileLifeSpan,
                        ProjectileSpeed = projectileSpeed,
                        TexturePathShit = texture,
                        ExplosionFramesPath = explosionFrames,
                        ProjectileTexturePath = projectileTexture,
                    };
                }
            }
            catch
            {

            }
            foreach (var sap in alienTypes)
            {
                var alienType = sap.Value;

                alienType.Frames = Animation.LoadFrames(Path.Combine(AppContext.BaseDirectory, alienType.TexturePathShit));
                alienType.ExplosionFrames = Animation.LoadFrames(Path.Combine(AppContext.BaseDirectory, alienType.ExplosionFramesPath));
                alienType.ProjectileTexture = Globals.Content.Load<Texture2D>(alienType.ProjectileTexturePath);

                AlienTexturesForWIZ.Add(alienType.Frames);
            }
        }
        public static void Init()
        {
            watcher = new FileSystemWatcher(Path.GetDirectoryName(alienJsonFilePath)!,
                Path.GetFileName(alienJsonFilePath));

            watcher.Changed += OnFileChanged;
            watcher.EnableRaisingEvents = true;
            alienSpawnTime = 1f; 
            alienSpawnCooldown = alienSpawnTime;

            LoadAlienTypes();
        }

        private static float nextDepth = LayerManager.AlienLayer;
        private const float depthStep = 0.000001f;

        public static bool AddAlien(AlienType alienType)
        {
            if (alienTypes.Count == 0 || alienType == null)
                return false;

            var spawnPosition = RandomPosition(alienType).ToPoint();

            Aliens.Add(new(alienType, new(spawnPosition.X, spawnPosition.Y,
                alienType.Frames[0].Width - alienType.Frames[0].Width / 10, alienType.Frames[0].Height - alienType.Frames[0].Height / 10))
            {
                DepthLayer = nextDepth
            });

            nextDepth += depthStep;

            return true;
        }

        public static void AddAlienRandomly()
        {
            if (alienTypes.Count == 0)
                return;

            int index = Globals.Random.Next(alienTypes.Count);

            AlienType alienType = alienTypes.Values.ElementAt(index);

            var spawnPosition = RandomPosition(alienType).ToPoint();

            Aliens.Add(new(alienType, new(spawnPosition.X, spawnPosition.Y,
                alienType.Frames[0].Width, alienType.Frames[0].Height))
            {
                DepthLayer = nextDepth
            });

            nextDepth += depthStep;
        }

        private static Vector2 RandomPosition(AlienType alienType)
        {
            if (alienType == null)
                return new Vector2();

            var padding = alienType.Frames[0].Width / 2;

            float w = Globals.Bounds.X;
            float h = Globals.Bounds.Y;

            Vector2 pos = new();

            if (Globals.Random.NextDouble() < w / (w + h))
            {
                pos.X = (int)(Globals.Random.NextDouble() * w);
                pos.Y = (int)(Globals.Random.NextDouble() < 0.5 ? -padding : h + padding);
            }
            else
            {
                pos.X = (int)(Globals.Random.NextDouble() < 0.5 ? -padding : w + padding);
                pos.Y = (int)(Globals.Random.NextDouble() * h);
            }

            return pos;
        }

        public static AlienType GetAlienType(string name)
        {
            if(alienTypes.TryGetValue(name, out var alienType))
            {
                return alienType;
            }

            return null;
        }

        public static void ExterminateAllAliens(object sender, EventArgs e)
        {
            DefendBase.IsNeutronFlashDetonated = true;
            SoundManager.StopAll();
            SoundManager.Play(SoundManager.NeutronBombSound, 2);
            IsNeutronBombDetonated = true;

            foreach(var alien in Aliens)
            {
                Globals.Player.Credit += alien.RewardCredit / 4;
                Globals.Player.Uridium += alien.RewardUridium / 4;

                alien.Destroy();
            }

            IsNeutronBombDetonated = false;

        }
        public static void Update()
        {
            foreach(var alien in Aliens)
            {
                alien.Update();

                if(alien.IsGettingDestroyed)
                {
                    alien.IsDead = true;
                    continue;
                }
            }

            Aliens.RemoveAll(a =>
            {
                if (a.IsDead)
                {
                    Globals.Player.NumAlienDestroyed++;
                    return true;
                }
                else
                {
                    return false;
                }
            });

            if (Aliens.Count == 0)
            {
                nextDepth = LayerManager.AlienLayer;
            }
        }

        private static void OnFileChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Changed)
            {
                return;
            }
            Debug.WriteLine($"File {e.FullPath} changed successfully.");
            LoadAlienTypes();
        }

        public static void Draw()
        {
            foreach(var alien in Aliens)
            {
                alien.Draw();
            }
        }
    }
}
