using System;
using System.Collections.Generic;
using System.Text;

namespace DroidesApp_PlusTests_KT3
{

    public abstract class Droid : IComparable
    { 
        public string Id { get; } = Guid.NewGuid().ToString();
        private static int level = 0;
        public static int Level { get { return level; } }
        public Enum DroidStatus;
        public int Armor { get; }
        public int Attack { get; }
        public int HealthPoints { get; }
        public int EnergyPoints { get; }
        public abstract void Recover();
        public virtual void Upgrade()
        {
            level++;
        }

        public int CompareTo(object o)
        {
            Droid p = o as Droid;
            if (p != null)
                return this.Id.CompareTo(p.Id);
            else
                throw new Exception("Uanable to compare objects");
        }
        
    }

}
