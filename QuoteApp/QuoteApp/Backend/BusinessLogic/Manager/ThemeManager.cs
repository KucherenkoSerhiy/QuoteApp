using System.Collections.Generic;
using System.Linq;
using QuoteApp.Backend.Model;

namespace QuoteApp.Backend.BusinessLogic.Manager
{
    public class ThemeManager
    {
        #region Singleton
        private static ThemeManager _instance;
        public List<Theme> Themes { get; set; }

        public static ThemeManager Instance => _instance ?? (_instance = new ThemeManager());

        private ThemeManager()
        {
            _sqliteDbManager = SqliteDbManager.Instance;
            Themes = _sqliteDbManager.GetList<Theme>();
        }

        #endregion

        private readonly SqliteDbManager _sqliteDbManager;

        public void AddTheme(Theme theme)
        {
            _sqliteDbManager.Insert(theme);
            Themes.Add(theme);
        }

        public void UpdateTheme(Theme theme)
        {
            _sqliteDbManager.Update(theme);
            Themes.SingleOrDefault(x => x.Id == theme.Id)?.SetValues(theme);
        }

        public void DeleteTheme(Theme theme)
        {
            _sqliteDbManager.Delete(theme);
            Themes.Remove(theme);
        }

        public Theme GetThemeById(int themeId)
        {
            return Themes.Single(x => x.Id == themeId);
        }
    }
}