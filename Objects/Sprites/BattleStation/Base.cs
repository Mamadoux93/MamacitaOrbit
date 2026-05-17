using MamacitaOrbit.Main;
using MamacitaOrbit.Managers;
using MamacitaOrbit.Objects.Interfaces;
using MamacitaOrbit.Objects.Sprites.InHeritance;
using MamacitaOrbit.UI.Canvases;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MamacitaOrbit.Objects.Sprites.BattleStation
{
    internal sealed class Base : DestroyableSprite, IDestroyable
    {
        private Vector2[] turretPositions = [];
        public Texture2D BaseTexture { get; set; }
        public string Name { get; set; }
        public Dictionary<Turret, Vector2> Turrets { get; set; } = new();

        private const int turretWidth = 100;
        private const int turretHeight = 100;
        public BaseLevel CurrentLevel { get; set; }

        private Texture2D level1Texture = Globals.Content.Load<Texture2D>("Sprite/BattleStation/base/base_hull/1");

        private Texture2D level2Texture = Globals.Content.Load<Texture2D>("Sprite/BattleStation/base/base_hull/2");

        private Texture2D level3Texture = Globals.Content.Load<Texture2D>("Sprite/BattleStation/base/base_hull/3");

        private Texture2D levelMaxShieldTexture = Globals.Content.Load<Texture2D>("Sprite/BattleStation/base/base_shields/3");

        public static List<Turret> TurretsInventory { get; set; } = new();

        public Base(Rectangle rectangle) : base(rectangle) 
        {
            CurrentLevel = BaseLevel.Level1;
            BaseTexture = Globals.Content.Load<Texture2D>("Sprite/BattleStation/base/base_hull/1");
            Texture = BaseTexture;
            rect = rectangle;

            origin = new(Texture.Width / 2, Texture.Height / 2);

            Health = 5000;
            MaxHealth = Health;

            rect.X = (int)(Globals.Bounds.X / 2) - rect.Width / 2;
            rect.Y = (int)(Globals.Bounds.Y / 2) - rect.Height / 2;

            turretPositions = new Vector2[8]
            {
                new(rect.Center.X + 240, rect.Center.Y - 200),
                new(rect.Center.X + 240, rect.Center.Y + 200),
                new(rect.Center.X, rect.Center.Y - 270),
                new(rect.Center.X, rect.Center.Y + 270),
                new(rect.Center.X - 300, rect.Center.Y),
                new(rect.Center.X + 300, rect.Center.Y),
                new(rect.Center.X - 240, rect.Center.Y + 200),
                new(rect.Center.X - 240, rect.Center.Y - 200),
            };
            
            position = new(rect.Center.X, rect.Center.Y);

            DepthLayer = LayerManager.BaseLayer;
        }
        public override void Destroy()
        {
            Globals.ALL_Freeze = true;
            IsDead = true;
        }
        public void AddTurret<T>(object sender, EventArgs e) where T : Turret
        {
            if (!TurretsInventory.OfType<T>().Any() || Turrets.Count == 8)
                return;

            var turretInventory = TurretsInventory.OfType<T>().First();

            TurretsInventory.Remove(turretInventory);

            var freePositions = (from position in turretPositions
                                where !Turrets.ContainsValue(position)
                                select position).ToList();

            var bullshit = Globals.Random.Next(freePositions.Count);
            
            var randomFreePosition = freePositions[bullshit];

            T turret = (T)Activator.CreateInstance(typeof(T),
                new Rectangle((int)randomFreePosition.X - turretWidth / 2, 
                (int)randomFreePosition.Y - turretHeight / 2, 
                turretWidth, turretHeight)
            );

            Turrets.Add(turret, randomFreePosition);
        }

        public void RemoveTurret<T>(object sender, EventArgs e) where T : Turret
        {
            if (!Turrets.Keys.OfType<T>().Any())
                return;

            var turretToDelete = Turrets.Keys.OfType<T>().First();

            if (turretToDelete is CreditTurret creditTurret)
            {
                GlobalsBonuses.CreditBonus -= creditTurret.CreditBonus;
            }

            TurretsInventory.Add(turretToDelete);
            Turrets.Remove(turretToDelete);
        }

        public override void Update()
        {
            foreach (var projectile in ProjectileManager.Projectiles)
            {
                if (rect.Contains(projectile.position) &&
                    !projectile.IsFromAlly)
                {
                    TakeDamage(projectile.Damage);
                    projectile.Destroy();
                }
            }

            if(Health <= 0)
            {
                Destroy();
            }
            if (Health > MaxHealth)
            {
                Health = MaxHealth;
            }

            foreach(var turret in Turrets)
            {
                turret.Key.Update();
            }
        }

        public void UpgradeBase()
        {
            switch (CurrentLevel)
            {
                case BaseLevel.Level1:
                    Texture = level1Texture;
                    Health = 5000;
                    MaxHealth = Health;
                    break;

                case BaseLevel.Level2:
                    Texture = level2Texture;
                    Health = 256000;
                    MaxHealth = Health;
                    break;

                case BaseLevel.Level3:
                    Texture = level3Texture;
                    Health = 2000000;
                    MaxHealth = Health;
                    break;

                case BaseLevel.LevelMax:
                    Texture = level3Texture;
                    Health = 5000000;
                    MaxHealth = Health;
                    break;
            }
            BaseTexture = Texture;
        }

        public override void Draw()
        {
            Name = $"{Globals.Player?.Name}'s Base";
            var hitboxTextureOpacity = 0.5f;
            var textureScale = 0.8f;

            Globals.SpriteBatch.Draw(Texture, position, null, Color.White, 0f, origin, textureScale, SpriteEffects.None, DepthLayer);
            if (Globals.IsDebugMode)
            {
                Globals.SpriteBatch.Draw(HitboxTexture, rect, null, Color.White * hitboxTextureOpacity, 0, origin, 0, DepthLayer);
            }

            if (CurrentLevel == BaseLevel.LevelMax)
            {
                Globals.SpriteBatch.Draw(levelMaxShieldTexture, 
                    new Vector2(rect.Center.X - levelMaxShieldTexture.Width / 2, 
                    rect.Center.Y - levelMaxShieldTexture.Height / 2), 
                    null, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, DepthLayer -0.000001f);
            }


            foreach (var turret in Turrets)
            {
                turret.Key.Draw();
            }
            HealthBarDraw(MaxHealth, 250, 15);
            DrawFont(Name, Color.White, 10);
        }
    }

    public enum BaseLevel
    {
        Level1,
        Level2,
        Level3,
        LevelMax
    }
}

