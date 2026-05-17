using MamacitaOrbit.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MamacitaOrbit.UI.Interfaces
{
    internal interface IDraggable
    {
        Rectangle Rect { get; set; }

        void RegisterDraggable()
        {
            DragDropManager.AddDraggable(this);
        }
    }
}
