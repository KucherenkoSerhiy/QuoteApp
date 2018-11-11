﻿using System;
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

        
        [Ignore]
        public List<Autor> Autors { get; set; }
        [Ignore]
        public List<Theme> Themes { get; set; }

        public Quote()
        {
            Autors = new List<Autor>();
            Themes = new List<Theme>();
        }

    }
}