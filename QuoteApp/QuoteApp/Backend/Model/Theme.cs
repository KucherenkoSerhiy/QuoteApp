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

        // Specifies color by its hex code. Format example: a1b2c3
        public string DayLineColor { get; set; }
        public string DayTextColor { get; set; }
        public string NightLineColor { get; set; }
        public string NightTextColor { get; set; }
    }
}