using MamacitaOrbit.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MamacitaOrbit.UI.Interfaces
{
    internal interface IUIObjects
    {
        public Rectangle Rect { get; set; }
        public float DepthLayer { get; set; }
        public void Draw();
        void RegisterUIObject()
        {
            SceneManager.GetCurrentScene().UIObjects.Add(this);
        }
    }
}
