using MamacitaOrbit.Main;
using MamacitaOrbit.Managers;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MamacitaOrbit.Objects.Types
{
    internal class AlienType
    {
        public string Name { get; set; }
        public Texture2D Texture { get; set; }
        public Texture2D ProjectileTexture { get; set; }
        public int Health { get; set; }
        public int RewardCredit { get; set; }
        public int RewardUridium { get; set; }
        public int ProjectileSpeed { get; set; }
        public int ProjectileLifeSpan { get; set; }
        public int ProjectileDamage { get; set; }
        public float AlienSpeed { get; set; }
        public float FireCooldown { get; set; }
        public int Endurium { get; set; }
        public int Terbium { get; set; }
        public int Prometium { get; set; }
        public string ExplosionFramesPath { get; set; }
        public string ProjectileTexturePath { get; set; }
        public List<Texture2D> Frames { get; set; }
        public List<Texture2D> ExplosionFrames { get; set; }
        public bool IsRotating { get; set; }
        public SoundEffect[] ProjectileSounds { get; set; }

        public string TexturePathShit { get; set; }
        public AlienType(Texture2D texture, Texture2D projectileTexture, int health, int rewardCredit, int rewardUridium, 
            float alienSpeed, int endurium = 0, int terbium = 0, int prometium = 0,
            int projectileDamage= 10, int projectileLifeSpan = 3, int projectileSpeed = 200)
        {
            Texture = texture;
            Health = health;
            RewardCredit = rewardCredit;
            RewardUridium = rewardUridium;
            ProjectileSpeed = projectileSpeed;
            ProjectileLifeSpan = projectileLifeSpan;
            ProjectileDamage = projectileDamage;
            ProjectileTexture = projectileTexture;
            AlienSpeed = alienSpeed;
            Endurium = endurium;
            Terbium = terbium;
            Prometium = prometium;
        }

        public AlienType(List<Texture2D> frames, Texture2D projectileTexture,bool isRotating, int health, int rewardCredit, int rewardUridium,
            float alienSpeed, int endurium = 0, int terbium = 0, int prometium = 0,
            int projectileDamage = 10, int projectileLifeSpan = 3, int projectileSpeed = 200, float fireCooldown = 1)
        {
            this.Frames = frames;
            Health = health;
            RewardCredit = rewardCredit;
            RewardUridium = rewardUridium;
            ProjectileSpeed = projectileSpeed;
            ProjectileLifeSpan = projectileLifeSpan;
            ProjectileDamage = projectileDamage;
            ProjectileTexture = projectileTexture;
            AlienSpeed = alienSpeed;
            Endurium = endurium;
            Terbium = terbium;
            Prometium = prometium;
            IsRotating = isRotating;
            FireCooldown = fireCooldown;
        }


        public override string ToString() => Name;
    }
}
