using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MamacitaOrbit.Objects.Types
{
    internal class CollectableTypes
    {
        public List<Texture2D> frames;
        public int Endurium { get; set; }
        public int Prometium { get; set; }
        public int Terbium { get; set; }
        public int CreditReward { get; set; }
        public int UridiumReward { get; set; }

        public CollectableTypes(List<Texture2D> frames, int endurium, int prometium, int terbium)
        {
            this.frames = frames;
            Endurium = endurium;
            Prometium = prometium;
            Terbium = terbium;
        }
    }
}
