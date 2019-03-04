using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using QuoteApp.Backend.BusinessLogic.Subsystem.DataReader;
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

        #region Public API
        
        public Dictionary<int, Quote> Quotes => PersistentProperties.Instance.OnlyUnreadQuotes
            ? _quotes.Where(q => !q.Value.HasBeenRead).ToDictionary(q => q.Key, q => q.Value) 
            : _quotes;
        
        public SortedDictionary<string, Autor> GetAutorsList()
        {
            SortedDictionary<string, Autor> autorList = _autors;

            if (PersistentProperties.Instance.OnlyUnreadQuotes)
                autorList = new SortedDictionary<string, Autor>(autorList
                    .Where(a => !a.Value.HasBeenFullyRead).ToDictionary(x => x.Key, x => x.Value));
            return autorList;
        }

        public SortedDictionary<string, Theme> GetThemesList()
        {
            SortedDictionary<string, Theme> themeList = _themes;

            themeList = new SortedDictionary<string, Theme>(themeList
                .Where(ThemeEvaluationCondition).ToDictionary(x => x.Key, x => x.Value));
            return themeList;
        }

        public Quote GetNextQuoteByAutor(Autor autor)
        {
            var notFullyReadAutor = autor.HasBeenFullyRead ? GetNextAutor(autor) : autor;

            var selectedAutorQuoteAutor = _autorQuoteThemes.First(aqt => 
                aqt.AutorId == notFullyReadAutor.Id && Quotes.ContainsKey(aqt.QuoteId));

            return Quotes[selectedAutorQuoteAutor.QuoteId];
        }

        public Quote GetNextQuoteByTheme(Theme theme)
        {
            var notFullyReadTheme = theme.HasBeenFullyRead ? GetNextTheme(theme) : theme;

            var selectedAutorQuoteTheme = _autorQuoteThemes.First(aqt => 
                aqt.ThemeId == notFullyReadTheme.Id && Quotes.ContainsKey(aqt.QuoteId));

            return Quotes[selectedAutorQuoteTheme.QuoteId];
        }

        public Quote GetRandomQuote()
        {
            Random rnd = new Random();
            int randomQuoteId = rnd.Next(0, Quotes.Count);
            return Quotes[randomQuoteId];
        }

        public Autor GetAutorByQuote(Quote quote)
        {
            var selectedAutorQuoteTheme = _autorQuoteThemes.First(aqt => aqt.QuoteId == quote.Id);
            return _autors.Single(x => x.Value.Id == selectedAutorQuoteTheme.AutorId).Value;
        }

        public Theme GetThemeByQuote(Quote quote)
        {
            var selectedThemeQuoteTheme = _autorQuoteThemes.First(aqt => aqt.QuoteId == quote.Id);
            return _themes.Single(x => x.Value.Id == selectedThemeQuoteTheme.ThemeId).Value;
        }

        public void SetQuoteRead(Quote quote)
        {
            if (quote.HasBeenRead) return;

            var autor = GetAutorByQuote(quote);
            var theme = GetThemeByQuote(quote);

            quote.HasBeenRead = true;
            autor.NumberOfReadQuotes++;
            theme.NumberOfReadQuotes++;

            new Thread(() => 
            {
                Thread.CurrentThread.IsBackground = true; 

                _sqliteDbManager.Update(quote);
                _sqliteDbManager.Update(autor);
                _sqliteDbManager.Update(theme);
            }).Start();
        }
        
        #endregion

        #region Private Helper Methods

        #region Initialization

        private void InitializeDatabase()
        {
            string[] lines = QuoteAppUtils.ReadLocalFile("QuoteApp.Resources.QuotesDatabaseLite.csv").Skip(1).ToArray();
            var csvDataReader = new CsvDataReader(lines);

            _quotes = csvDataReader.Quotes;
            _autors = csvDataReader.Autors;
            _themes = csvDataReader.Themes;
            _autorQuoteThemes = csvDataReader.AutorQuoteThemes;

            PopulateDatabase();
            PersistentProperties.Instance.DatabaseIsInitialized = true;
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
        

        #endregion

        #region Runtime Helpers

        private Autor GetNextAutor(Autor autor)
        {
            SortedDictionary<string, Autor> autorList = GetAutorsList();

            try
            {
                return autorList.First(x => string.Compare(x.Key, autor.FullName, StringComparison.Ordinal) > 0).Value;
            }
            catch
            {
                return autorList.First().Value;
            }
        }

        private Theme GetNextTheme(Theme theme)
        {
            SortedDictionary<string, Theme> themeList = GetThemesList();

            try
            {
                return themeList.First(x => string.Compare(x.Key, theme.Name, StringComparison.Ordinal) > 0).Value;
            }
            catch
            {
                return themeList.First().Value;
            }
        }

        private static bool ThemeEvaluationCondition(KeyValuePair<string, Theme> a)
        {
            return a.Value.NumberOfQuotes > a.Value.NumberOfReadQuotes
                && a.Value.Evaluation >= PersistentProperties.Instance.SelectedThemeRange;
        }

        #endregion

        #endregion
    }
}
