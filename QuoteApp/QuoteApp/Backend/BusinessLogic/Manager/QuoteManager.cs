using System.Collections.Generic;
using System.Linq;
using QuoteApp.Backend.Model;

namespace QuoteApp.Backend.BusinessLogic.Manager
{
    public class QuoteManager
    {
        #region Singleton
        private static QuoteManager _instance;

        public static QuoteManager Instance => _instance ?? (_instance = new QuoteManager());

        private QuoteManager()
        {
            _sqliteDbManager = SqliteDbManager.Instance;
            Quotes = _sqliteDbManager.GetList<Quote>();
        }

        #endregion

        private readonly SqliteDbManager _sqliteDbManager;
        public List<Quote> Quotes { get; set; }

        public void AddQuote(Quote quote)
        {
            _sqliteDbManager.Insert(quote);
            Quotes.Add(quote);
        }

        public void UpdateQuote(Quote quote)
        {
            _sqliteDbManager.Update(quote);
            Quotes.SingleOrDefault(x => x.Id == quote.Id)?.SetValues(quote);
        }

        public void DeleteQuote(Quote quote)
        {
            _sqliteDbManager.Delete(quote);
            Quotes.Remove(quote);
        }

        public Quote GetQuoteById(int quoteId)
        {
            return Quotes.Single(x => x.Id == quoteId);
        }
    }
}