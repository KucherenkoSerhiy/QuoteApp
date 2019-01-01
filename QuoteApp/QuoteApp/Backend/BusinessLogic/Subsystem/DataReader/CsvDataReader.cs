using System;
using System.Collections.Generic;
using System.Linq;
using QuoteApp.Backend.Model;
using QuoteApp.Globals;

namespace QuoteApp.Backend.BusinessLogic.Subsystem.DataReader
{
    public class CsvDataReader
    {
        

        public Dictionary<int, Quote> Quotes { get; set; }
        public SortedDictionary<string, Autor> Autors { get; set; }
        public SortedDictionary<string, Theme> Themes { get; set; }
        public List<AutorQuoteTheme> AutorQuoteThemes { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lines">.csv file content splitted by lines</param>
        public CsvDataReader(string[] lines)
        {
            Quotes = new Dictionary<int, Quote>();
            Autors = new SortedDictionary<string, Autor>();
            Themes = new SortedDictionary<string, Theme>();
            AutorQuoteThemes = new List<AutorQuoteTheme>();

            ExtractData(lines);
        }
        
        //TODO: theme colors
        private void ExtractData(string[] lines)
        {
            int autorCount = 0, quoteCount = 0, themeCount = 0;

            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                // extract quote text, autor full name, theme name
                string[] quoteAutorTheme = line.Split(';');

                string quoteText = quoteAutorTheme[0].Trim();

                string autorFullName = quoteAutorTheme[1].Trim();
                if (QuoteAppConstants.AutorsToRename.ContainsKey(autorFullName))
                    autorFullName = QuoteAppConstants.ThemesToRename[autorFullName];

                char[] a = quoteAutorTheme[2].ToCharArray();
                a[0] = char.ToUpper(a[0]);
                string themeName = new string(a).Trim();
                if (QuoteAppConstants.ThemesToRename.ContainsKey(themeName))
                    themeName = QuoteAppConstants.ThemesToRename[themeName];

                if (IsToBeDiscarded(autorFullName, themeName)) continue;

                int quoteId = AddQuoteAndGetId(ref quoteCount, quoteText);
                var autorId = AddAutorAndGetId(ref autorCount, autorFullName);
                int themeId = AddThemeAndGetId(ref themeCount, themeName);
                AddLink(quoteId, autorId, themeId);
            }
        }

        private bool IsToBeDiscarded(string autorFullName, string themeName)
        {
            return QuoteAppConstants.AutorsToDiscard.Any(x => 
                       String.Equals(x, autorFullName, StringComparison.CurrentCultureIgnoreCase)) 
                   || QuoteAppConstants.ThemesToDiscard.Any(x => 
                       String.Equals(x, themeName, StringComparison.CurrentCultureIgnoreCase));
        }

        /// <summary>
        /// Adds quote and gets its id
        /// </summary>
        /// <param name="quoteCount"></param>
        /// <param name="quoteText"></param>
        /// <returns></returns>
        private int AddQuoteAndGetId(ref int quoteCount, string quoteText)
        {
            var quote = new Quote { Id = quoteCount, Text = quoteText };
            Quotes.Add(quoteCount, quote);
            int quoteId = quoteCount;
            quoteCount++;
            return quoteId;
        }

        /// <summary>
        /// Adds autor if not exists and gets his/her id (or finds id of existent one)
        /// </summary>
        /// <param name="autorCount"></param>
        /// <param name="autorFullName"></param>
        /// <returns></returns>
        private int AddAutorAndGetId(ref int autorCount, string autorFullName)
        {
            int autorId;
            if (!Autors.ContainsKey(autorFullName))
            {
                var autor = new Autor { Id = autorCount, FullName = autorFullName };
                autor.NumberOfQuotes++;

                Autors.Add(autorFullName, autor);
                autorId = autorCount;
                autorCount++;
            }
            else
            {
                var autor = Autors[autorFullName];
                autor.NumberOfQuotes++;
                autorId = autor.Id;
            }

            return autorId;
        }

        /// <summary>
        /// Adds theme if not exists and gets its id (or finds id of existent one)
        /// </summary>
        /// <param name="themeCount"></param>
        /// <param name="themeName"></param>
        /// <returns></returns>
        private int AddThemeAndGetId(ref int themeCount, string themeName)
        {
            int themeId;
            if (!Themes.ContainsKey(themeName))
            {
                var evaluation = QuoteAppConstants.ThemeEvaluations.ContainsKey(themeName)
                    ? QuoteAppConstants.ThemeEvaluations[themeName]
                    : EnEvaluation.Extension;

                var theme = new Theme
                {
                    Id = themeCount,
                    Name = themeName,
                    Evaluation = evaluation,
                    DayLineColor = QuoteAppConstants.DefaultDayLineColor,
                    DayTextColor = QuoteAppConstants.DefaultDayTextColor,
                    NightLineColor = QuoteAppConstants.DefaultNightLineColor,
                    NightTextColor = QuoteAppConstants.DefaultNightTextColor
                };
                theme.NumberOfQuotes++;

                Themes.Add(themeName, theme);

                themeId = themeCount;
                themeCount++;
            }
            else
            {
                var theme = Themes[themeName];
                theme.NumberOfQuotes++;
                themeId = theme.Id;
            }

            return themeId;
        }

        private void AddLink(int quoteId, int autorId, int themeId)
        {
            AutorQuoteThemes.Add(new AutorQuoteTheme(autorId, quoteId, themeId));
        }
    }
}
