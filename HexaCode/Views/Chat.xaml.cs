using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;


namespace HexaCode.Views
{
	public sealed partial class Chat : Page
	{
		private HubConnection hubConnection;
		private ObservableCollection<ChatMessage> chatMessages = new ObservableCollection<ChatMessage>();
		private string user;
		private UdpClient _udpClient;

		public Chat()
		{
			InitializeComponent();
		}

		protected override async void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);
			Debug.WriteLine("Navigated to.");
			if (e.Parameter is string user_)
			{
				user = user_;
				send.IsEnabled = false;
				// Utilize 'user' and 'ip' variables here
			}

			// Initialize UdpClient (Ensure it's not initialized multiple times)
			if (_udpClient == null)
			{
				_udpClient = new UdpClient(12345);
				_udpClient.EnableBroadcast = true;

				await Task.Run(async () =>
				{
					while (true)
					{
						try
						{
							var receivedResults = await _udpClient.ReceiveAsync();
							string receivedMessage = Encoding.UTF8.GetString(receivedResults.Buffer);
							string senderIpAddress = receivedResults.RemoteEndPoint.Address.ToString();
							Debug.WriteLine($"Received UDP broadcast: {receivedMessage}");

							if (receivedMessage == "SignalRServer")
							{
								DispatcherQueue.TryEnqueue(() =>
								{
									// Call the asynchronous method inside the synchronous delegate
									_ = InitializeHubConnection(senderIpAddress);
								});
								_udpClient.Close();
								break;
							}
						}
						catch (Exception ex)
						{
							// Handle exceptions, log them, or print them to the console for debugging
							Debug.WriteLine($"Error: {ex.Message}");
						}
					}
				});
			}
		}

		private async Task InitializeHubConnection(string ipAddress)
		{
			try
			{
				hubConnection = new HubConnectionBuilder()
				    .WithUrl($"http://{ipAddress}:5000/chatHub")
				    .Build();

				hubConnection.On<string, string>("ReceiveMessage", (sender, message) =>
				{
					DispatcherQueue.TryEnqueue(() =>
					{
						var receivedMessage = new ChatMessage { User = sender, Message = message };
						chatMessages.Add(receivedMessage);
					});
				});
				progrssring.IsActive = false;
				send.IsEnabled = true;
				await hubConnection.StartAsync();
				await hubConnection.SendAsync("Connect", user);
			}
			catch (Exception ex)
			{
				HandleError(ex.Message);
			}
		}


		private void HandleError(string ex)
		{
			Debug.WriteLine($"Error: {ex}");
			this.Frame.Navigate(typeof(Views.Login), ex);
		}

		private async void OnSendMessageButtonClick(object sender, RoutedEventArgs e)
		{
			var message = MessageTextBox.Text;
			await SendMessageToHubAsync(user, message);
			MessageTextBox.Text = string.Empty;
		}

		private async Task SendMessageToHubAsync(string user, string message)
		{
			try
			{
				await hubConnection.SendAsync("SendMessage", user, message);
			}
			catch (Exception ex)
			{
				HandleError(ex.Message);
			}
		}
	}

	public class ChatMessage
	{
		public string User { get; set; }
		public string Message { get; set; }
	}
}
