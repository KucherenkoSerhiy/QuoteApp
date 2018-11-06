using System.Collections.Generic;
using System.IO;
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
            dataBase = new SQLiteConnection(dbPath);
            InitializeDb();
        }

        #endregion

        private readonly string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), QuoteAppConstants.DatabaseFilePath);
        readonly SQLiteConnection dataBase;

        public void CreateTable<T>()
        {
            dataBase.CreateTable<T>();
        }

        
        private void InitializeDb()
        {
            CreateTable<Quote>();
            CreateTable<Autor>();
            CreateTable<Theme>();

            // read autors
            // read quotes
            // read themes
            // create relations
        }


        public void Insert <T>(T objectToInsert)
        {
            dataBase.Insert(objectToInsert);
        }

        public void Update<T>(T objectToUpdate)
        {
            dataBase.Update(objectToUpdate);
        }

        public void Delete<T>(T objectToDelete)
        {
            dataBase.Delete(objectToDelete);
        }

        public T Get<T>(int id) where T: new()
        {
            return dataBase.Get<T>(id);
        }

        public List<T> GetList<T>() where T: new()
        {
            return dataBase.Table<T>().ToList();
        }
    }
}