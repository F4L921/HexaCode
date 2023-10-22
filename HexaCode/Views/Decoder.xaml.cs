using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace HexaCode.Views
{
	public sealed partial class Decoder : Page
	{
		public Decoder()
		{
			this.InitializeComponent();
		}

		private void decrypt_Click(object sender, RoutedEventArgs e)
		{
			string algo = @"
{
    ""@#$"": ""a"",
    ""*&%"": ""b"",
    ""!?+"": ""c"",
    ""=~_"": ""d"",
    ""$@*"": ""e"",
    ""&!#"": ""f"",
    ""%_="": ""g"",
    ""+~$"": ""h"",
    ""_=!"": ""i"",
    ""#*&"": ""j"",
    ""~%+"": ""k"",
    ""?!_"": ""l"",
    ""*=@"": ""m"",
    ""%$#"": ""n"",
    ""_~!"": ""o"",
    ""+&?"": ""p"",
    ""!#*"": ""q"",
    ""@=~"": ""r"",
    ""$%_"": ""s"",
    ""&_~"": ""t"",
    ""~#+"": ""u"",
    ""?*$"": ""v"",
    ""=!@"": ""w"",
    ""*#%"": ""x"",
    ""+$_"": ""y"",
    ""#~&"": ""z"",
    ""   "": "" ""
}";
			Dictionary<string, string> algodico = JsonConvert.DeserializeObject<Dictionary<string, string>>(algo);
			string outputString = ConvertToLettersFromSymbolCombinations(inp.Text, algodico);

			// Update the input field with the decoded message
			inp.Text = outputString;
		}

		private string ConvertToLettersFromSymbolCombinations(string input, Dictionary<string, string> algodico)
		{
			string output = "";
			int index = 0;

			while (index < input.Length)
			{
				// Take three characters from the input
				string currentSymbols = input.Substring(index, Math.Min(3, input.Length - index));

				// Check if the current symbols exist in the dictionary
				if (algodico.TryGetValue(currentSymbols, out string letters))
				{
					output += letters;
				}
				else
				{
					// If symbols are not found, append them as is
					HandleError();
					return input;
				}

				// Move to the next group of three characters
				index += 3;
			}

			return output;
		}

		private void HandleError()
		{
			// Create a TextBlock with the error message
			TextBlock errorMessage = new TextBlock
			{
				Text = "Le texte que tu as entré n'est pas un code valide.",
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