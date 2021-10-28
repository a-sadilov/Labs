using System;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace FunwithSoketsApp_KT_1
{
    class ConsoleClient
    {
        private IPAddress mcastAddress;
        private const int mcastPort = 11011;
        private IPAddress localIPAddr = (Dns.GetHostEntry(Dns.GetHostName())).AddressList[1];
        private Socket mcastSocket;
        private void JoinMulticastGroup(string _mcastAddress)
        {
            try
            {
                // Create a multicast socket.
                this.mcastAddress = IPAddress.Parse(_mcastAddress);
                this.mcastSocket = new Socket(AddressFamily.InterNetwork,
                                         SocketType.Dgram,
                                         ProtocolType.Udp);

                // Create an IPEndPoint object.
                IPEndPoint IPlocal = new IPEndPoint(localIPAddr, 0);

                // Bind this endpoint to the multicast socket.
                mcastSocket.Bind(IPlocal);
                mcastSocket.EnableBroadcast = true;
                // Define a MulticastOption object specifying the multicast group
                // address and the local IP address.
                // The multicast group address is the same as the address used by the listener.
                MulticastOption mcastOption;
                mcastOption = new MulticastOption(mcastAddress, localIPAddr);

                mcastSocket.SetSocketOption(SocketOptionLevel.IP,
                                            SocketOptionName.AddMembership,
                                            mcastOption);
            }
            catch (Exception e)
            {
                Console.WriteLine("\n" + e.ToString());
            }
        }
        public async void BroadcastMessageAsync(string message)
        {
            await Task.Run(()=>this.BroadcastMessage(message));
        }
        public void BroadcastMessage(string message)
        {
            Console.WriteLine("Multicast data sent.....");
            IPEndPoint endPoint;
            try
            {
                foreach (ConsoleListener cl in ConsoleListener.servers)
                {
                    //Send multicast packets to the listener.
                    endPoint = new IPEndPoint(this.mcastAddress, cl.mcastPort);
                    this.mcastSocket.SendTo(ASCIIEncoding.ASCII.GetBytes(message), endPoint);
                    
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("\n" + e.ToString());
            }
            this.mcastSocket.Close();
        }

        private void GetListenersInfo()
        {
            Console.WriteLine("\t\tList of Servers\t\t");
            foreach (ConsoleListener a in ConsoleListener.servers) 
            {
                if (this.mcastAddress.Equals(a.mcastAddress))
                    Console.WriteLine("Host: {0}\t IP: {1}", a.HostName, a.localIPAddr);
            }
        }
        static void Main(string[] args)
        {
            try
            {
                ConsoleListener server = new ConsoleListener();
                ConsoleListener server2 = new ConsoleListener(11012);
                ConsoleListener server3 = new ConsoleListener(11013);
                server.StartMulticast("224.168.100.2");
                server.AddListener();

                server2.StartMulticast("224.168.100.2");
                server2.AddListener();

                server3.StartMulticast("224.168.100.2");
                server3.AddListener();

                ConsoleClient client = new ConsoleClient();
                client.JoinMulticastGroup("224.168.100.2");
                Thread.Sleep(1000);
                client.GetListenersInfo();
                server.ListenAsync();
                server2.ListenAsync();
                server3.ListenAsync();
                client.BroadcastMessage("Whats up?");
            }
            catch (Exception ex)
            {
                ConsoleListener.DisconnectAllListeners();
                Console.WriteLine(ex.Message);
            }
        }
    }
}
