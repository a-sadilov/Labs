using System;
using System.Threading;
using System.Reflection;

namespace DroidesApp_PlusTests_KT3
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                DroidesServer server = new DroidesServer();
                server.ListenAsync();
                Client client = new Client();
                while(true)
                    client.TranslateCommand();

            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e.Message);
            }
            
            /*server.CreateDroid(10, "w");
            server.CreateDroid(20, "s");
            server.CreateDroid(20, "d");
            server.Mine(10,1000,"energy", "w");
            server.Mine(2, 1000, "gems", "w");
            server.GetBaseInfo();
            server.GetDroidInfo();
            server.CreateBattle();
            server.JoinBattleGroup(20, "s");
            server.JoinBattleGroup(20, "d");
            server.StartBattle();
            server.GetDroidInfo();
            Thread.Sleep(5000);
            server.GetDroidInfo();
            server.GetBaseInfo();
            Thread.Sleep(20000);
            server.GetDroidInfo();
            server.GetBaseInfo();
            Thread.Sleep(50000);
            server.GetDroidInfo();
            server.GetBaseInfo();*/
        }
    }
}


