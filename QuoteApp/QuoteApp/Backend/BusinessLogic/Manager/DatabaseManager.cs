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
                Quotes = _sqliteDbManager.GetList<Quote>();
                Autors = new SortedDictionary<string, Autor>(_sqliteDbManager.GetList<Autor>().ToDictionary(x => x.FullName, x => x)); // Todo: suspiciously many autors
                Themes = new SortedDictionary<string, Theme>(_sqliteDbManager.GetList<Theme>().ToDictionary(x => x.Name, x => x));
                AutorQuoteThemes = _sqliteDbManager.GetList<AutorQuoteTheme>();
                //Todo: theme colors
            }
            else
            {
                InitializeDatabase();
            }
        }

        #endregion

        private SqliteDbManager _sqliteDbManager;

        public List<Quote> Quotes { get; set; }
        public SortedDictionary<string, Autor> Autors { get; set; }
        public SortedDictionary<string,Theme> Themes { get; set; }
        public List<AutorQuoteTheme> AutorQuoteThemes { get; set; }
        
        private void InitializeDatabase()
        {
            string[] lines = QuoteAppUtils.ReadLocalFile("QuoteApp.Resources.QuotesDatabaseLite.csv").Skip(2).ToArray();

            AutorQuoteThemes = new List<AutorQuoteTheme>();

            Stopwatch watch = Stopwatch.StartNew();

            ExtractData(lines);
            PopulateDatabase();

            watch.Stop();

            string watchTime = $"Elapsed: {watch.ElapsedMilliseconds / 1000}";

            string summary = $"Total: Quotes={Quotes.Count}, Autors={Autors.Count}, Themes={Themes.Count}, Links={AutorQuoteThemes.Count}";

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
                Quotes.Add(new Quote { Id = quoteCount, Text = quoteText });
                int quoteId = quoteCount;
                quoteCount++;

                // add autor if not exists and get his/her id (or find id of existent one)
                int autorId;
                if (Autors[autorFullName] == null)
                {
                    Autors[autorFullName] = new Autor {Id = autorCount, FullName = autorFullName};
                    autorId = autorCount;
                    autorCount++;
                }
                else
                {
                    autorId = Autors[autorFullName].Id;
                }

                // add theme if not exists and get its id (or find id of existent one)
                int themeId;
                if (Themes[themeName] == null)
                {
                    Themes[themeName] = new Theme
                    {
                        Id = themeCount,
                        Name = themeName,
                        DayLineColor = QuoteAppConstants.DefaultDayLineColor,
                        DayTextColor = QuoteAppConstants.DefaultDayTextColor,
                        NightLineColor = QuoteAppConstants.DefaultNightLineColor,
                        NightTextColor = QuoteAppConstants.DefaultNightTextColor
                    };

                    themeId = themeCount;
                    themeCount++;
                }
                else
                {
                    themeId = Themes[themeName].Id;
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

        public IEnumerable<Quote> GetQuotesByAutor(Autor autor)
        {
            var selectedAutorQuoteThemes = AutorQuoteThemes.Where(aqt => aqt.AutorId == autor.Id);

            return Quotes.Where(q => selectedAutorQuoteThemes.Any(aqt => aqt.QuoteId == q.Id));
        }

        
        public IEnumerable<Quote> GetQuotesByTheme(Theme theme)
        {
            var selectedAutorQuoteThemes = AutorQuoteThemes.Where(aqt => aqt.AutorId == theme.Id);

            return Quotes.Where(q => selectedAutorQuoteThemes.Any(aqt => aqt.QuoteId == q.Id));
        }

        public Theme GetThemeByAutor(Autor autor)
        {
            var selectedAutorQuoteThemes = AutorQuoteThemes.Where(aqt => aqt.AutorId == autor.Id);

            return Themes.Single(x => x.Value.Id == selectedAutorQuoteThemes.First().ThemeId).Value;
        }

        public Autor GetAutorByTheme(Theme theme)
        {
            var selectedAutorQuoteThemes = AutorQuoteThemes.Where(aqt => aqt.AutorId == theme.Id);

            return Autors.Single(x => x.Value.Id == selectedAutorQuoteThemes.First().AutorId).Value;
        }
    }
}
