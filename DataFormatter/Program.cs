using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFormatter
{
    class Program
    {
        private static readonly string[] ThemesToDiscard =
        {
            "Christmas", "Cool", "Dad", "Design", "Food", "Gardening", "Hope", "Mom", "Anniversary", "Diet", "Freedom",
            "Graduation", "Humor", "Marriage", "Medical", "Poetry", "Romantic", "Sad", "Science", "Sports", "War",
            "Wedding", "Architecture", "Art", "Car", "Computers", "Memorialday", "Mothersday", "Movies", "Teen",
            "Thanksgiving", "Valentinesday", "Newyears", "Saintpatricksday", "God"
        };

        private static readonly string[] AutorsToDiscard = { "Adolf Hitler" };

        static void Main(string[] args)
        {
            string projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            string inputFilePath = Path.Combine(projectDirectory, "QuotesDatabaseLite.csv");
            string outputFilePath = Path.Combine(projectDirectory, "QuotesDatabaseLite_output.csv");

            var lines = File.ReadAllLines(inputFilePath);

            lines = Filter(lines);

            File.WriteAllLines(outputFilePath, lines);
        }

        private static string[] Filter(string[] lines)
        {
            List<string> lines_output = new List<string>();

            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                // extract quote text, autor full name, theme name
                string[] quoteAutorTheme = line.Split(';');

                if (quoteAutorTheme.Length != 3 || quoteAutorTheme.Any(string.IsNullOrWhiteSpace)) continue;

                string quoteText = quoteAutorTheme[0].Trim();

                string autorFullName = quoteAutorTheme[1].Trim();

                char[] a = quoteAutorTheme[2].ToCharArray();
                a[0] = char.ToUpper(a[0]);
                string themeName = new string(a).Trim();

                if (!IsToBeDiscarded(autorFullName, themeName))
                {
                    lines_output.Add(line);
                }
            }

            return lines_output.ToArray();
        }

        private static bool IsToBeDiscarded(string autorFullName, string themeName)
        {
            return AutorsToDiscard.Any(x =>
                       String.Equals(x, autorFullName, StringComparison.CurrentCultureIgnoreCase))
                   || ThemesToDiscard.Any(x =>
                       String.Equals(x, themeName, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}
