using System;
using System.Collections.Generic;
using SQLite;

namespace QuoteApp.Backend.Model
{
    public class Autor
    {
        [PrimaryKey]
        public int Id { get; set; }

        public string FullName { get; set; }
        public string Title { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime? DeathDate { get; set; }

        [Ignore]
        public List<Quote> Quotes { get; set; }
        [Ignore]
        public List<Theme> Themes { get; set; }

        public Autor()
        {
            Quotes = new List<Quote>();
            Themes = new List<Theme>();
        }
    }
}