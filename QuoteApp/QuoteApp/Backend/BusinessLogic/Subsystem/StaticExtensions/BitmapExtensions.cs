using System;
using System.IO;
using System.Reflection;
using SkiaSharp;

namespace QuoteApp.Backend.BusinessLogic.Subsystem.StaticExtensions
{
    public static class BitmapExtensions
    {
        public static SKBitmap LoadBitmapResource(Type type, string resourceId)
        {
            Assembly assembly = type.GetTypeInfo().Assembly;

            using (Stream stream = assembly.GetManifestResourceStream(resourceId))
            {
                return SKBitmap.Decode(stream);
            }
        }
    }
}
