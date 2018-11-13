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
    }
}