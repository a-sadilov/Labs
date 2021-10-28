using Microsoft.VisualStudio.TestTools.UnitTesting;
using DroidesApp_PlusTests_KT3;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using System.Threading;

namespace DroidesApp_PlusTests_KT3.Tests
{
    [TestClass()]
    public class DroidesServerTests
    {
        //Unit tests
        [TestMethod()]
        public void CreateDroidTest_NotEnoughResourcesForWorker_ShouldReturnString()
        {
            string exceptionOutput = "Not enoght resources for creating Droid-Worker";
            DroidesServer server = new DroidesServer(20, 4, 5); //(20, 5, 5)
            string output = server.CreateDroid(1, "w");
            try
            {
                Assert.AreEqual(exceptionOutput, output);
                return;
            }
            catch (AssertFailedException)
            {

                Assert.Fail("The expected exception-message was not thrown.");
                //return;
            }

        }
        [TestMethod()]
        public void CreateDroidTest_NotEnoughResourcesForSoldier_ShouldReturnString()
        {
            string exceptionOutput = "Not enoght resources for creating Droid-Soldier";
            DroidesServer server = new DroidesServer(50, 10, 5); //(50, 10, 15)
            string output = server.CreateDroid(1, "s");
            try
            {
                Assert.AreEqual(exceptionOutput, output);
                return;
            }
            catch (AssertFailedException)
            {
                Assert.Fail("The expected exception-message was not thrown.");
            }
        }
        [TestMethod()]
        public void CreateDroidTest_NotEnoughResourcesForDestroyer_ShouldReturnString()
        {
            string exceptionOutput = "Not enoght resources for creating Droid-Destroyer";
            DroidesServer server = new DroidesServer(100, 24, 35); //(100, 25, 35)
            string output = server.CreateDroid(1, "d");
            try
            {
                Assert.AreEqual(exceptionOutput, output);
                return;
            }
            catch (AssertFailedException)
            {
                Assert.Fail("The expected exception-message was not thrown.");
            }
        }
        [TestMethod()]
        public void CreateDroidTest_WrongFirstParameter_ShouldReturnString()
        {
            string exceptionOutput = "Invalid first parameter in CreateDroid method";
            DroidesServer server = new DroidesServer(1000, 240, 350);
            string output = server.CreateDroid(0, "d"); //(1,"d")
            try
            {
                Assert.AreEqual(exceptionOutput, output);
                return;
            }
            catch (AssertFailedException)
            {
                Assert.Fail("The expected exception-message was not thrown.");
            }
        }
        [TestMethod()]
        public void CreateDroidTest_WrongSecondParameter_ShouldReturnString()
        {
            string exceptionOutput = "Invalid second parameter in CreateDroid method";
            DroidesServer server = new DroidesServer(1000, 240, 350);
            string output = server.CreateDroid(2, "q"); //(2,"d")
            try
            {
                Assert.AreEqual(exceptionOutput, output);
                return;
            }
            catch (AssertFailedException)
            {
                Assert.Fail("The expected exception-message was not thrown.");
            }
        }
        [TestMethod()]
        public void MineTest_WrongFirstParameter_ShouldThrowException()
        {
            string exceptionOutput = "Invalid first parameter in Mine method";
            DroidesServer server = new DroidesServer(1000, 240, 350);
            server.CreateDroid(2, "d");
            server.CreateDroid(2, "w");
            Task<string> output = server.MineAsync(-1, 100, "iron", "w");
            string message = output.Result.ToString();
            try
            {
                Assert.AreEqual(exceptionOutput, message);
                return;
            }
            catch (AssertFailedException)
            {
                Assert.Fail("The expected exception-message was not thrown.");
            }



        }
        [TestMethod()]
        public void MineTest_WrongSecondParameter_ShouldThrowException()
        {
            string exceptionOutput = "Not enought energy to mine per one droid";
            DroidesServer server = new DroidesServer(1000, 240, 350);
            server.CreateDroid(2, "d");
            server.CreateDroid(2, "w");
            Task<string> output = server.MineAsync(2, 1000, "iron", "w");
            string message = output.Result.ToString();
            try
            {
                Assert.AreEqual(exceptionOutput, message);
                return;
            }
            catch (AssertFailedException)
            {
                Assert.Fail("The expected exception-message was not thrown.");
            }
        }
        [TestMethod()]
        public void MineTest_WrongDroidTypeUsed_ShouldThrowException()
        {
            string exceptionOutput = "Not correct Droid-Type used";
            DroidesServer server = new DroidesServer(1000, 240, 350); //(100, 25, 35)
            server.CreateDroid(1, "d");
            server.CreateDroid(1, "w");
            Task<string> output = server.MineAsync(1, 50, "iron", "d");
            string message = output.Result.ToString();
            try
            {
                Assert.AreEqual(exceptionOutput, message);
                return;
            }
            catch (AssertFailedException)
            {
                Assert.Fail("The expected exception-message was not thrown.");
            }
        }


        //System-Funtional Tests
        [TestMethod()]
        public void RequestTest_Create10DroidWorkers_ChangesResources()
        {
            int droidQuantity = 10;
            string droidType = "w";
            string commandCreateDroid = string.Format("CreateDroid#{0}#{1}", droidQuantity, droidType);
            string commandGetBaseInfo = "GetBaseInfo";

            int baseIronQuantity = 1200;
            int baseGemsQuantity = 1050;
            int baseEnergyQuantity = 1050;
            DroidesServer server = new DroidesServer(baseIronQuantity, baseGemsQuantity, baseEnergyQuantity);
            server.ListenAsync();

            try
            {
                string[] response = ResponseGetter(commandGetBaseInfo);
                Assert.AreEqual(Convert.ToInt32(response[0]), baseIronQuantity);
                Assert.AreEqual(Convert.ToInt32(response[1]), baseGemsQuantity);
                Assert.AreEqual(Convert.ToInt32(response[2]), baseEnergyQuantity);
                //server.CreateDroid(droidQuantity,droidType);
                ResponseGetter(commandCreateDroid);
                response = ResponseGetter(commandGetBaseInfo);
                Assert.AreEqual(Convert.ToInt32(response[0]), baseIronQuantity - (20 * droidQuantity));
                Assert.AreEqual(Convert.ToInt32(response[1]), baseGemsQuantity - (5 * droidQuantity));
                Assert.AreEqual(Convert.ToInt32(response[2]), baseEnergyQuantity - (5 * droidQuantity));
                return;
            }
            catch (AssertFailedException)
            {
                Assert.Fail("The expected exception-message was not thrown.");
            }
        }

        [TestMethod()]
        public void RequestTest_Create10DroidSoldiersPlusBattle_ChangesResources()
        {
            int droidQuantity = 10;
            string droidType = "s";
            string commandCreateDroid = string.Format("CreateDroid#{0}#{1}", droidQuantity, droidType);
            string commandGetBaseInfo = "GetBaseInfo";

            int baseIronQuantity = 5000;
            int baseGemsQuantity = 3000;
            int baseEnergyQuantity = 4000;
            DroidesServer server = new DroidesServer(baseIronQuantity, baseGemsQuantity, baseEnergyQuantity);
            server.ListenAsync();

            try
            {
                string[] response = ResponseGetter(commandGetBaseInfo);
                Assert.AreEqual(Convert.ToInt32(response[0]), baseIronQuantity);
                Assert.AreEqual(Convert.ToInt32(response[1]), baseGemsQuantity);
                Assert.AreEqual(Convert.ToInt32(response[2]), baseEnergyQuantity);
                //server.CreateDroid(droidQuantity,droidType);
                ResponseGetter(commandCreateDroid);
                response = ResponseGetter(commandGetBaseInfo);
                Assert.AreEqual(Convert.ToInt32(response[0]), baseIronQuantity - (50 * droidQuantity));
                Assert.AreEqual(Convert.ToInt32(response[1]), baseGemsQuantity - (10 * droidQuantity));
                Assert.AreEqual(Convert.ToInt32(response[2]), baseEnergyQuantity - (15 * droidQuantity));
                RequestSetter("CreateBattle");
                RequestSetter("JoinBattleGroup#10#s");
                ResponseGetter("StartBattle");
                Thread.Sleep(1000);
                response = ResponseGetter(commandGetBaseInfo);
                Assert.AreNotEqual(baseIronQuantity, Convert.ToInt32(response[0]));
                Assert.AreNotEqual(baseGemsQuantity, Convert.ToInt32(response[1]));
                Assert.AreNotEqual(baseEnergyQuantity, Convert.ToInt32(response[2]));
                return;
            }
            catch (AssertFailedException)
            {
                Assert.Fail("The expected exception-message was not thrown.");
            }
        }

        [TestMethod()]
        public void RequestTest_Create30DroidWorkersPlusMine_ChangesResources()
        {
            int droidQuantity = 30;
            string droidType = "w";
            string commandCreateDroid = string.Format("CreateDroid#{0}#{1}", droidQuantity, droidType);
            string commandGetBaseInfo = "GetBaseInfo";

            int baseIronQuantity = 5000;
            int baseGemsQuantity = 3000;
            int baseEnergyQuantity = 4000;
            DroidesServer server = new DroidesServer(baseIronQuantity, baseGemsQuantity, baseEnergyQuantity);
            server.ListenAsync();

            try
            {
                string[] response = ResponseGetter(commandGetBaseInfo);
                Assert.AreEqual(Convert.ToInt32(response[0]), baseIronQuantity);
                Assert.AreEqual(Convert.ToInt32(response[1]), baseGemsQuantity);
                Assert.AreEqual(Convert.ToInt32(response[2]), baseEnergyQuantity);
                //server.CreateDroid(droidQuantity,droidType);
                ResponseGetter(commandCreateDroid);
                response = ResponseGetter(commandGetBaseInfo);
                Assert.AreEqual(Convert.ToInt32(response[0]), baseIronQuantity - (20 * droidQuantity));
                Assert.AreEqual(Convert.ToInt32(response[1]), baseGemsQuantity - (5 * droidQuantity));
                Assert.AreEqual(Convert.ToInt32(response[2]), baseEnergyQuantity - (5 * droidQuantity));
                RequestSetter("Mine#10#1000#iron#w");
                RequestSetter("Mine#10#1000#gems#w");
                RequestSetter("Mine#10#1000#energy#w");
                Thread.Sleep(1000);
                response = ResponseGetter(commandGetBaseInfo);
                Assert.AreNotEqual(baseIronQuantity, Convert.ToInt32(response[0]));
                Assert.AreNotEqual(baseGemsQuantity, Convert.ToInt32(response[1]));
                Assert.AreNotEqual(baseEnergyQuantity, Convert.ToInt32(response[2]));
                return;
            }
            catch (AssertFailedException)
            {
                Assert.Fail("The expected exception-message was not thrown.");
            }
        }

        public static string[] ResponseGetter(string command)
        {
            byte[] commandbyte = ProtocolDefinition.GetBytes(command);
            int port = 8082;
            string serverIP = "192.168.0.106";
            TcpClient client = new TcpClient();
            client.Connect(serverIP, port);
            NetworkStream stream = client.GetStream();
            stream.Write(commandbyte, 0, commandbyte.Length);

            byte[] bytesForProtocol = new byte[17];
            stream.Read(bytesForProtocol, 0, 17);
            string protocol = Encoding.ASCII.GetString(bytesForProtocol);

            if (protocol != "selfMade_protocol")
            {
                throw new Exception("Unknown protocol");
            }

            byte[] bytesForLength = new byte[4];
            stream.Read(bytesForLength, 0, 4);
            int length = BitConverter.ToInt32(bytesForLength, 0);
            byte[] bytesForResponse = new byte[length];
            stream.Read(bytesForResponse, 0, length);
            string response = Encoding.ASCII.GetString(bytesForResponse);

            var x = from e in response.Split(": ")
                    where e.Contains('\t')
                    select e.Split('\t').First();
            string[] subresponse = new string[x.Count()];
            int i = 0;
            foreach (string e in x)
                subresponse[i++] += e.ToString();
            return subresponse;
        }



        public static void RequestSetter(string command)
        {
            byte[] commandbyte = ProtocolDefinition.GetBytes(command);
            int port = 8082;
            string serverIP = "192.168.0.106";
            TcpClient client = new TcpClient();
            client.Connect(serverIP, port);
            NetworkStream stream = client.GetStream();
            stream.Write(commandbyte, 0, commandbyte.Length);

            
        }
    }

}                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                      