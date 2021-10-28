using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TCP_App
{
    public class RequestProcessor
    {
        internal void ProcessRequest(NetworkStream stream)
        {
            byte[] bytesForProtocol = new byte[17];
            stream.Read(bytesForProtocol, 0, 17);
            string protocol = Encoding.ASCII.GetString(bytesForProtocol);

            if (protocol != "selfMade_protocol")
            {
                Console.WriteLine("Unknown protocol");
                SendResponse("Unknown protocol", stream);
                return;
            }

            byte[] bytesForLength = new byte[5];
            stream.Read(bytesForLength, 0, 5);
            int length = BitConverter.ToInt32(bytesForLength, 0);

            byte[] bytesForCommand = new byte[length];
            stream.Read(bytesForCommand, 0, length);
            string message = Encoding.ASCII.GetString(bytesForCommand);

            string[] partsOfCommand = message.Split('#');
            
                

            
            string command = partsOfCommand[0];

            switch (command)
            {
                case ("create"): Console.WriteLine("Droid has been created"); break;
                case ("count"): Console.WriteLine("Nothing to be counted"); break;
                default: Console.WriteLine("default action executed"); break;
            }

            string reponse = string.Format("Command \"{0}\" has been successfully processed", command);
            SendResponse(reponse, stream);
        }

        private void SendResponse(string response, NetworkStream stream)
        {
            byte[] buffer = KsitProtocolDefinition.GetBytes(response);
            stream.Write(buffer, 0, buffer.Length);
        }
    }
}
