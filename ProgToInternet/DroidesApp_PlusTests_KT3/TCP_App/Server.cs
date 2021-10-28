using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace TCP_App
{
    class Server
    {
        static RequestProcessor pocessor = new RequestProcessor();

        static IPAddress addressForAccessToServer = IPAddress.Any;
        const int serverPort = 8000;

        static void Main(string[] args)
        {
            TcpListener listener = null;
            try
            {
                listener = new TcpListener(addressForAccessToServer, serverPort);
                listener.Start();
                Console.WriteLine("Server has started at the port {0}", serverPort);

                var listeningTask = Task.Factory.StartNew(() => ListenIncommingRequests(listener));

                Console.WriteLine("To stop the server press any key");
                Console.ReadKey();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                if (listener != null)
                    listener.Stop();
            }
        }

        private static void ListenIncommingRequests(TcpListener listener)
        {
            while (true)
            {
                TcpClient incommingClient = listener.AcceptTcpClient();

                Console.WriteLine("Incomming call from {0} has just been accepted. Processing request...", incommingClient.Client.RemoteEndPoint);

                var task = Task.Factory.StartNew(() => ProcessRequest(incommingClient));
            }
        }

        static void ProcessRequest(TcpClient client)
        {
            using (NetworkStream stream = client.GetStream())
            {
                pocessor.ProcessRequest(stream);
            }
        }
    }
}
