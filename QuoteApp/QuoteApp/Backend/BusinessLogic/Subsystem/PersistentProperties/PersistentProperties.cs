using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using QuoteApp.Globals;

namespace QuoteApp.Backend.BusinessLogic.Subsystem.PersistentProperties
{
    [Serializable]
    public class PersistentProperties
    {
        private static readonly string FilePath = Path
            .Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal),
                QuoteAppConstants.PropertiesFilePath).Substring(1);

        #region Singleton
        private static PersistentProperties _instance;

        public static PersistentProperties Instance => _instance ?? (_instance = File.Exists(FilePath)
                                                           ? QuoteAppUtils.DeserializeXml<PersistentProperties>(FilePath)
                                                           : new PersistentProperties());

        private PersistentProperties()
        {
            
        }

        #endregion

        public bool DatabaseIsInitialized { get; set; }

        public void SerializeToXml()
        {
            QuoteAppUtils.SerializeToXml(this, FilePath);
        }
    }
}
