using System.Collections.Generic;
using System.Drawing;
using SQLite;

namespace QuoteApp.Backend.Model
{
    public class ThemeColor
    {
        public string ColorCode { get; set; }
        public int GradientPosition { get; set; }
        
        [Ignore]
        public Theme Theme { get; set; }
    }
}