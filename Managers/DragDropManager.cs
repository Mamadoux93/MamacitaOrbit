using MamacitaOrbit.Main;
using MamacitaOrbit.Scenes;
using MamacitaOrbit.UI;
using MamacitaOrbit.UI.Canvases;
using MamacitaOrbit.UI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MamacitaOrbit.Managers
{
    internal class DragDropManager
    {
        //private static readonly List<IDraggable> draggables = new List<IDraggable>();
        //private static readonly List<ITargetable> targetables = new List<ITargetable>();
        private static IDraggable dragItem;
        private static Point dragOffset;
        private static bool isDragging = false;

        public static void AddDraggable(IDraggable draggable)
        {
            SceneManager.GetCurrentScene().Draggables.Add(draggable);
        }

        /*public static void AddTargetable(ITargetable targetable)
        {
            targetables.Add(targetable);
        }
        */
        private static void CheckDragStart()
        {
            if (Globals.Drag & !isDragging)
            {
                foreach (var draggable in SceneManager.GetCurrentScene().Draggables)
                {
                    if (draggable is Canvas canvas && !canvas.Called)
                        continue;

                    if (draggable.Rect.Contains(InputManager.LogicalMouse.ToPoint()))
                    {
                        dragItem = draggable;
                        isDragging = true;
                        dragOffset = new Point((int)InputManager.LogicalMouse.ToPoint().X - draggable.Rect.X, (int)InputManager.LogicalMouse.ToPoint().Y - draggable.Rect.Y);
                        break;
                    }
                }
            }
        }

        private static void CheckDragStop()
        {
            if (!Globals.Drag && dragItem != null)
            {
                //CheckTarget();
                dragItem = null;
                isDragging = false;
            }
        }

        /*private static void CheckTarget()
        {
            if (dragItem is not Item)
                return;

            foreach (var targetable in targetables)
            {
                if (!targetable.Rect.Contains(InputManager.LogicalMouse.ToPoint()))
                    continue;
                
                dragItem.Rect = new Rectangle(targetable.Rect.X, targetable.Rect.Y, dragItem.Rect.Width, dragItem.Rect.Y);
                break;
            }
        }
        */
        public static void Update()
        {
            CheckDragStart();
            if (dragItem != null)
            {
                dragItem.Rect = new Rectangle(
                (int)InputManager.LogicalMouse.ToPoint().X - dragOffset.X,
                (int)InputManager.LogicalMouse.ToPoint().Y - dragOffset.Y,
                dragItem.Rect.Width,
                dragItem.Rect.Height
                );
            }
            CheckDragStop();
        }
    }
}
