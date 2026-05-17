using MamacitaOrbit.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MamacitaOrbit.Achievements
{
    internal interface IAchievement
    {
        public event EventHandler OnUnlocked;
        public string Description { get; set; }
        public string Name { get; set; }
        public bool IsUnlocked { get; set; }
        void Update(object context);
        void RegisterToAchievementList()
        {
            AchievementManager.Achievements.Add(this);
        }
    }
}
