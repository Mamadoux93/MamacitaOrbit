using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MamacitaOrbit.Scenes
{
    internal static class SceneManager
    {
        private static Stack<IScene> sceneStack;

        public static void Init()
        {
            sceneStack = new Stack<IScene>();
        }

        public static void AddScene(IScene scene)
        {
            if (sceneStack.Count > 0)
            {
                GetCurrentScene().UIObjects.Clear();
                GetCurrentScene().Draggables.Clear();
            }

            sceneStack.Push(scene);
            scene.InitTexture();
            scene.InitUI();
        }

        public static void RemoveScene()
        {
            if (sceneStack.Count > 0)
            {
                GetCurrentScene().UIObjects.Clear();
                GetCurrentScene().Draggables.Clear();
            }

            sceneStack.Pop();
        }

        public static IScene GetCurrentScene()
        { 
            return sceneStack.Peek();
        }
    }
}
