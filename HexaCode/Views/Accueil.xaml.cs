using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace HexaCode.Views
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class Accueil : Page
	{
		public Accueil()
		{
			this.InitializeComponent();

		}

		private void codbut_Click(object sender, RoutedEventArgs e)
		{
			var window = (Application.Current as App)?.m_window as MainWindow;
			this.Frame.Navigate(typeof(Views.Coder));
			window.nvSample.SelectedItem = window.cod;
		}

		private void decbut_Click(object sender, RoutedEventArgs e)
		{
			var window = (Application.Current as App)?.m_window as MainWindow;
			this.Frame.Navigate(typeof(Views.Decoder));
			window.nvSample.SelectedItem = window.dec;
		}
	}
}
