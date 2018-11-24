using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using QuoteApp.Backend.BusinessLogic.Manager;
using QuoteApp.Backend.BusinessLogic.Subsystem.PersistentProperties;
using SkiaSharp;
using SkiaSharp.Views.Forms;

namespace QuoteApp.Globals
{
    public class QuoteAppUtils
    {
        public static void SerializeToXml(object instance, string path)
        {
            using (FileStream fileStream = new FileStream(path, FileMode.Create))
            using (StreamWriter sw = new StreamWriter(fileStream))
            using (XmlTextWriter writer = new XmlTextWriter(sw))
            {
                XmlSerializer serializer = new XmlSerializer(instance.GetType());

                writer.Formatting = Formatting.Indented;
                writer.Indentation = 4;

                writer.WriteStartDocument();

                serializer.Serialize(writer, instance);

                writer.WriteEndDocument();
                writer.Close();
            }
        }

        public static T DeserializeXml<T>(string xmlFilePath)
        {
            string xmltext = File.ReadAllText(xmlFilePath);

            XmlReader xReader = XmlReader.Create(new StringReader(xmltext));
            var deserializer = new XmlSerializer(typeof(T));
            var deserializedData = (T)deserializer.Deserialize(xReader);

            return deserializedData;
        }

        /// <summary>
        /// Reads file located in project directory
        /// </summary>
        /// <param name="namespacePlusFileName">Must specify file location + namespace. Example: MyProject.MyFolder.MyFile</param>
        /// <returns>file content splitted by lines</returns>
        public static string[] ReadLocalFile(string namespacePlusFileName)
        {
            var assembly = typeof(SqliteDbManager).GetTypeInfo().Assembly;
            Stream stream = assembly.GetManifestResourceStream(namespacePlusFileName);
            string text = "";
            using (var reader = new System.IO.StreamReader(stream))
            {
                text = reader.ReadToEnd();
            }

            string[] lines = text.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            return lines;
        }

        /// <summary>
        /// Used to get font size
        /// </summary>
        /// <returns></returns>
        public static int PxToPt(int px)
        {
            return (int)(px * 0.75);
        }


        #region Background painting

        private static SKColor[] _themeColors;
        private static float[] _gradientPositions;

        public static SKCanvasView CreateGradientBackground(SKColor[] themeColors, float[] gradientPositions)
        {
            _themeColors = themeColors;
            _gradientPositions = gradientPositions;

            SKCanvasView background = new SKCanvasView();
            background.PaintSurface += OnCanvasViewPaintSurface;

            return background;
        }
        
        private static void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();

            using (SKPaint paint = new SKPaint())
            {
                SKRect rect = new SKRect(0, 0, App.ScreenWidth, App.ScreenHeight);

                paint.Shader = CreateGradientShader(ref rect);

                // Draw the gradient on the rectangle
                canvas.DrawRect(rect, paint);
            }
        }

        private static SKShader CreateGradientShader(ref SKRect rect)
        {
            return SKShader.CreateLinearGradient(
                new SKPoint(rect.Left, rect.Top),
                new SKPoint(rect.Right, rect.Bottom),
                _themeColors,
                _gradientPositions,
                SKShaderTileMode.Repeat);
        }

        #endregion

    }
}
