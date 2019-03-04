using System;
using System.Text;
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

            int textSize = 64;

            // set the positions
            float quoteTextPositionX = background.Width / 8;
            float quoteTextPositionY = 200;
            float autorPositionX = background.Width - 50;
            float autorPositiony = background.Height - 300;

            using (SKCanvas bitmapCanvas = new SKCanvas(background))
            {                
                //bitmapCanvas.DrawText(quoteText, quoteTextPositionX, quoteTextPositionY, textPaint);
                //bitmapCanvas.DrawText(autor, autorPositionX, autorPositiony, autorPaint);

                //THIS WORKS
                //var font = SKTypeface.FromFamilyName("Arial");
                //var brush = new SKPaint
                //{
                //    Typeface = font,
                //    TextSize = 64.0f,
                //    IsAntialias = true,
                //    Color = new SKColor(255, 255, 255, 255)
                //};

                //bitmapCanvas.DrawText("Resized!", 0, background.Height * 0.5f / 2.0f, brush);

                // define brushes for text and autor
                var textPaint = new SKPaint
                {
                    TextSize = textSize,
                    Typeface = SKTypeface.FromFamilyName("Open Sans"),
                    Color = SKColor.Parse(textColor),
                    TextAlign = SKTextAlign.Center,
                    IsAntialias = true
                };

                var autorPaint = new SKPaint
                {
                    TextSize = textSize,
                    Typeface = SKTypeface.FromFamilyName("Open Sans", SKFontStyleWeight.Medium,
                        SKFontStyleWidth.Normal, SKFontStyleSlant.Italic),
                    Color = SKColor.Parse(textColor),
                    TextAlign = SKTextAlign.Right,
                    IsAntialias = true
                };

                var multilineQuoteText = MakeMultiline(quoteText);
                //bitmapCanvas.DrawText(quoteText, quoteTextPositionX, quoteTextPositionY, textPaint);
                
                for (var i = 0; i < multilineQuoteText.Length; i++)
                {
                    string s = multilineQuoteText[i];
                    bitmapCanvas.DrawText(s, background.Width/2, (background.Height * 0.5f + 100 * i) / 2.0f, textPaint);
                }

                return background;
            }
        }

        private static string[] MakeMultiline(string text)
        {
            int characterWidth = 30;
            int line = 1;
            StringBuilder sb = new StringBuilder(text);

            // for each line...
            while (line * characterWidth < text.Length)
            {
                // find last space character and convert it to newline
                int index = line * characterWidth - 1;

                try
                {
                    while (text[index] != ' ')
                        index--;
                    if (text[index] == ' ')
                    {
                        sb[index] = '\n'; // TODO: Invalid new line character
                    }
                }
                catch
                {
                    // ignored
                }

                line++;
            }

            return sb.ToString().Split('\n');
        }
    }
}