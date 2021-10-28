using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace FunwithSoketsApp_KT_1
{
	public class ConsoleListener
	{

		internal IPAddress mcastAddress { get; set; }
		internal int mcastPort  = 11011;
		internal string HostName { get; set; } = Dns.GetHostName();
		internal IPAddress localIPAddr { get; set; } = (Dns.GetHostEntry(Dns.GetHostName())).AddressList[1];
		internal static List<ConsoleListener> servers = new List<ConsoleListener>();
		private Socket mcastSocket;
		public ConsoleListener() 
		{

		}
		public ConsoleListener(int port)
		{
			mcastPort = port;
		}

		public void AddListener()
		{
			servers.Add(this);
		}

		internal void RemoveListener()
		{
			// и удаляем его из списка подключений
			if (this != null)
				servers.Remove(this);
				this.mcastSocket.Close();

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
		public void  Listen()
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
				this.mcastSocket.Close();
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
	}
}