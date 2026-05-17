using MamacitaOrbit.UI.Buttons;
using MamacitaOrbit.UI.Canvases;
using MamacitaOrbit.UI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MamacitaOrbit.Scenes
{
    internal interface IScene
    {
        public void Update();
        public void Draw();
        public void InitUI();
        public void DrawUI();
        public void InitTexture();
        public List<Canvas> SceneCanvas { get; set; }
        public List<Button> SceneButtons { get; set; }
        public List<IUIObjects> UIObjects { get; set; }
        public List<IDraggable> Draggables { get; set; }
    }
}   
