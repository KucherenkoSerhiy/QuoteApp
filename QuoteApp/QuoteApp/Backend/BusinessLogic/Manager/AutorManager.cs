using System.Collections.Generic;
using System.Linq;
using QuoteApp.Backend.Model;

namespace QuoteApp.Backend.BusinessLogic.Manager
{
    public class AutorManager
    {
        #region Singleton
        private static AutorManager _instance;

        public static AutorManager Instance => _instance ?? (_instance = new AutorManager());

        private AutorManager()
        {
            _sqliteDbManager = SqliteDbManager.Instance;
            Autors = _sqliteDbManager.GetList<Autor>();
        }

        #endregion

        private readonly SqliteDbManager _sqliteDbManager;
        public List<Autor> Autors { get; set; }

        public void AddAutor(Autor autor)
        {
            _sqliteDbManager.Insert(autor);
            Autors.Add(autor);
        }

        public void UpdateAutor(Autor autor)
        {
            _sqliteDbManager.Update(autor);
            Autors.SingleOrDefault(x => x.Id == autor.Id)?.SetValues(autor);
        }

        public void DeleteAutor(Autor autor)
        {
            _sqliteDbManager.Delete(autor);
            Autors.Remove(autor);
        }

        public Autor GetAutorById(int autorId)
        {
            return Autors.Single(x => x.Id == autorId);
        }
    }
}