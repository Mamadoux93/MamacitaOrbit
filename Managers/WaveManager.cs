using MamacitaOrbit.Main;
using MamacitaOrbit.Objects.Types;
using MamacitaOrbit.Scenes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MamacitaOrbit.Objects.Sprites;

namespace MamacitaOrbit.Managers
{
    public enum Waves
    {
        Wave1 = 1,
        Wave2, 
        Wave3, 
        Wave4, 
        Wave5, 
        Wave6, 
        Wave7, 
        Wave8, 
        Wave9, 
        Wave10,
        Wave11,
        Wave12,
        Wave13,
        Wave14,
        Wave15,
        Wave16,
        Wave17,
        Wave18,
        Wave19,
    }
    internal static class WaveManager
    {
        private record WaveData
        {
            public Dictionary<string, int> AlienSpawnChancePairs { get; set; }
            public int RemainingAliens { get; set; }
            public float AlienSpawnCooldown { get; set; }
        }

        private static readonly string waveJsonFilePath = Path.Combine(AppContext.BaseDirectory, "resources", "waves.json");
        private static FileSystemWatcher watcher;
        public static bool IsWaveEnded { get; set; } = false;
        public static bool IsWavePaused { get; set; } = false;
        public static Waves activeWave;
        private static float alienSpawnTime = 0;

        private static Dictionary<Waves, WaveData> wavesDictionary = new Dictionary<Waves, WaveData>();

        private static int aliensAddition = 0;
        private static float spawnCooldownSubstraction = 1;

        private static WaveData infiniteWave;
        public static event EventHandler OnWaveRecordBroken;
        public static bool IsWaveRecordBroken { get; private set; }
        private static int multiplier = 1;

        private static void WaveParameters(WaveData wave)
        {
            alienSpawnTime -= Globals.DeltaTime;

            if (DefendBase.IsNeutronFlashDetonated)
                return;


            if (alienSpawnTime > 0)
                return;

            if(wave.RemainingAliens == 0 && AlienManager.Aliens.Count == 0)
            {
                IsWaveEnded = true;
            }

            if (wave.RemainingAliens == 0)
            {
                return;
            }
            
            var spawnChance = Globals.Random.Next(0, 99);

            if (wave.AlienSpawnChancePairs.Sum(x => x.Value) > 100)
            {
                throw new ArgumentOutOfRangeException($"The sum of all the spawnchances of the wave {wave} has to be less than 100.");
            }

            foreach (var kvp in wave.AlienSpawnChancePairs.OrderBy(x => x.Value))
            {
                if (kvp.Value < spawnChance)
                {
                    continue;
                }

                if (AlienManager.AddAlien(AlienManager.GetAlienType(kvp.Key)) == false)
                    continue;

                wave.RemainingAliens--;
                alienSpawnTime = wave.AlienSpawnCooldown;
                break;
            }
        }

        private static WaveData InfiniteWaves()
        {
            if (wavesDictionary.Count == 0)
                return null;

            if (!wavesDictionary.TryGetValue(Waves.Wave19, out var baseWave))
            {
                baseWave = wavesDictionary.Values.Last();
            }

            if (infiniteWave == null)
            {
                multiplier++;
                aliensAddition = 30 * multiplier;

                spawnCooldownSubstraction /= 1.5f;

                infiniteWave = new WaveData
                {
                    AlienSpawnChancePairs = baseWave.AlienSpawnChancePairs,
                    RemainingAliens = aliensAddition,
                    AlienSpawnCooldown = baseWave.AlienSpawnCooldown * spawnCooldownSubstraction,
                };
            }

            return infiniteWave;
        }

        private static void LoadAllWaves()
        {
            try
            {
                var waveJson = File.ReadAllText(waveJsonFilePath);

                wavesDictionary = JsonSerializer.Deserialize<Dictionary<Waves, WaveData>>(waveJson);
            }
            catch(Exception ex) 
            {
                Debug.WriteLine($"Error while Reading the waveJson: {ex}");
            }
        }
        public static void NextRound(object sender, EventArgs e)
        {
            if (!IsWaveEnded)
            {
                Debug.WriteLine("The wave is not finished yet.");
                return;
            }

            IsWaveEnded = false;
            activeWave++;

            if ((int)Player.WaveRecord < (int)activeWave)
            {
                Player.WaveRecord = activeWave;
                OnWaveRecordBroken?.Invoke(null, EventArgs.Empty);
                IsWaveRecordBroken = true;
            }

            if (activeWave > Waves.Wave19)
            {
                if (!wavesDictionary.TryGetValue(Waves.Wave19, out var baseWave))
                    return;
                multiplier++;
                aliensAddition = 30 * multiplier;
                spawnCooldownSubstraction /= 1.5f;
                infiniteWave = new WaveData
                {
                    AlienSpawnChancePairs = baseWave.AlienSpawnChancePairs,
                    RemainingAliens = aliensAddition,
                    AlienSpawnCooldown = baseWave.AlienSpawnCooldown * spawnCooldownSubstraction,
                };
            }
        }

        private static void PauseRound()
        {
            IsWavePaused = !IsWavePaused;
        }

        public static void Init()
        {
            IsWavePaused = false;
            IsWaveEnded = false;
            IsWaveRecordBroken = false;
            activeWave = Waves.Wave1;

            watcher = new FileSystemWatcher(Path.GetDirectoryName(waveJsonFilePath)!,
                Path.GetFileName(waveJsonFilePath));

            watcher.Changed += OnFileChanged;
            watcher.EnableRaisingEvents = true;
            LoadAllWaves();
        }
        private static void OnFileChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Changed)
                return;
            
            Debug.WriteLine($"File {e.FullPath} changed successfully.");
            LoadAllWaves();
        }
        public static void Update()
        {
            if(IsWaveEnded || IsWavePaused) 
                return;

            if (wavesDictionary.TryGetValue(activeWave, out var waveAction) 
                && activeWave <= Waves.Wave19)
            {
                WaveParameters(waveAction);
            }
            else
            {
                WaveParameters(InfiniteWaves());
            }
        }
    }
}
