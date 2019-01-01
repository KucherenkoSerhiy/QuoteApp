using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using QuoteApp.Backend.Model;
using QuoteApp.Globals;

namespace QuoteApp.Backend.BusinessLogic.Subsystem.PersistentProperties
{
    [Serializable]
    public class PersistentProperties
    {
        private static readonly string FilePath = Path
            .Combine(Environment.GetFolderPath(System.Environment.SpecialFolder.Personal),
                QuoteAppConstants.PropertiesFilePath).Substring(1);

        #region Singleton
        private static PersistentProperties _instance;

        public static PersistentProperties Instance
        {
            get
            {
                if (_instance != null) return _instance;

                if (File.Exists(FilePath))
                {
                    if (!string.IsNullOrWhiteSpace(File.ReadAllText(FilePath)))
                    {
                        _instance = QuoteAppUtils.DeserializeXml<PersistentProperties>(FilePath);
                    }
                    else
                    {
                        File.Delete(FilePath);
                        _instance = new PersistentProperties();
                    }
                }
                else
                {
                    _instance = new PersistentProperties();
                }

                return _instance;
            }
        }

        private PersistentProperties()
        {
            
        }

        #endregion

        private bool _nightModeActivated;

        public bool DatabaseIsInitialized { get; set; }
        public bool NightModeActivated
        {
            get => _nightModeActivated;
            set
            {
                _nightModeActivated = value; 
                OnNightModeSwitched();
            }
        }

        public bool OnlyUnreadQuotes { get; set; } = true;
        public EnEvaluation SelectedThemeRange { get; set; } = EnEvaluation.Recommended;
        public bool FirstTimeEnteredMainMenu { get; set; } = true;

        public void SerializeToXml()
        {
            QuoteAppUtils.SerializeToXml(this, FilePath);
        }

        #region Events

        public event EventHandler NightModeSwitched;

        protected virtual void OnNightModeSwitched()
        {
            NightModeSwitched?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}
