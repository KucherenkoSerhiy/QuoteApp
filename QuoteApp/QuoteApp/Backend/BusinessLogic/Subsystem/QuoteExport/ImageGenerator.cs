using System;
using SkiaSharp;

namespace QuoteApp.Backend.BusinessLogic.Subsystem.QuoteExport
{
    public static class ImageGenerator
    {
        /// <summary>
        /// Generates new image with background, quote text, its autor and theme
        /// </summary>
        /// <param name="background"></param>
        /// <param name="quoteText"></param>
        /// <param name="autor"></param>
        /// <param name="theme"></param>
        /// <returns></returns>
        public static SKBitmap GenerateImageWithQuote(SKBitmap background, string quoteText, string autor, string textColor)
        {
            int numberOfCharactersInLine = 30;
            int lines = (quoteText.Length / numberOfCharactersInLine) + 1;
            int maxLines = Math.Max(lines, 16);

            int quoteTextSize = 48 * Math.Min(maxLines / lines, 4);
            int autorSize = 48;

            // set the positions
            float quoteTextPositionX = background.Width / 8;
            float quoteTextPositionY = 200;
            float autorPositionX = background.Width - 50;
            float autorPositiony = background.Height - 300;
            
            using (var textPaint = new SKPaint())
            using (var autorPaint = new SKPaint())
            using (SKCanvas bitmapCanvas = new SKCanvas(background))
            {
                // define brushes for text and autor
                textPaint.TextSize = quoteTextSize;
                textPaint.Typeface = SKTypeface.FromFamilyName("Arial");
                textPaint.Color = new SKColor(255, 255, 0, 0);
                textPaint.TextAlign = SKTextAlign.Center;
                textPaint.IsStroke = true;
                textPaint.StrokeWidth = background.Width * 3 / 4;
                textPaint.IsAntialias = true;

                autorPaint.TextSize = autorSize;
                autorPaint.Typeface = SKTypeface.FromFamilyName("Arial", SKFontStyleWeight.Medium,
                    SKFontStyleWidth.Normal, SKFontStyleSlant.Italic);
                autorPaint.Color = SKColor.Parse(textColor);
                autorPaint.TextAlign = SKTextAlign.Right;
                autorPaint.IsStroke = true;
                autorPaint.StrokeWidth = background.Width * 3 / 4; // max width
                autorPaint.IsAntialias = true;
                
                bitmapCanvas.DrawText(quoteText, quoteTextPositionX, quoteTextPositionY, textPaint);
                bitmapCanvas.DrawText(autor, autorPositionX, autorPositiony, autorPaint);
                
                return background;
            }
        }
    }
}