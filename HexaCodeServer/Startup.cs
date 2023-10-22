using System.Net;
using System.Net.Sockets;
using System.Text;

namespace MySignalRApp
{
	public class Startup
	{
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddSignalR();
			services.AddControllers();
			services.AddLogging();
			services.AddHostedService<UdpBroadcastBackgroundService>();

		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			app.UseRouting();
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapHub<ChatHub>("/chatHub");
				endpoints.MapDefaultControllerRoute();
			});
		}
	}
	public class UdpBroadcastBackgroundService : BackgroundService
	{
		private readonly ILogger<UdpBroadcastBackgroundService> _logger;

		public UdpBroadcastBackgroundService(ILogger<UdpBroadcastBackgroundService> logger)
		{
			_logger = logger;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				try
				{
					using (UdpClient udpClient = new UdpClient())
					{
						udpClient.EnableBroadcast = true;
						IPEndPoint endPoint = new IPEndPoint(IPAddress.Broadcast, 12345);
						byte[] data = Encoding.UTF8.GetBytes("SignalRServer");
						udpClient.Send(data, data.Length, endPoint);
					}
				}
				catch (Exception ex)
				{
					_logger.LogError($"Error broadcasting message: {ex.Message}");
				}

				// Delay for 5 seconds before the next broadcast
				await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
			}
		}
	}
}
