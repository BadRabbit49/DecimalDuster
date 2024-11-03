using System.Globalization;
using System.Text.RegularExpressions;

namespace DecimalDuster {
	internal class Program {
		/**The purpose of this program is to take files with long floating points and rounds them up to a certain decimal place.**/
		/**Helpful for when you have large files for games or whatnot with thousands of lines with numbers that have collected "decimal dust" from floating points.**/
		/**Said decimal dust is entirely harmless, however for OCD freaks like myself this can be incredibly annoying to deal.**/

		static void Main(string[] args) {
			bool notSure = false;
			string repeats = "";
			while (true) {
				if (notSure) {
					Console.WriteLine("Sorry, I didn't understand you there, do you want to continue? Yes or no?");
				} else {
					UserInput();
					Console.WriteLine("Do you wish to continue or quit?");
				}
				repeats = Console.ReadLine()?.ToLowerInvariant() ?? "no";
				running = positives.Contains(repeats);
				if (positives.Contains(repeats)) {
					Console.WriteLine("Okay, let's go again!");
					continue;
				}
				if (negatives.Contains(repeats)) {
					Console.WriteLine("Okay, Bye!");
					return;
				}
				notSure = true;
			}
		}

		public static void UserInput() {
			string path;
			string name;
			int decimalPlacesTo;
			StreamReader reader;
			StreamWriter writer;

			Console.WriteLine("Specify the path to the file you want to round numbers in:");

			while (true) {
				if (File.Exists(path = Console.ReadLine() ?? "")) {
					Console.Clear();
					reader = File.OpenText(path);
					string[] full = path.Split('\\');
					name = full.Last().Insert(0, "rounded_");
					Console.WriteLine("How many decimals do you want to round to?");
					if (!int.TryParse(Console.ReadLine(), out decimalPlacesTo)) {
						decimalPlacesTo = 4;
					}
					break;
				}
				Console.WriteLine($"The provided file path for {path} cannot be found.\nTry dragging and dropping the file into the console if possible.");
			}

			string[] old_lines = File.ReadAllLines(path).ToArray();
			string[] new_lines = new string[old_lines.Length];

			// Checking through each line.
			for (int i = 0; i < old_lines.Length; i++) {
				new_lines[i] = DustOffDecimals(old_lines[i], decimalPlacesTo);
			}
			reader.Close();
			writer = File.CreateText(name);
			for (int l = 0; l < new_lines.Length; l++) {
				writer.WriteLine(new_lines[l]);
			}
			writer.Close();
		}

		public static string DustOffDecimals(string input, int decimalPlaces) {
			// Regular expression to find floating-point numbers.
			string pattern = @"-?\d+\.\d+";
			// Use Regex.Replace to replace each match in the input string.
			string result = Regex.Replace(input, pattern, match => {
				// Parse the matched number
				double number = double.Parse(match.Value, CultureInfo.InvariantCulture);
				// Round the number to the specified decimal places.
				double roundedNumber = Math.Round(number, decimalPlaces, MidpointRounding.AwayFromZero);
				// Format the rounded number back to a string with fixed decimal places.
				return roundedNumber.ToString($"F{decimalPlaces}", CultureInfo.InvariantCulture);
			});
			return result;
		}

		static bool running = true;
		static string[] positives = new string[] { "yes", "sure", "y", "go ahead", "again", "continue", "1", "da", "affirmative", "im gay" };
		static string[] negatives = new string[] { "no", "nope", "n", "not really", "no more", "this sucks", "yeet", "fuck off", "bye" };
	}
}