using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DroidesApp_PlusTests_KT3
{
    public class DroidSoldier : Droid
    {
        public new Enum DroidStatus = DroidesApp_PlusTests_KT3.DroidStatus.Aviable;
        public new int Armor { get; set; } = 50 + Level * 5;
        public new int Attack { get; set; } = 5 + Level;
        public new int HealthPoints { get; set; } = 100 + Level * 5;
        public new int EnergyPoints { get; set; } = 100 + Level *2;

        public override void Recover()
        {
            HealthPoints = EnergyPoints = 100 + Level * 5;
            Armor = 50 + Level * 5;
            EnergyPoints = 100 + Level * 2;
        }
        public DroidSoldier Fight(int resource)
        {
            int DroidTaskTime = resource / this.Attack;
            if (this.EnergyPoints - DroidTaskTime <= 0)
                return this;
            this.DroidStatus = DroidesApp_PlusTests_KT3.DroidStatus.DoingTask;
            this.EnergyPoints -= DroidTaskTime;
            //Thread.Sleep(DroidTaskTime * 1000);
            Random rnd = new Random();
            if (rnd.Next(0, 500) < 1)
            {
                this.DroidStatus = DroidesApp_PlusTests_KT3.DroidStatus.Lost;
                return this;
            }
            if ((this.HealthPoints + this.Armor) - resource <= 0)
            {
                this.DroidStatus = DroidesApp_PlusTests_KT3.DroidStatus.Destroed;
                return this;
            }
            if (this.Armor - resource <= 0)
            {
                this.HealthPoints -= (resource - this.Armor);
                this.Armor = 0;
                this.DroidStatus = DroidesApp_PlusTests_KT3.DroidStatus.Aviable;
                return this;
            }
            else
            {
                this.Armor -= resource;
                this.DroidStatus = DroidesApp_PlusTests_KT3.DroidStatus.Aviable;
                return this;
            }
        }
        public async Task<DroidSoldier> FightAsync(int resource)
        {
            return await Task.Run(() => this.Fight(resource));
        }
    }
}
