using System;
using System.Collections.Generic;
using System.Text;
using QuoteApp.Backend.BusinessLogic.Manager;

namespace QuoteApp.Backend.BusinessLogic.Subsystem.Database
{
    public class DatabaseBusinessLogics
    {
        private QuoteManager _quoteManager;
        private AutorManager _autorManager;
        private ThemeManager _themeManager;

        public void PopulateDatabase()
        {
            _quoteManager = QuoteManager.Instance;
            _autorManager = AutorManager.Instance;
            _themeManager = ThemeManager.Instance;


        }

        public void LoadDatabase()
        {

        }
    }
}
