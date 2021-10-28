using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroidesApp_PlusTests_KT3
{
    public class ProtocolDefinition
    {
        public static string Version { get; private set; } = "selfMade_protocol";
        public static Encoding Encoder { get; private set; } = Encoding.ASCII;

        public static byte[] GetBytes(string message)
        {
            byte[] bytesForProtocol = Encoder.GetBytes(Version);

            byte[] bytesForCommand = Encoder.GetBytes(message);
            int length = bytesForCommand.Length;
            byte[] bytesForLength = BitConverter.GetBytes(length);

            int messageLength = bytesForProtocol.Length + bytesForLength.Length + bytesForCommand.Length;
            byte[] buffer = new byte[messageLength];
            Array.Copy(bytesForProtocol, 0, buffer, 0, bytesForProtocol.Length);
            Array.Copy(bytesForLength, 0, buffer, bytesForProtocol.Length, bytesForLength.Length);
            Array.Copy(bytesForCommand, 0, buffer, bytesForProtocol.Length + bytesForLength.Length, bytesForCommand.Length);
            return buffer;
        }
    }
}
