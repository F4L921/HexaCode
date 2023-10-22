using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace HexaCode.Views
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class Coder : Page
	{
		public Coder()
		{
			this.InitializeComponent();
		}

		private void crypt_Click(object sender, RoutedEventArgs e)
		{
			string algo = @"
{
    ""a"": ""@#$"",
    ""b"": ""*&%"",
    ""c"": ""!?+"",
    ""d"": ""=~_"",
    ""e"": ""$@*"",
    ""f"": ""&!#"",
    ""g"": ""%_="",
    ""h"": ""+~$"",
    ""i"": ""_=!"",
    ""j"": ""#*&"",
    ""k"": ""~%+"",
    ""l"": ""?!_"",
    ""m"": ""*=@"",
    ""n"": ""%$#"",
    ""o"": ""_~!"",
    ""p"": ""+&?"",
    ""q"": ""!#*"",
    ""r"": ""@=~"",
    ""s"": ""$%_"",
    ""t"": ""&_~"",
    ""u"": ""~#+"",
    ""v"": ""?*$"",
    ""w"": ""=!@"",
    ""x"": ""*#%"",
    ""y"": ""+$_"",
    ""z"": ""#~&"",
    "" "": ""   ""
}";
			Dictionary<string, string> algodico = JsonConvert.DeserializeObject<Dictionary<string, string>>(algo);
			string outputString = ConvertToSymbolCombinations(inp.Text, algodico);
			string ConvertToSymbolCombinations(string input, Dictionary<string, string> symbolDictionary)
			{
				string output = "";
				foreach (char letter in input.ToLower()) // Convert input to lowercase for case-insensitive matching
				{
					if (Char.IsLetter(letter) || Char.IsWhiteSpace(letter))
					{
						string letterAsString = letter.ToString();
						output += algodico[letterAsString];
					}
					else
					{
						HandleError();
						return input;
					}
				}
				return output;
			}
			inp.Text = outputString;
		}
		private void HandleError()
		{
			// Create a TextBlock with the error message
			TextBlock errorMessage = new TextBlock
			{
				Text = "Le texte que tu as entré doit être une lettre ou un espace.",
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

			// Set the Flyout's placement target and show it
			flyout.ShowAt(inp);

			// Close the Flyout when the close button is clicked
			closeButton.Click += (sender, args) => flyout.Hide();
		}
	}
}
