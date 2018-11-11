using System.Collections.Generic;
using System.Drawing;
using SQLite;

namespace QuoteApp.Backend.Model
{
    public class Theme
    {
        [PrimaryKey]
        public int Id { get; set; }
        public string Name { get; set; }
        
        [Ignore]
        public List<Quote> Quotes { get; set; }
        [Ignore]
        public List<Autor> Autors { get; set; }
        [Ignore]
        public List<ThemeColor> Colors { get; set; }
        
        public Theme()
        {
            Quotes = new List<Quote>();
            Autors = new List<Autor>();
            Colors = new List<ThemeColor>();
        }
    }
}