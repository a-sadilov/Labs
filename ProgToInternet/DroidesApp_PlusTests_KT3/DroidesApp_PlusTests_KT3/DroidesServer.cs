using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Reflection;
using System.Net;
using System.Threading.Tasks;

namespace DroidesApp_PlusTests_KT3
{
    public class DroidesServer
    {
        private const int serverPort = 8082;
        private IPAddress localIPAddr = (Dns.GetHostEntry(Dns.GetHostName())).AddressList[1];
        protected static int ironQuantity;
        protected static int gemsQuantity;
        protected static int energyQuantity;
        public static List<DroidWorker> droidWorkerCounter = new List<DroidWorker>();
        protected static List<DroidSoldier> droidSoldierCounter = new List<DroidSoldier>();
        protected static List<DroidDestroyer> droidDestroyerCounter = new List<DroidDestroyer>();
        public DroidesBattle droidesBattle;

        public DroidesServer(int _ironQuantity = 5000, int _gemsQuantity = 1000, int _energyQuantity = 2000)
        {
            ironQuantity = _ironQuantity;
            gemsQuantity = _gemsQuantity;
            energyQuantity = _energyQuantity;
        }

        public void AddDroid<T>(List<T> droidTypeCounter, T dw)
        {
            droidTypeCounter.Add(dw);
            droidTypeCounter.Sort();
        }
        public string CreateDroid(int quantity, string droidType)
        {
            try
            {
                if (quantity <= 0)
                    throw new Exception("Invalid first parameter in CreateDroid method");
                switch (droidType)
                {
                    case "w":   //worker
                        for (int i = 0; i < quantity; i++)
                        {
                            if (ironQuantity - 20 < 0 || gemsQuantity - 5 < 0 || energyQuantity - 5 < 0)
                                throw new Exception("Not enoght resources for creating Droid-Worker");
                            else
                            {
                                ironQuantity -= 20;
                                gemsQuantity -= 5;
                                energyQuantity -= 5;
                                AddDroid(droidWorkerCounter, new DroidWorker());
                            }
                        }
                        break;
                    case "s":   //soldier
                        for (int i = 0; i < quantity; i++)
                        {
                            if (ironQuantity - 50 < 0 || gemsQuantity - 10 < 0 || energyQuantity - 15 < 0)
                                throw new Exception("Not enoght resources for creating Droid-Soldier");
                            else
                            {
                                ironQuantity -= 50;
                                gemsQuantity -= 10;
                                energyQuantity -= 15;
                                AddDroid(droidSoldierCounter, new DroidSoldier());
                            }
                        }
                        break;
                    case "d":   //destroyer
                        for (int i = 0; i < quantity; i++)
                        {
                            if (ironQuantity - 100 < 0 || gemsQuantity - 25 < 0 || energyQuantity - 35 < 0)
                                throw new Exception("Not enoght resources for creating Droid-Destroyer");
                            else
                            {
                                ironQuantity -= 100;
                                gemsQuantity -= 25;
                                energyQuantity -= 35;
                                AddDroid(droidDestroyerCounter, new DroidDestroyer());
                            }
                        }
                        break;
                    default:
                        throw new Exception("Invalid second parameter in CreateDroid method");
                }
                return string.Format("Creating has been successfully processed");
            }
            catch (Exception e)
            {
                return string.Format(e.Message);
            }
        }
        public void DisassembleDroid(DroidWorker droid)
        {
            RemoveDroid<DroidWorker>(droidWorkerCounter, droid);
            ironQuantity += 10;
            gemsQuantity += 2;
        }
        public void DisassembleDroid(DroidSoldier droid)
        {
            RemoveDroid<DroidSoldier>(droidSoldierCounter, droid);
            ironQuantity += 25;
            gemsQuantity += 5;
        }
        public void DisassembleDroid(DroidDestroyer droid)
        {
            RemoveDroid<DroidDestroyer>(droidDestroyerCounter, droid);
            ironQuantity += 50;
            gemsQuantity += 12;
        }
        public void RemoveDroid<T>(List<T> droidList, T droid)
        {
            droidList.Remove(droid);
        }
        public string GetBaseInfo()
        {
            string str = string.Format("On Base:\nIron: {0}\t Gems: {1}\t Energy: {2}\t",
                ironQuantity, gemsQuantity, energyQuantity);
            return str += string.Format("Droids aviable:\nDroid-Wrokers: {0}\tDroid-Soldiers: {1}\tDroid-Destroyers: {2}\t",
                droidWorkerCounter?.Count, droidSoldierCounter?.Count, droidDestroyerCounter?.Count);
        }
        public string GetDroidInfo()
        {
            string allInfo = null;
            int i = 1;
            foreach (DroidWorker droid in droidWorkerCounter)
            {

                string str = string.Format("Worker_{0}\tStatus:{1}\tHP: {2}\tEnergy:{3}\tLevel: {4}\t ID: {5}\n",
                    i, droid.DroidStatus, droid.HealthPoints, droid.EnergyPoints, DroidWorker.Level, droid.Id);
                allInfo += str;
                i++;
            }
            i = 1;
            foreach (DroidSoldier droid in droidSoldierCounter)
            {
                string str = string.Format("Soldier_{0}\tStatus:{1}\tHP: {2}\tArmor: {3}\tEnergy:{4}\tLevel: {5}\t ID: {6}\n",
                    i, droid.DroidStatus, droid.HealthPoints, droid.Armor, droid.EnergyPoints, DroidWorker.Level, droid.Id);
                allInfo += str;
                i++;
            }
            i = 1;
            foreach (DroidDestroyer droid in droidDestroyerCounter)
            {
                string str = string.Format("Destroyer_{0}\tStatus:{1}\tHP: {2}\tArmor: {3}\tEnergy:{4}\tLevel: {5}\t ID: {6}\n",
                    i, droid.DroidStatus, droid.HealthPoints, droid.Armor, droid.EnergyPoints, DroidWorker.Level, droid.Id);
                allInfo += str;
                i++;
            }
            return allInfo;

        }
        public async Task<string> MineAsync(int quantity, int resource, string resourceType, string droidType)
        {
            if (quantity <= 0)
                return "Invalid first parameter in Mine method";
            switch (droidType)
            {
                case "w":
                    if (quantity <= droidWorkerCounter.Count)
                    {
                        foreach (DroidWorker element in droidWorkerCounter)
                        {
                            if (quantity != 0 && element.DroidStatus.ToString() == "Aviable")
                            {
                                resource /= quantity;
                                if (resource > 200)
                                    return "Not enought energy to mine per one droid";
                                await element.MineAsync(resource);
                                quantity--;
                                switch (resourceType)
                                {
                                    case "iron":
                                        ironQuantity += resource;
                                        break;
                                    case "gems":
                                        gemsQuantity += resource;
                                        break;
                                    case "energy":
                                        energyQuantity += resource;
                                        break;
                                }
                            }
                            if (element.DroidStatus.ToString() == "Lost")
                                RemoveDroid(droidWorkerCounter, element);

                        }
                        break;
                    }
                    return "Not enought Workers on Base";
                default:
                    return "Not correct Droid-Type used";
            }
            return "Mining complited";
        }
        public void CreateBattle()
        {
            this.droidesBattle = new DroidesBattle();
        }
        public void JoinBattleGroup(int quantity, string droidType)
        {
            try
            {
                switch (droidType)
                {
                    case "s":
                        if (quantity <= droidSoldierCounter.Count)
                        {
                            foreach (DroidSoldier element in droidSoldierCounter)
                            {
                                if (quantity != 0 && element.DroidStatus.ToString() == "Aviable")
                                {
                                    this.droidesBattle.AddToBattle(DroidesBattle.droidSoldierArmy, element);
                                    quantity--;
                                }
                            }
                            break;
                        }
                        throw new Exception("Not enought Soldiers on Base");
                        
                    case "d":

                        if (quantity <= droidDestroyerCounter.Count)
                        {
                            foreach (DroidDestroyer element in droidDestroyerCounter)
                            {
                                if (quantity != 0 && element.DroidStatus.ToString() == "Aviable")
                                {
                                    this.droidesBattle.AddToBattle(DroidesBattle.droidDestroyerArmy, element);
                                    quantity--;
                                }

                            }
                            break;
                        }
                        throw new Exception("Not enought Destroyers on Base");
                    default:
                        throw new Exception("Not correct Droid-Type used");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public void StartBattle()
        {
            droidesBattle.StartBattle();
        }
        public void ListenIncommingRequests(TcpListener listener)
        {
            while (true)
            {
                TcpClient incommingClient = listener.AcceptTcpClient();

                Console.WriteLine("Incomming call from {0} has just been accepted. Processing request...", incommingClient.Client.RemoteEndPoint);

                var task = Task.Factory.StartNew(() => this.ProcessRequest(incommingClient));
            }
        }
        public void ProcessRequest(TcpClient client)
        {
            using (NetworkStream stream = client.GetStream())
            {
                this.ProcessRequest(stream);
            }
        }
        public async void ListenAsync()
        {
            await Task.Run(() => Listen());
        }
        public void Listen()
        {
            TcpListener listener = null;
            try
            {
                listener = new TcpListener(localIPAddr, serverPort);
                listener.Start();
                Console.WriteLine("Server has started at the port {0}", serverPort);

                var listeningTask = Task.Factory.StartNew(() => ListenIncommingRequests(listener));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public void ProcessRequest(NetworkStream stream)
        {
            try
            {
                byte[] bytesForProtocol = new byte[17];
                stream.Read(bytesForProtocol, 0, 17);
                string protocol = Encoding.ASCII.GetString(bytesForProtocol);

                if (protocol != "selfMade_protocol")
                {
                    SendResponse("Unknown protocol", stream);
                    throw new Exception("Unknown protocol");
                }

                byte[] bytesForLength = new byte[4];
                stream.Read(bytesForLength, 0, 4);
                int length = BitConverter.ToInt32(bytesForLength, 0);

                byte[] bytesForCommand = new byte[length];
                stream.Read(bytesForCommand, 0, length);
                string message = Encoding.ASCII.GetString(bytesForCommand);

                string[] partsOfCommand = message.Split('#');

                Type myType = Type.GetType("DroidesApp_PlusTests_KT3.DroidesServer", false, true);
                foreach (MethodInfo method in myType.GetMethods()) 
                {
                    if (partsOfCommand[0] == method.Name)
                    {
                        switch (partsOfCommand[0])
                        {
                            case ("CreateDroid"):
                                int _quantity = Convert.ToInt32(partsOfCommand[1]);
                                string _droidtype = partsOfCommand[2];
                                SendResponse(this.CreateDroid(_quantity, _droidtype) +"\n"+ this.GetDroidInfo(), stream);
                                
                                break;
                            case ("Mine"):
                                int _quantityMine = Convert.ToInt32(partsOfCommand[1]);
                                int _resource = Convert.ToInt32(partsOfCommand[2]);
                                string _resourcetype = partsOfCommand[3];
                                string _droidtypeMine = partsOfCommand[4];
                                SendResponse((this.MineAsync(_quantityMine, _resource,_resourcetype, _droidtypeMine)).Result.ToString(),stream);
                                
                                break;
                            case ("GetBaseInfo"):
                                SendResponse(this.GetBaseInfo(),stream);
                                break;
                            case ("GetDroidInfo"):
                                SendResponse(this.GetDroidInfo(),stream);
                                break;
                            case ("CreateBattle"):
                                this.CreateBattle();
                                break;
                            case ("JoinBattleGroup"): 
                                int _quantityB = Convert.ToInt32(partsOfCommand[1]);
                                string _droidtypeB = partsOfCommand[2];
                                this.JoinBattleGroup(_quantityB,_droidtypeB);
                                break;
                            case ("StartBattle"):
                                this.StartBattle();
                                SendResponse(this.GetDroidInfo(), stream);
                                break;
                        }
                        string reponse = string.Format("Command \"{0}\" has been successfully processed", partsOfCommand[0]);
                        SendResponse(reponse, stream);
                    }
                }
            }
            catch (Exception e) 
            {
                Console.WriteLine(e.Message);
            }
        }
        private void SendResponse(string response, NetworkStream stream)
        {
            byte[] buffer = ProtocolDefinition.GetBytes(response);
            stream.Write(buffer, 0, buffer.Length);
        }
    }
}
