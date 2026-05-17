using MamacitaOrbit.Main;
using MamacitaOrbit.Main.enums;
using MamacitaOrbit.Managers;
using MamacitaOrbit.Objects.Sprites.Mines;
using MamacitaOrbit.Scenes;
using MamacitaOrbit.UI.Buttons;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MamacitaOrbit.UI.Bars
{
    internal class PlayerItemsBar
    {
        private Button RocketButton { get; set; }
        public static Button MineButton { get; set; }
        public static Button FireworkButton { get; set; }

        private ItemsBar mainItemsBar;
        private ItemsBar rocketSubItemsBar;
        private ItemsBar mineSubItemsBar;
        private ItemsBar fireworkSubItemsBar;

        private List<ItemsBar> SubItemsBars { get; set; }

        public static Button DamageRocketButton { get; set; }
        public static Button DCR250Button { get; set; }
        public static Button WIZButton { get; set; }

        public static Button DamageMineButton { get; set; }
        public static Button SlowMineButton { get; set; }

        private Texture2D itemBarsButtonTextureNormal = Globals.Content.Load<Texture2D>("UI/itemsControl/114");
        private Texture2D itemBarsButtonTextureHovered = Globals.Content.Load<Texture2D>("UI/itemsControl/18");

        private float buttonIntersectCooldown = 0.1f;

        public PlayerItemsBar() 
        {
            SubItemsBars = new List<ItemsBar>();

            RocketButton = UIManager.AddButton(0, 0, 0, 0,
                itemBarsButtonTextureNormal,
                itemBarsButtonTextureHovered);

            MineButton = UIManager.AddButton(0, 0, 0, 0,
                itemBarsButtonTextureNormal,
                itemBarsButtonTextureHovered);

            FireworkButton = UIManager.AddButton(0, 0, 0, 0,
                itemBarsButtonTextureNormal,
                itemBarsButtonTextureHovered);

            mainItemsBar = new(new(900, 1000), RocketButton, MineButton, FireworkButton);

            FireworkButton.Icon = UIManager.AddIcon(FireworkButton.Rect.Center.X - 22, FireworkButton.Rect.Center.Y - 22, 45, 45,
                EffectManager.FireworkLargeRedDetonationFrames[20], RocketButton.DepthLayer - 0.000001f, null);

            MineButton.Icon = UIManager.AddIcon(MineButton.Rect.Center.X - 15, MineButton.Rect.Center.Y - 15, 30, 30,
                IconsTextures.DamageMine, MineButton.DepthLayer - 0.000001f, null);

            RocketButton.Icon = UIManager.AddIcon(RocketButton.Rect.Center.X - 20, RocketButton.Rect.Center.Y - 20, 40, 40,
                IconsTextures.R310, RocketButton.DepthLayer - 0.000001f, null);

            DamageRocketButton = UIManager.AddButton(0, 0, 0, 0,
                itemBarsButtonTextureNormal,
                itemBarsButtonTextureHovered);

            DCR250Button = UIManager.AddButton(0, 0, 0, 0,
                itemBarsButtonTextureNormal,
                itemBarsButtonTextureHovered);

            WIZButton = UIManager.AddButton(0, 0, 0, 0,
                itemBarsButtonTextureNormal,
                itemBarsButtonTextureHovered);

            SceneManager.GetCurrentScene().SceneButtons.Remove(DamageRocketButton);
            SceneManager.GetCurrentScene().SceneButtons.Remove(DCR250Button);
            SceneManager.GetCurrentScene().SceneButtons.Remove(WIZButton);

            rocketSubItemsBar = mainItemsBar.CreateSubItemsBar(new(RocketButton.Rect.X, RocketButton.Rect.Y - RocketButton.Rect.Height), 
                DamageRocketButton, DCR250Button, WIZButton);

            DamageRocketButton.Icon = UIManager.AddIcon(DamageRocketButton.Rect.Center.X - 20, DamageRocketButton.Rect.Center.Y - 20, 40, 40,
                            IconsTextures.R310, DamageRocketButton.DepthLayer - 0.000001f, null);

            DCR250Button.Icon = UIManager.AddIcon(DCR250Button.Rect.Center.X - 20, DCR250Button.Rect.Center.Y - 20, 40, 40,
                            IconsTextures.DCR250, DCR250Button.DepthLayer - 0.000001f, null);

            WIZButton.Icon = UIManager.AddIcon(WIZButton.Rect.Center.X - 20, WIZButton.Rect.Center.Y - 20, 40, 40,
                            IconsTextures.WIZ, WIZButton.DepthLayer - 0.000001f, null);

            SubItemsBars.Add(rocketSubItemsBar);

            RocketButton.OnClick += (s, e) => ToggleSubBar(rocketSubItemsBar);
            DamageRocketButton.OnClick += (s, e) => EquipRocket((EquippedRocket)Globals.Player.MaxRocketUpgrade);

            rocketSubItemsBar.IsHidden = true;

            DamageMineButton = UIManager.AddButton(0, 0, 0, 0,
                itemBarsButtonTextureNormal,
                itemBarsButtonTextureHovered);

            SlowMineButton = UIManager.AddButton(0, 0, 0, 0,
                itemBarsButtonTextureNormal,
                itemBarsButtonTextureHovered);

            SceneManager.GetCurrentScene().SceneButtons.Remove(DamageMineButton);
            SceneManager.GetCurrentScene().SceneButtons.Remove(SlowMineButton);

            mineSubItemsBar = mainItemsBar.CreateSubItemsBar(new(MineButton.Rect.X, MineButton.Rect.Y - MineButton.Rect.Height), DamageMineButton, SlowMineButton);

            DamageMineButton.Icon = UIManager.AddIcon(DamageMineButton.Rect.Center.X - 20, DamageMineButton.Rect.Center.Y - 20, 40, 40,
                            IconsTextures.DamageMine, DamageMineButton.DepthLayer - 0.000001f, null);

            SlowMineButton.Icon = UIManager.AddIcon(SlowMineButton.Rect.Center.X - 20, SlowMineButton.Rect.Center.Y - 20, 40, 40,
                            IconsTextures.SlowMine, SlowMineButton.DepthLayer - 0.000001f, null);

            MineButton.OnClick += (s, e) => ToggleSubBar(mineSubItemsBar);


            var fireworkButtons = new List<Button>();

            for (int i = 0; i < EffectManager.FireworkDetonationFramesList.Count; i++)
            {
                int index = i;

                var button = UIManager.AddButton(0, 0, 0, 0,
                    itemBarsButtonTextureNormal,
                    itemBarsButtonTextureHovered);

                button.Icon = UIManager.AddIcon(
                    button.Rect.Center.X - 22,
                    button.Rect.Center.Y - 22,
                    45, 45,
                    EffectManager.FireworkDetonationFramesList[index][20],
                    button.DepthLayer - 0.000001f,
                    null
                );

                SceneManager.GetCurrentScene().SceneButtons.Remove(button);

                button.OnClick += (s, e) =>
                {
                    int safeIndex = index > 2 ? 2 : index;

                    Globals.Player.ChosenFireworkExplosion =
                        EffectManager.FireworkDetonationFramesList[safeIndex];

                    FireworkButton.Icon.Texture = button.Icon.Texture;
                };

                fireworkButtons.Add(button);
            }

            FireworkButton.OnClick += (s, e) => ToggleSubBar(fireworkSubItemsBar);

            fireworkSubItemsBar = mainItemsBar.CreateSubItemsBar(new(FireworkButton.Rect.X, FireworkButton.Rect.Y - FireworkButton.Rect.Height), fireworkButtons.ToArray());
            mineSubItemsBar.IsHidden = true;
            fireworkSubItemsBar.IsHidden = true;
            SubItemsBars.Add(mineSubItemsBar);
            SubItemsBars.Add(fireworkSubItemsBar);
        }

        public static void EnableRocketButton(Button button, EquippedRocket equippedRocket)
        {
            button.OnClick += (s, e) => EquipRocket(equippedRocket);
        }

        public static void EnableMineButton(Button button, EquippedMine equippedMine)
        {
            button.OnClick += (s, e) => EquipMine(equippedMine);
        }


        private void ToggleSubBar(ItemsBar target)
        {
            if (buttonIntersectCooldown > 0)
                return;

            bool wasOpen = !target.IsHidden;

            foreach (var subBar in SubItemsBars)
            {
                subBar.IsHidden = true;
            }

            target.IsHidden = wasOpen;

            buttonIntersectCooldown = 0.1f;
        }

        private static void EquipRocket(EquippedRocket equippedRocket)
        {
            Globals.Player.EquippedRocket = equippedRocket;
            Globals.Player.ApplyRocketStats();
        }

        private static void EquipMine(EquippedMine equippedMine)
        {
            Globals.Player.EquippedMine = equippedMine;
        }

        public void Update()
        {
            mainItemsBar.Update();

            buttonIntersectCooldown -= Globals.DeltaTime;

            foreach (var subBar in SubItemsBars)
            {
                subBar.Update();
            }

            if(buttonIntersectCooldown <= 0)
            {
                foreach (var subBar in SubItemsBars)
                {
                    foreach (var button in subBar.Buttons)
                    {
                        button.IsIntersectable = false;
                    }
                }
            }
            else
            {
                foreach (var subBar in SubItemsBars)
                {
                    foreach (var button in subBar.Buttons)
                    {
                        button.IsIntersectable = true;
                    }
                }
            }

        }

        public void Draw()
        {
            if (IconsTextures.RocketIconPair.TryGetValue(Globals.Player.RocketTexture, out var rocketIcon))
            {
                RocketButton.Icon.Texture = rocketIcon;
            }

            if (RocketButton.Icon.Texture == IconsTextures.R310 ||
                RocketButton.Icon.Texture == IconsTextures.PLT2026 ||
                RocketButton.Icon.Texture == IconsTextures.PLT3030)
            {
                DamageRocketButton.Icon.Texture = RocketButton.Icon.Texture;
            }

            if (Globals.Player.EquippedMine == EquippedMine.DamageMine)
            {
                MineButton.Icon.Texture = IconsTextures.DamageMine;
            }
            else if (Globals.Player.EquippedMine == EquippedMine.SlowMine)
            {
                MineButton.Icon.Texture = IconsTextures.SlowMine;
            }

            foreach (var subBar in SubItemsBars)
            {
                subBar.Draw();
            }
        }
    }
}
