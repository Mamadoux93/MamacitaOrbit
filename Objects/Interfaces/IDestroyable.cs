using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MamacitaOrbit.Objects.Interfaces
{
    internal interface IDestroyable
    {
        int Health { get; set; }
        void TakeDamage(int damage);
        public void Destroy();

    }
}
