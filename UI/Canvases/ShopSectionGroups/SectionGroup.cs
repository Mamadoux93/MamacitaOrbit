using MamacitaOrbit.Objects.Sprites.BattleStation;
using MamacitaOrbit.UI.Buttons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MamacitaOrbit.UI.Canvases.ShopSectionGroups
{
    internal abstract class SectionGroup
    {
        protected Page[] pages;

        protected Vector2 upLeftIconPosition = new Vector2(5, 95);
        protected Vector2 upRightIconPosition = new Vector2(210, 100);
        protected Vector2 downLeftIconPosition = new Vector2(5, 200);
        protected Vector2 downRightIconPosition = new Vector2(210, 200);

        protected Vector2 upLeftButtonPosition = new Vector2(70, 100);
        protected Vector2 upRightButtonPosition = new Vector2(275, 100);
        protected Vector2 downLeftButtonPosition = new Vector2(70, 200);
        protected Vector2 downRightButtonPosition = new Vector2(275, 200);

        protected class Page
        {
            public List<Icon> Icons { get; set; } = new();
            public List<Text> Texts { get; set; } = new();
            public List<Button> Buttons { get; set; } = new();

            public void Update()
            {
                foreach (var button in Buttons)
                {
                    button.Update();
                }
            }

            public void Draw()
            {
                foreach (var button in Buttons)
                {
                    button.Draw();
                }
                foreach (var icon in Icons)
                {
                    icon.Draw();
                }
                foreach (var text in Texts)
                {
                    text.Draw();
                }
            }
        }

        protected Page currentPage;

        protected Button nextPageButton;
        protected Button previousPageButton;
        public SelectButton SectionButton { get; set; }

        protected CanvasShop canvasShop;

        protected static Dictionary<TurretLevel, Texture2D> taLevelTexturePair = new Dictionary<TurretLevel, Texture2D>()
        {
            { TurretLevel.Level1, AttackTurret.level1IdleTexture },
            { TurretLevel.Level2, AttackTurret.level2IdleTexture },
            { TurretLevel.Level3, AttackTurret.level3IdleTexture },
            { TurretLevel.LevelMax, AttackTurret.level3IdleTexture },
        };

        public SectionGroup(CanvasShop canvasShop)
        {
            var page1 = new Page();
            pages = new Page[]
            {
                page1,
            };

            currentPage = pages[0];

            nextPageButton = canvasShop.AddButton(450, 100, 30, 60, Color.White, Color.Gray, NextPage, buttonText: "=>");
            previousPageButton = canvasShop.AddButton(420, 100, 30, 60, Color.White, Color.Gray, PreviousPage, buttonText: "<=");

            this.canvasShop = canvasShop;
            canvasShop.SectionGroups.Add(this);
        }

        protected void UpdatePageNavigatorButtons()
        {
            if (pages.Length == 1)
                return;

            if (Array.IndexOf(pages, currentPage) != 0)
            {
                previousPageButton.Update();
            }

            if (Array.IndexOf(pages, currentPage) != pages.Length - 1)
            {
                nextPageButton.Update();
            }
        }
        
        protected void DrawPageNavigatorButtons()
        {
            if (pages.Length == 0)
                return;

            if (Array.IndexOf(pages, currentPage) != 0)
            {
                previousPageButton.Draw();
            }


            if (Array.IndexOf(pages, currentPage) != pages.Length - 1)
            {
                nextPageButton.Draw();
            }
        }

        private void NextPage(object sender, EventArgs e)
        {
            var index = Array.IndexOf(pages, currentPage);
            if (index != pages.Length - 1)
            {
                currentPage = pages[index + 1];
            }
        }

        private void PreviousPage(object sender, EventArgs e)
        {
            var index = Array.IndexOf(pages, currentPage);

            if (index != 0)
            {
                currentPage = pages[index - 1];
            }
        }

        public virtual void Update()
        {
            UpdatePageNavigatorButtons();
            currentPage.Update();
        }

        public virtual void Draw()
        {
            DrawPageNavigatorButtons();
            currentPage.Draw();
        }
    }
}
