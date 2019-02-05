

using System;
using System.Globalization;
using SkiaSharp;
using Xamarin.Forms;

namespace QuoteApp.ExportImageGenerator
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
                Color = SKColor.Parse(textColor),
                IsStroke = true,
                StrokeWidth = background.Width * 3 / 4, // max width
                TextAlign = SKTextAlign.Center
            };
            SKPaint autorPaint = new SKPaint
            {
                TextSize = autorSize,
                Typeface = SKTypeface.FromFamilyName("Arial", 
                    SKFontStyleWeight.Medium, SKFontStyleWidth.Normal, SKFontStyleSlant.Italic),
                Color = SKColor.Parse(textColor),
                IsStroke = true,
                StrokeWidth = background.Width * 3 / 4, // max width
                TextAlign = SKTextAlign.Right
            };

            // set the positions
            float quoteTextPositionX = background.Width / 8;
            float quoteTextPositionY = 200;
            float autorPositionX = background.Width - 50;
            float autorPositiony = background.Height - 300;

            // draw
            SKBitmap copy = background.Copy();
            using (SKCanvas bitmapCanvas = new SKCanvas(copy))
            {
                bitmapCanvas.DrawText(quoteText, quoteTextPositionX, quoteTextPositionY, textPaint);
                bitmapCanvas.DrawText(autor, autorPositionX, autorPositiony, autorPaint);

                bitmapCanvas.Save();
            }

            return copy;
        }
    }
}