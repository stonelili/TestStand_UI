using System.Net;
using System.Net.Sockets;

namespace FTP2017
{
	public class ClientSession
	{
		public string IP;

		public Socket ClientSocket { get; set; }

		public ClientSession(Socket clientSocket)
		{
			ClientSocket = clientSocket;
			IP = GetIPStr();
		}

		public string GetIPStr()
		{
			return ((IPEndPoint)ClientSocket.RemoteEndPoint).Address.ToString();
		}
	}
}
