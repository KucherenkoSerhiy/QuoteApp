using System;
using System.Collections.Generic;
using System.IO;
using QuoteApp.Backend.BusinessLogic.Subsystem.PersistentProperties;
using QuoteApp.Backend.Model;
using QuoteApp.Globals;
using SQLite;

namespace QuoteApp.Backend.BusinessLogic.Manager
{
    internal class SqliteDbManager
    {
        #region Singleton
        private static SqliteDbManager _instance;

        public static SqliteDbManager Instance => _instance ?? (_instance = new SqliteDbManager());

        private SqliteDbManager()
        {
            if (!PersistentProperties.Instance.DatabaseIsInitialized)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(_dbPath));
                if (File.Exists(_dbPath)) File.Delete(_dbPath);
            }

            _dataBase = new SQLiteConnection(_dbPath);
        }

        #endregion

        private readonly string _dbPath = Path
            .Combine(Environment.GetFolderPath(System.Environment.SpecialFolder.Personal),
                QuoteAppConstants.DatabaseFilePath).Substring(1);
        private SQLiteConnection _dataBase;

        public void CreateTable<T>()
        {
            _dataBase.CreateTable<T>();
        }

        public void InsertList<T>(IEnumerable<T> objectToInsert)
        {
            _dataBase.InsertAll(objectToInsert);
        }

        public void Update<T>(T objectToUpdate)
        {
            _dataBase.Update(objectToUpdate);
        }
        
        public List<T> GetList<T>() where T: new()
        {
            return _dataBase.Table<T>().ToList();
        }
    }
}