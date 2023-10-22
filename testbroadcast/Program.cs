using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class Program
{
	private static UdpClient _udpClient;

	static void Main(string[] args)
	{
		_udpClient = new UdpClient(12345); // Specify the port number to listen on
		_udpClient.EnableBroadcast = true;

		Console.WriteLine("Listening for broadcasted messages...");

		while (true)
		{
			IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
			byte[] data = _udpClient.Receive(ref remoteEndPoint);
			string message = Encoding.UTF8.GetString(data);

			Console.WriteLine($"Received message: {message}");
		}
	}
}