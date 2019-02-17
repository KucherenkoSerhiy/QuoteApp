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

            // define brushes for text and autor
            SKPaint textPaint = new SKPaint
            {
                TextSize = quoteTextSize,
                Typeface = SKTypeface.FromFamilyName("Arial"),
                Color = new SKColor(255, 255, 0, 0),
                TextAlign = SKTextAlign.Center,
                IsStroke = true,
                StrokeWidth = background.Width * 3 / 4,
                IsAntialias = true
            };
            SKPaint autorPaint = new SKPaint
            {
                TextSize = autorSize,
                Typeface = SKTypeface.FromFamilyName("Arial", 
                    SKFontStyleWeight.Medium, SKFontStyleWidth.Normal, SKFontStyleSlant.Italic),
                Color = SKColor.Parse(textColor),
                TextAlign = SKTextAlign.Right,
                IsStroke = true,
                StrokeWidth = background.Width * 3 / 4, // max width
                IsAntialias = true
            };

            // set the positions
            float quoteTextPositionX = background.Width / 8;
            float quoteTextPositionY = 200;
            float autorPositionX = background.Width - 50;
            float autorPositiony = background.Height - 300;

            using (SKCanvas bitmapCanvas = new SKCanvas(background))
            {
                bitmapCanvas.DrawText(quoteText, quoteTextPositionX, quoteTextPositionY, textPaint);
                bitmapCanvas.DrawText(autor, autorPositionX, autorPositiony, autorPaint);

                SKPaint tPaint = new SKPaint
                {
                    TextSize = 64,
                    Color = new SKColor(255, 255, 0, 0),
                    TextAlign = SKTextAlign.Left,
                    IsAntialias = true
                };
                bitmapCanvas.DrawText("Hello SkiaSharp!", 0, 0, tPaint);
                bitmapCanvas.Save();
            
                return background;
            }
        }
    }
}