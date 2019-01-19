using System;
using System.Collections.Generic;
using System.Text;
using QuoteApp.Backend.BusinessLogic.Manager;

namespace QuoteApp.Backend.BusinessLogic.Subsystem.SubsystemInitializer
{
    public class SubsystemInitializer
    {
        #region Singleton
        private static SubsystemInitializer _instance;

        public static SubsystemInitializer Instance => 
            _instance ?? (_instance = new SubsystemInitializer());

        private SubsystemInitializer()
        {
            var databaseManager =  DatabaseManager.Instance;
            var persistentProperties = PersistentProperties.PersistentProperties.Instance;
        }

        #endregion

    }
}
