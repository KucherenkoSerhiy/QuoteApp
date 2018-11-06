using System;
using System.Collections.Generic;
using SQLite;

namespace QuoteApp.Backend.Model
{
    public class Autor
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }

        public string FullName { get; set; }
        public string Title { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime? DeathDate { get; set; }

        [Ignore]
        public List<Quote> Quotes { get; set; }
        [Ignore]
        public List<Theme> Themes { get; set; }

        public void SetValues(Autor autor)
        {
            this.FullName = autor.FullName;
            this.Title = autor.Title;
            this.BirthDate = autor.BirthDate;
            this.DeathDate = autor.DeathDate;
        }
    }
}