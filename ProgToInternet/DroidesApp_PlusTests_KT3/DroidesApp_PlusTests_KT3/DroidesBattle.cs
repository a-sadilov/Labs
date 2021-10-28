using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace DroidesApp_PlusTests_KT3
{
    public class DroidesBattle : DroidesServer
    {
        //protected internal string Id { get; private set; }
        public static List<DroidSoldier> droidSoldierArmy = new List<DroidSoldier>();
        public static List<DroidDestroyer> droidDestroyerArmy = new List<DroidDestroyer>();
        /*public DroidesBattle()
        {
            Id = Guid.NewGuid().ToString();
        }*/
        public void AddToBattle<T>(List<T> droidTypeCounter, T dw)
        {
            droidTypeCounter.Add(dw);
        }
        public void RemoveFromBattle<T>(List<T> droidTypeCounter, T dw)
        {
            droidTypeCounter.Remove(dw);
        }
        public new async void StartBattle()
        {
            int Resource = 0;
            if (droidDestroyerArmy != null) 
            {
                foreach (DroidDestroyer droidDestroyer in droidDestroyerArmy)
                {
                    
                    //int index = droidDestroyerCounter.BinarySearch(droidDestroyer, new DroidComparer());
                    
                    Random rand = new Random();
                    int resource = (droidDestroyer.Armor + droidDestroyer.HealthPoints) / rand.Next(2, 9);
                    DroidDestroyer droidDestroyerUpdate = await droidDestroyer.FightAsync(resource);
                    //droidDestroyerCounter.Insert(index, droidDestroyerUpdate);
                    
                    if (droidDestroyer.DroidStatus.ToString() == "Aviable")
                        Resource += resource;
                }
            }
            if (droidSoldierArmy != null) 
            {
                foreach (DroidSoldier droidSoldier in droidSoldierArmy)
                {
                    //int index = droidSoldierCounter.BinarySearch(droidSoldier);
                    Random rand = new Random();
                    int resource = (droidSoldier.Armor + droidSoldier.HealthPoints) / rand.Next(2, 10); ;
                    DroidSoldier droidSoldierUpdate = await droidSoldier.FightAsync(resource);
                    //droidSoldierCounter.Insert(index, droidSoldierUpdate);
                    if (droidSoldier.DroidStatus.ToString() == "Aviable")
                        Resource += resource;
                }
            }
            Random rnd = new Random();
            int intRnd = rnd.Next(2, 7);
            gemsQuantity += (Resource / intRnd);
            ironQuantity += (Resource - gemsQuantity);
        }
    }
}
