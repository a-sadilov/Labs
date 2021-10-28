using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Windows;
using System.ComponentModel;
using System.Windows.Controls;
using System.Threading;
using System.Threading.Tasks;

namespace ProgToInternet_WPF
{
    class Client : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        Loger loger;
        protected void OnPropertyChanged(string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        IPAddress _serverIp;
        int _serverPort = 8000;
        IPEndPoint _serverIpEndPoint;

        Socket _connectionSocket;

        public Client(ListBox logListBox, int serverPort = 8000)
        {
            loger = new Loger(logListBox);

            _serverPort = serverPort;
            _connectionSocket = new Socket(AddressFamily.InterNetwork,
                                               SocketType.Stream,
                                               ProtocolType.Tcp);

            _serverIp = new IPAddress(new byte[] { 0, 0, 0, 0 });

            _serverIpEndPoint = new IPEndPoint(_serverIp, _serverPort);
        }

        public void FindServer()
        {
            if (ServerIp != null)
            {
                string[] ipOctetsStr = ServerIp.Split('.');

                int variableOctet = Convert.ToInt32(ipOctetsStr[3]);

                for (int i = 0; i < 255; i++)
                {
                    string searchIpServerString = String.Format("{0:s}.{1:s}.{2:s}.{3:D}", ipOctetsStr[0], ipOctetsStr[1], ipOctetsStr[2], i);
                    ServerIp = searchIpServerString;

                    if (TryPing(searchIpServerString, 8000, 100))
                    {
                        string logString = String.Format("tried ip {0:s} - FOUND!", searchIpServerString);

                        loger.Print(logString);


                        //break;
                    }
                    else
                    {
                        string logString = String.Format("tried ip {0:s} - fail!", searchIpServerString);
                        loger.Print(logString);
                    }

                }
            }
        }

        public bool TryPing(string strIpAddress, int intPort, int nTimeoutMsec)
        {
            Socket socket = null;
            try
            {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontLinger, false);


                IAsyncResult result = socket.BeginConnect(strIpAddress, intPort, null, null);
                bool success = result.AsyncWaitHandle.WaitOne(nTimeoutMsec, true);

                return socket.Connected;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (null != socket)
                    socket.Close();
            }
        }
        public int ServerPort
        {
            get
            {
                return _serverPort;
            }
        }

        public string ServerIp
        {
            get
            {
                if (_serverIp != null)
                {
                    return _serverIp.ToString();
                }
                else
                {
                    MessageBox.Show("Попытка взятия IP-сервера : IP не задан");
                    return null;
                }
            }
            set
            {
                if (IPAddress.TryParse(value, out _serverIp))
                {
                    _serverIpEndPoint.Address = _serverIp;
                    OnPropertyChanged("server");
                }
                else
                {
                    MessageBox.Show("Входная строка не может преобразоваться в IP");
                }
            }
        }

        public bool ConnectSocket()
        {
            try
            {
                if (!_connectionSocket.Connected)
                {
                    _connectionSocket = new Socket(AddressFamily.InterNetwork,
                                               SocketType.Stream,
                                               ProtocolType.Tcp);

                    //_connectionSocket.ReceiveTimeout = 1;
                    //_connectionSocket.SendTimeout = 500;
                    _connectionSocket.Connect(_serverIpEndPoint);
                    return true;
                }
                else
                {
                    MessageBox.Show("Already connected");
                }

                return false;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message.ToString());
                return false;
            }
            
        }

        public bool SendAndGetResponse(ref byte[] sendBuf, ref byte[] outBuf)
        {
            if (_connectionSocket.Connected)
            {
                _connectionSocket.Send(sendBuf);
                _connectionSocket.Receive(outBuf);
                return true;
            }
            else
            {
                MessageBox.Show("Socket is closed");
                return false;
            }
        }

        public bool CloseSocket()
        {
            if (_connectionSocket.Connected)
            {
                _connectionSocket.Close();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
