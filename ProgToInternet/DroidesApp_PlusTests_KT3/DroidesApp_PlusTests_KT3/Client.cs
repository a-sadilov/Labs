using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace DroidesApp_PlusTests_KT3
{
    class Client
    {
        private const int port = 8082;
        private const string server = "192.168.0.106";

        public void TranslateCommand()
        {
            
            try
            {
                string command = GetCommand();
                using (NetworkStream stream = EstablishNewConnectionStream())
                {
                    SendCommand(stream, command);
                    string response = ReceiveResponse(stream);
                    ShowResponse(response);
                }
            }
                
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e.Message);
            }
        }

        private static void SendCommand(NetworkStream stream, string command)
        {
            byte[] buffer = ProtocolDefinition.GetBytes(command);
            stream.Write(buffer, 0, buffer.Length);
        }

        private static string ReceiveResponse(NetworkStream stream)
        {
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
            return response;
        }

        private static void ShowResponse(string response)
        {
            Console.WriteLine("Response received:");
            Console.WriteLine(response);
        }

        private static NetworkStream EstablishNewConnectionStream()
        {
            TcpClient client = new TcpClient();
            client.Connect(server, port);
            NetworkStream stream = client.GetStream();
            return stream;
        }

        private static string GetCommand()
        {
            Console.WriteLine("Insert a command for sending to the server:");
            string command = Console.ReadLine();
            return command;
        }
    }
}
