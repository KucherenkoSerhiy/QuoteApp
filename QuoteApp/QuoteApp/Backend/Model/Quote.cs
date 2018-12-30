using System;
using System.Collections.Generic;
using SQLite;

namespace QuoteApp.Backend.Model
{
    public class Quote
    {
        [PrimaryKey]
        public int Id { get; set; }

        public string Text { get; set; }
        public string Context { get; set; }
        public DateTime Date { get; set; }
        public bool HasBeenRead { get; set; }
    }
}