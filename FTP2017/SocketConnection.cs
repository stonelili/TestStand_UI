using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace FTP2017
{
	public class SocketConnection : IDisposable
	{
		public delegate void DataReceivedHandle(byte[] dataBuffer);

		public byte[] msgBuffer = new byte[8192];

		private Socket _clientSocket = null;

		public Socket ClientSocket => _clientSocket;

		public event DataReceivedHandle DataReceived;

		public SocketConnection(Socket sock)
		{
			_clientSocket = sock;
		}

		public void Connect(IPAddress ip, int port)
		{
			ClientSocket.BeginConnect(ip, port, ConnectCallback, ClientSocket);
		}

		private void ConnectCallback(IAsyncResult ar)
		{
			try
			{
				Socket handler = (Socket)ar.AsyncState;
				handler.EndConnect(ar);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}

		public void Send(string data)
		{
			Send(Encoding.UTF8.GetBytes(data));
		}

		public void Send(byte[] byteData)
		{
			try
			{
				ClientSocket.BeginSend(byteData, 0, byteData.Length, SocketFlags.None, SendCallback, ClientSocket);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}

		private void SendCallback(IAsyncResult ar)
		{
			try
			{
				Socket handler = (Socket)ar.AsyncState;
				handler.EndSend(ar);
			}
			catch (Exception innerException)
			{
				throw new Exception("", innerException);
			}
		}

		public void BeginReceiveData()
		{
			ClientSocket.BeginReceive(msgBuffer, 0, msgBuffer.Length, SocketFlags.None, ReceiveCallback, null);
		}

		private void ReceiveCallback(IAsyncResult ar)
		{
			try
			{
				int REnd = ClientSocket.EndReceive(ar);
				if (REnd > 0)
				{
					byte[] data = new byte[REnd];
					Array.Copy(msgBuffer, 0, data, 0, REnd);
					this.DataReceived(msgBuffer);
					ClientSocket.BeginReceive(msgBuffer, 0, msgBuffer.Length, SocketFlags.None, ReceiveCallback, null);
				}
				else
				{
					Dispose();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}

		public void Dispose()
		{
			try
			{
				ClientSocket.Shutdown(SocketShutdown.Both);
				ClientSocket.Close();
			}
			catch (Exception innerException)
			{
				throw new Exception("", innerException);
			}
		}
	}
}
