using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
namespace HexaCode.Views
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class Login : Page
	{
		private string errmess;
		public Login()
		{
			this.InitializeComponent();
		}

		void connect_Click(object sender, RoutedEventArgs e)
		{
			string pseudo = user.Text;
			this.Frame.Navigate(typeof(Views.Chat), pseudo);
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);
			if (e.Parameter is string mess)
			{
				errmess = mess;
				TextBlock errorMessage = new TextBlock
				{
					Text = mess,
					TextWrapping = TextWrapping.WrapWholeWords
				};

				// Create a Button for the close action inside the Flyout
				Button closeButton = new Button
				{
					Style = (Style)Application.Current.Resources["AccentButtonStyle"],
					Content = "Fermer",
					Margin = new Thickness(0, 8, 0, 0)
				};

				// Create a StackPanel to hold the error message and the close button
				StackPanel stackPanel = new StackPanel();
				stackPanel.Children.Add(errorMessage);
				stackPanel.Children.Add(closeButton);

				// Create a Flyout with the StackPanel as its content
				Flyout flyout = new Flyout
				{
					Content = stackPanel
				};
			}
		}
	}
}
