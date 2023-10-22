using Microsoft.AspNetCore.SignalR;
using System.Net.Sockets;

public class ChatHub : Hub
{
	public async Task SendMessage(string user, string message)
	{
		await Clients.All.SendAsync("ReceiveMessage", user, message);
	}

	public async Task Connect(string user)
	{
		await Clients.All.SendAsync("ReceiveMessage", "Serveur", $"{user} s'est connecté.");
	}
}
