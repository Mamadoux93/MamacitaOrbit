using MamacitaOrbit.Main;
using MamacitaOrbit.Managers;
using MamacitaOrbit.UI.Canvases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MamacitaOrbit.UI.Buttons;
using MamacitaOrbit.UI.Interfaces;

namespace MamacitaOrbit.Scenes
{
    internal abstract class Scene : IScene
    {
        public List<Canvas> SceneCanvas { get; set; } = new List<Canvas>();
        public List<Button> SceneButtons { get; set; } = new List<Button>();
        public List<IUIObjects> UIObjects { get; set; } = new List<IUIObjects>();
        public List<IDraggable> Draggables { get; set; } = new List<IDraggable>();

        public Scene()
        {

        }
        public abstract void Update();
        public abstract void Draw();
        public abstract void InitUI();
        public abstract void DrawUI();
        public abstract void InitTexture();
    }
}
