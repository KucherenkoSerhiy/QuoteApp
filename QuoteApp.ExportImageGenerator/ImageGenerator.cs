using System;
using System.Drawing;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Color = System.Windows.Media.Color;
using Point = System.Windows.Point;

namespace QuoteApp.ExportImageGenerator
{
    public class ImageGenerator
    {
        /// <summary>
        /// Generates new image with background, quote text, its autor and theme
        /// </summary>
        /// <param name="background"></param>
        /// <param name="quoteText"></param>
        /// <param name="autor"></param>
        /// <param name="theme"></param>
        /// <returns></returns>
        public ImageSource GenerateImageWithQuote(BitmapImage background, string quoteText, string autor, Color textColor)
        {
            var visual = new DrawingVisual();
            using (DrawingContext drawingContext = visual.RenderOpen())
            {
                int numberOfCharactersInLine = 30;
                int lines = (quoteText.Length / numberOfCharactersInLine) + 1;
                int maxLines = Math.Max(lines, 16);

                int quoteTextSize = 48 * Math.Min(maxLines / lines, 4);
                int autorSize = 48;

                // define the text to write
                var formattedQuoteText = new FormattedText(quoteText, CultureInfo.InvariantCulture, FlowDirection.LeftToRight,
                    new Typeface("Arial"), quoteTextSize, new SolidColorBrush(textColor));
                formattedQuoteText.MaxTextWidth = background.Width * 3 / 4;
                formattedQuoteText.TextAlignment = TextAlignment.Left;

                var formattedAutor = new FormattedText(autor, CultureInfo.InvariantCulture, FlowDirection.LeftToRight,
                    new Typeface("Arial"), autorSize, new SolidColorBrush(textColor));
                formattedAutor.TextAlignment = TextAlignment.Right;
                formattedAutor.SetFontStyle(FontStyles.Italic);

                // set the positions
                double quoteTextPositionX = background.PixelWidth / 8;
                double quoteTextPositionY = 200;
                double autorPositionX = background.PixelWidth - 50;
                double autorPositiony = background.PixelHeight - 300;
                
                // draw
                drawingContext.DrawImage(background,
                    new Rect(0, 0, background.PixelWidth, background.PixelHeight));
                drawingContext.DrawText(formattedQuoteText, new Point(quoteTextPositionX, quoteTextPositionY));
                drawingContext.DrawText(formattedAutor, new Point(autorPositionX, autorPositiony));
            }

            return new DrawingImage(visual.Drawing);
        }
    }
}