using MamacitaOrbit.Objects.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MamacitaOrbit.Achievements
{
    internal class Achievement<T> : IAchievement
    {
        public event EventHandler OnUnlocked;
        public Predicate<T> UnlockCondition { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public bool IsUnlocked { get; set; }

        public Achievement(string name, string description, Predicate<T> unlockCondition)
        {
            Name = name;
            Description = description;
            UnlockCondition = unlockCondition;  
            IsUnlocked = false;

            (this as IAchievement).RegisterToAchievementList();
        }

        private void Unlock()
        {
            OnUnlocked?.Invoke(this, EventArgs.Empty);
        }

        public void Update(object context)
        {
            if (context is T typedContext)
            {
                if (!IsUnlocked && UnlockCondition(typedContext))
                {
                    IsUnlocked = true;
                    Unlock();
                }
            }
        }
    }
}
