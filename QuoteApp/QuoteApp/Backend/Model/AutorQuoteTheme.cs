using SQLite;

namespace QuoteApp.Backend.Model
{
    /// <summary>
    /// Represents n to n to n' relation between Autor, Quote and Theme
    /// </summary>
    public class AutorQuoteTheme
    {
        public int AutorId { get; set; }

        public int QuoteId { get; set; }

        public int ThemeId { get; set; }

        public AutorQuoteTheme(int autorId, int quoteId, int themeId)
        {
            AutorId = autorId;
            QuoteId = quoteId;
            ThemeId = themeId;
        }

        public AutorQuoteTheme()
        {
        }
    }
}
