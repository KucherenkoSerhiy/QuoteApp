using System;
using System.Collections.Generic;
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
                Quotes = _sqliteDbManager.GetList<Quote>();
                Autors = _sqliteDbManager.GetList<Autor>(); // Todo: suspiciously many autors
                Themes = _sqliteDbManager.GetList<Theme>();
                AutorQuoteThemes = _sqliteDbManager.GetList<AutorQuoteTheme>();
                //Todo: theme colors
            }
            else
            {
                InitializeDatabase();
            }

            CreateLinks();
        }

        #endregion

        private SqliteDbManager _sqliteDbManager;

        public List<Autor> Autors { get; set; }
        public List<Quote> Quotes { get; set; }
        public List<Theme> Themes { get; set; }
        public List<AutorQuoteTheme> AutorQuoteThemes { get; set; }
        
        private void InitializeDatabase()
        {
            string[] lines = QuoteAppUtils.ReadLocalFile("QuoteApp.Resources.QuotesDatabaseLite.csv").Skip(2).ToArray();

            Autors = new List<Autor>();
            Quotes = new List<Quote>();
            Themes = new List<Theme>();
            AutorQuoteThemes = new List<AutorQuoteTheme>();

            Stopwatch watch = Stopwatch.StartNew();

            ExtractData(lines);
            PopulateDatabase();

            watch.Stop();

            string watchTime = $"Elapsed: {watch.ElapsedMilliseconds / 1000}";

            string summary = $"Total: Quotes={Quotes.Count}, Autors={Autors.Count}, Themes={Themes.Count}, Links={AutorQuoteThemes.Count}";

            PersistentProperties.Instance.DatabaseIsInitialized = true;
        }

        private void CreateLinks()
        {
            foreach (var autorQuoteTheme in AutorQuoteThemes)
            {
                Autor autor = Autors.Single(x => x.Id == autorQuoteTheme.AutorId);
                Quote quote = Quotes.Single(x => x.Id == autorQuoteTheme.QuoteId);
                Theme theme = Themes.Single(x => x.Id == autorQuoteTheme.ThemeId);

                autor.Quotes.Add(quote);
                autor.Themes.Add(theme);

                quote.Autors.Add(autor);
                quote.Themes.Add(theme);

                theme.Autors.Add(autor);
                theme.Quotes.Add(quote);
            }
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
                Quotes.Add(new Quote { Id = quoteCount, Text = quoteText });
                int quoteId = quoteCount;
                quoteCount++;

                // add autor if not exists and get his/her id (or find id of existent one)
                int autorId;
                if (!Autors.Any(x => x.FullName == autorFullName))
                {
                    Autors.Add(new Autor { Id = autorCount, FullName = autorFullName });
                    autorId = autorCount;
                    autorCount++;
                }
                else
                {
                    autorId = Autors.First(x => x.FullName == autorFullName).Id;
                }

                // add theme if not exists and get its id (or find id of existent one)
                int themeId;
                if (!Themes.Any(x => x.Name == themeName))
                {
                    Themes.Add(new Theme { Id = themeCount, Name = themeName });
                    themeId = themeCount;
                    themeCount++;
                }
                else
                {
                    themeId = Themes.First(x => x.Name == themeName).Id;
                }

                // add link
                AutorQuoteThemes.Add(new AutorQuoteTheme(autorId, quoteId, themeId));
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
            _sqliteDbManager.InsertList(Quotes);
            _sqliteDbManager.InsertList(Autors);
            _sqliteDbManager.InsertList(Themes);
            _sqliteDbManager.InsertList(AutorQuoteThemes);
        }
    }
}
