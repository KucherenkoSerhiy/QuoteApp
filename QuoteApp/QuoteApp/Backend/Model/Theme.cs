using System.Collections.Generic;
using System.Drawing;
using SQLite;

namespace QuoteApp.Backend.Model
{
    public class Theme
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }
        public Color BackgroundColor { get; set; }
        public Color ForegroundColor { get; set; }

        
        [Ignore]
        public List<Quote> Quotes { get; set; }
        [Ignore]
        public List<Autor> Autors { get; set; }
        
        public void SetValues(Theme theme)
        {
            this.Name = theme.Name;
            this.BackgroundColor = theme.BackgroundColor;
            this.ForegroundColor = theme.ForegroundColor;
        }
    }
}