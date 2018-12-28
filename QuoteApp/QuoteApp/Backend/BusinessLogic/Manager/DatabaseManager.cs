using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using QuoteApp.Backend.BusinessLogic.Subsystem.PersistentProperties;
using QuoteApp.Backend.Model;
using QuoteApp.Globals;

namespace QuoteApp.Backend.BusinessLogic.Manager
{
    internal class DatabaseManager
    {
        #region Singleton
        private static DatabaseManager _instance;

        public static DatabaseManager Instance => _instance ?? (_instance = new DatabaseManager());

        private DatabaseManager()
        {
            _sqliteDbManager = SqliteDbManager.Instance;

            if (PersistentProperties.Instance.DatabaseIsInitialized)
            {
                _quotes = _sqliteDbManager.GetList<Quote>().ToDictionary(x => x.Id, x => x);
                _autors = new SortedDictionary<string, Autor>(_sqliteDbManager.GetList<Autor>().ToDictionary(x => x.FullName, x => x));
                _themes = new SortedDictionary<string, Theme>(_sqliteDbManager.GetList<Theme>().ToDictionary(x => x.Name, x => x));
                _autorQuoteThemes = _sqliteDbManager.GetList<AutorQuoteTheme>();
                //Todo: theme colors
            }
            else
            {
                InitializeDatabase();
            }
        }

        #endregion

        private readonly SqliteDbManager _sqliteDbManager;
        private Dictionary<int, Quote> _quotes;
        private SortedDictionary<string, Autor> _autors;
        private SortedDictionary<string, Theme> _themes;
        private List<AutorQuoteTheme> _autorQuoteThemes;

        public Dictionary<int, Quote> Quotes => PersistentProperties.Instance.OnlyUnreadQuotes
            ? _quotes.Where(q => !q.Value.HasBeenFullyRead).ToDictionary(q => q.Key, q => q.Value) 
            : _quotes;

        public SortedDictionary<string, Autor> Autors => PersistentProperties.Instance.OnlyUnreadQuotes
            ? new SortedDictionary<string, Autor>(_autors
                .Where(a => !a.Value.HasBeenFullyRead)
                .ToDictionary(x => x.Key, x => x.Value))
            : _autors;

        public SortedDictionary<string, Theme> Themes
        {
            get => PersistentProperties.Instance.OnlyUnreadQuotes
                ? new SortedDictionary<string, Theme>(_themes
                    .Where(a => a.Value.NumberOfQuotes > a.Value.NumberOfReadQuotes)
                    .ToDictionary(x => x.Key, x => x.Value))
                : _themes;
            set => _themes = value;
        }

        private void InitializeDatabase()
        {
            string[] lines = QuoteAppUtils.ReadLocalFile("QuoteApp.Resources.QuotesDatabaseLite.csv").Skip(2).ToArray();

            _quotes = new Dictionary<int, Quote>();
            _autors = new SortedDictionary<string, Autor>();
            _themes = new SortedDictionary<string, Theme>();
            _autorQuoteThemes = new List<AutorQuoteTheme>();

            Stopwatch watch = Stopwatch.StartNew();

            ExtractData(lines);
            PopulateDatabase();

            watch.Stop();

            string watchTime = $"Elapsed: {watch.ElapsedMilliseconds / 1000}";

            string summary = $"Total: Quotes={_quotes.Count}, Autors={_autors.Count}, Themes={_themes.Count}, Links={_autorQuoteThemes.Count}";

            PersistentProperties.Instance.DatabaseIsInitialized = true;
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

                char[] a = quoteAutorTheme[2].ToCharArray();
                a[0] = char.ToUpper(a[0]);
                string themeName = new string(a).Trim();

                // add quote and get its id
                var quote = new Quote { Id = quoteCount, Text = quoteText };
                _quotes.Add(quoteCount, quote);
                int quoteId = quoteCount;
                quoteCount++;

                // add autor if not exists and get his/her id (or find id of existent one)
                int autorId;
                if (!_autors.ContainsKey(autorFullName))
                {
                    var autor = new Autor {Id = autorCount, FullName = autorFullName};
                    autor.NumberOfQuotes++;
                    if (quote.HasBeenFullyRead) autor.NumberOfReadQuotes++;

                    _autors.Add(autorFullName, autor);
                    autorId = autorCount;
                    autorCount++;
                }
                else
                {
                    var autor = _autors[autorFullName];
                    autor.NumberOfQuotes++;
                    if (quote.HasBeenFullyRead) autor.NumberOfReadQuotes++;
                    autorId = autor.Id;
                }

                // add theme if not exists and get its id (or find id of existent one)
                int themeId;
                if (!_themes.ContainsKey(themeName))
                {
                    var theme = new Theme
                    {
                        Id = themeCount,
                        Name = themeName,
                        DayLineColor = QuoteAppConstants.DefaultDayLineColor,
                        DayTextColor = QuoteAppConstants.DefaultDayTextColor,
                        NightLineColor = QuoteAppConstants.DefaultNightLineColor,
                        NightTextColor = QuoteAppConstants.DefaultNightTextColor
                    };
                    theme.NumberOfQuotes++;
                    if (quote.HasBeenFullyRead) theme.NumberOfReadQuotes++;

                    _themes.Add(themeName, theme);

                    themeId = themeCount;
                    themeCount++;
                }
                else
                {
                    var theme = _themes[themeName];
                    theme.NumberOfQuotes++;
                    if (quote.HasBeenFullyRead) theme.NumberOfReadQuotes++;
                    themeId = theme.Id;
                }

                // add link
                _autorQuoteThemes.Add(new AutorQuoteTheme(autorId, quoteId, themeId));
            }
        }

        private void PopulateDatabase()
        {
            CreateTables();
            InsertLists();
        }

        private void CreateTables()
        {
            _sqliteDbManager.CreateTable<Quote>();
            _sqliteDbManager.CreateTable<Autor>();
            _sqliteDbManager.CreateTable<Theme>();
            _sqliteDbManager.CreateTable<ThemeColor>();
            _sqliteDbManager.CreateTable<AutorQuoteTheme>();
        }

        private void InsertLists()
        {
            _sqliteDbManager.InsertList(_quotes.Values);
            _sqliteDbManager.InsertList(_autors.Values);
            _sqliteDbManager.InsertList(_themes.Values);
            _sqliteDbManager.InsertList(_autorQuoteThemes);
        }

        public Autor GetNextAutor(Autor autor)
        {
            try
            {
                return Autors.First(x => string.Compare(x.Key, autor.FullName, StringComparison.Ordinal) > 0).Value;
            }
            catch
            {
                return Autors.First().Value;
            }
        }

        public Theme GetNextTheme(Theme theme)
        {
            try
            {
                return Themes.First(x => string.Compare(x.Key, theme.Name, StringComparison.Ordinal) > 0).Value;
            }
            catch
            {
                return Themes.First().Value;
            }
        }

        public Quote GetRandomQuote()
        {
            Random rnd = new Random();
            int randomQuoteId = rnd.Next(0, Quotes.Count);
            return Quotes[randomQuoteId];
        }

        
        public IEnumerable<Quote> GetQuotesByAutor(Autor autor)
        {
            var selectedAutorQuoteThemes = _autorQuoteThemes.Where(aqt => aqt.AutorId == autor.Id);

            return selectedAutorQuoteThemes.Select(selectedAutorQuoteTheme => Quotes[selectedAutorQuoteTheme.QuoteId]);
        }
        
        public IEnumerable<Quote> GetQuotesByTheme(Theme theme)
        {
            var selectedAutorQuoteThemes = _autorQuoteThemes.Where(aqt => aqt.AutorId == theme.Id);

            return selectedAutorQuoteThemes.Select(selectedAutorQuoteTheme => Quotes[selectedAutorQuoteTheme.QuoteId]);
        }

        public Theme GetThemeByAutor(Autor autor)
        {
            var selectedAutorQuoteThemes = _autorQuoteThemes.Where(aqt => aqt.AutorId == autor.Id);

            return Themes.Single(x => x.Value.Id == selectedAutorQuoteThemes.First().ThemeId).Value;
        }

        public Autor GetAutorByTheme(Theme theme)
        {
            var selectedAutorQuoteThemes = _autorQuoteThemes.Where(aqt => aqt.AutorId == theme.Id);

            return Autors.Single(x => x.Value.Id == selectedAutorQuoteThemes.First().AutorId).Value;
        }

        public Autor GetAutorByQuote(Quote quote)
        {
            var selectedAutorQuoteTheme = _autorQuoteThemes.First(aqt => aqt.QuoteId == quote.Id);
            return Autors.Single(x => x.Value.Id == selectedAutorQuoteTheme.AutorId).Value;
        }

        public Theme GetThemeByQuote(Quote quote)
        {
            var selectedThemeQuoteTheme = _autorQuoteThemes.First(aqt => aqt.QuoteId == quote.Id);
            return Themes.Single(x => x.Value.Id == selectedThemeQuoteTheme.ThemeId).Value;
        }

        public void SetQuoteRead(Quote quote)
        {
            if (quote.HasBeenFullyRead) return;

            var autor = GetAutorByQuote(quote);
            var theme = GetThemeByQuote(quote);

            quote.HasBeenFullyRead = true;
            autor.NumberOfReadQuotes++;
            theme.NumberOfReadQuotes++;
        }
    }
}
