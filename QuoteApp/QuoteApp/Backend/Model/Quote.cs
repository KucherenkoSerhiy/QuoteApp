using System;
using System.Collections.Generic;
using SQLite;

namespace QuoteApp.Backend.Model
{
    public class Quote
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }

        public string Text { get; set; }
        public string Context { get; set; }
        public DateTime Date { get; set; }

        
        [Ignore]
        public List<Autor> Autors { get; set; }
        [Ignore]
        public List<Theme> Themes { get; set; }
        
        public void SetValues(Quote quote)
        {
            this.Text = quote.Text;
            this.Context = quote.Context;
            this.Date = quote.Date;
        }
    }
}