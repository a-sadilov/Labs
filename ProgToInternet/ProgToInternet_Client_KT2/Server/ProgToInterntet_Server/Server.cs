using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace ProgToInterntet_Server
{
    class Server
    {

        static IPAddress serverIp = new IPAddress(new byte[] { 192, 168, 0, 106});
        static int serverPort = 8000;

        static IPEndPoint serverIpEndPoint = new IPEndPoint(serverIp, serverPort);

        static Socket listenSocket = new Socket(AddressFamily.InterNetwork,
                                                SocketType.Stream, 
                                                ProtocolType.Tcp);

        static int responseCounter = 0;
        static void Main(string[] args)
        {
            
            listenSocket.Bind(serverIpEndPoint);
			listenSocket.Listen(100000);

			while (true)
            {
				//
				Socket connectedSocket = listenSocket.Accept();

                int bytes = 0;
                byte[] buffer = new byte[1024];

                bytes = connectedSocket.Receive(buffer);

                Console.WriteLine(Encoding.UTF8.GetString(buffer, 0, bytes));
                buffer = Encoding.UTF8.GetBytes("Запрос обработан №" + (responseCounter+1).ToString());

                connectedSocket.Send(buffer);
                connectedSocket.Shutdown(SocketShutdown.Both);
                connectedSocket.Close();

            }

        }


		/*public class ConsoleListener
		{

			internal static IPAddress IP { get; set; }
			internal static int serverPort { get; set; } = 11011;
			internal string HostName { get; set; } = Dns.GetHostName();
			internal IPAddress localIPAddr { get; set; } = (Dns.GetHostEntry(Dns.GetHostName())).AddressList[1];
			internal IPEndPoint serverIpEndPoint = new IPEndPoint(IP, serverPort);

			static Socket listenSocket = new Socket(AddressFamily.InterNetwork,
													SocketType.Stream,
													ProtocolType.Tcp);
			public ConsoleListener()
			{

			}
			public ConsoleListener(int port)
			{
				serverPort = port;
			}

			

			public void StartMulticast(string _mcastAddress)
			{
				try
				{
					this.mcastAddress = IPAddress.Parse(_mcastAddress);
					this.mcastSocket = new Socket(AddressFamily.InterNetwork,
											 SocketType.Dgram,
											 ProtocolType.Udp);

					EndPoint localEP = (EndPoint)new IPEndPoint(this.localIPAddr, mcastPort);
					this.mcastSocket.EnableBroadcast = true;
					this.mcastSocket.Bind(localEP);
					MulticastOption mcastOption;

					mcastOption = new MulticastOption(this.mcastAddress, this.localIPAddr);

					mcastSocket.SetSocketOption(SocketOptionLevel.IP,
												SocketOptionName.AddMembership,
												mcastOption);
				}
				catch (Exception e)
				{
					Console.WriteLine(e.Message);
				}
			}
			//naher 
			public void ReceiveBroadcastMessages()
			{
				byte[] bytes = new Byte[100];
				IPEndPoint groupEP = new IPEndPoint(this.mcastAddress, mcastPort);
				EndPoint remoteEP = (EndPoint)new IPEndPoint(IPAddress.Any, 0);

				try
				{
					Console.WriteLine("Waiting for multicast packets.......");
					Console.WriteLine("Enter ^C to terminate.");

					mcastSocket.ReceiveFrom(bytes, ref remoteEP);

					Console.WriteLine("Server: Received broadcast from {0} :\n {1}\n",
						groupEP.ToString(),
						Encoding.ASCII.GetString(bytes, 0, bytes.Length));
					mcastSocket.Close();
				}
				catch (Exception e)
				{
					Console.WriteLine(e.Message);
				}
			}

			public async void ListenAsync()
			{
				await Task.Run(() => Listen());
			}
			public void Listen()
			{
				try
				{
					byte[] bytes = new Byte[100];
					IPEndPoint groupEP = new IPEndPoint(mcastAddress, mcastPort);
					EndPoint remoteEP = (EndPoint)new IPEndPoint(IPAddress.Any, 0);
					Console.WriteLine("Сервер хоста {0} запущен. IP:{1}:{2} " +
						"Ожидание сообщений...", HostName, localIPAddr, mcastPort);

					this.mcastSocket.ReceiveFrom(bytes, ref remoteEP);

					Console.WriteLine("Received broadcast-message from {0} :\nClient says:  {1}\n",
						groupEP.ToString(),
						Encoding.ASCII.GetString(bytes, 0, bytes.Length));
					listenSocket.Close();
				}
				catch (Exception e)
				{
					Console.WriteLine(e.Message);
				}
			}
			// закрытие сокета
			private void Close()
			{
				if (mcastSocket != null)
				{
					mcastSocket.Close();
					mcastSocket = null;
				}
			}

			// отключение всех клиентов
			public static void DisconnectAllListeners()
			{
				//остановка сервера

				for (int i = 0; i < servers.Count; i++)
				{
					servers[i].Close(); //отключение клиента
					servers[i] = null;
				}
				Environment.Exit(0); //завершение процесса
			}
		}*/
	}

}
