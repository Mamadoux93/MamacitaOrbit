using MamacitaOrbit.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MamacitaOrbit.UI.Interfaces
{
    internal interface ITargetable
    {
        Rectangle Rect { get; }

        /*void RegisterTargetable()
        {
            DragDropManager.AddTargetable(this);
        }*/
    }
}
