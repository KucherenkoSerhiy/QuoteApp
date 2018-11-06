using SQLite;

namespace QuoteApp.Backend.Model
{
    /// <summary>
    /// Represents n to n to n' relation between Autor, Quote and Theme
    /// </summary>
    public class AutorQuoteTheme
    {
        [PrimaryKey] public int AutorId { get; set; }
        [PrimaryKey] public int QuoteId { get; set; }
        [PrimaryKey] public int ThemeId { get; set; }
    }
}
