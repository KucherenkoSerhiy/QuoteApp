using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using QuoteApp.Backend.Services;
using QuoteApp.Droid;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: Dependency(typeof(ShareServiceActivity))]
namespace QuoteApp.Droid
{
    public class ShareServiceActivity : Activity, IShareService
    {
        public async void Share(string subject, string message, ImageSource image)
        {
            var intent = new Intent(Intent.ActionSend);
            //intent.PutExtra(Intent.ExtraSubject, subject);
            intent.PutExtra(Intent.ExtraText, message);
            intent.SetType("image/png");

            var handler = GetHandler(image);
            var bitmap = await handler.LoadImageAsync(image, this);

            var path = Environment.GetExternalStoragePublicDirectory(Environment.DirectoryDownloads
                                                                     + Java.IO.File.Separator + "shareImage.png");

            using (var stream = new System.IO.FileStream(path.AbsolutePath, System.IO.FileMode.Create))
            {
                bitmap.Compress(Bitmap.CompressFormat.Png, 100, stream);
            }

            intent.PutExtra(Intent.ExtraStream, Android.Net.Uri.FromFile(path));
            Forms.Context.StartActivity(Intent.CreateChooser(intent, "Share Image"));
        }

        private static IImageSourceHandler GetHandler(ImageSource source)
        {
            IImageSourceHandler returnValue = null;
            switch (source)
            {
                case UriImageSource _:
                    returnValue = new ImageLoaderSourceHandler();
                    break;
                case FileImageSource _:
                    returnValue = new FileImageSourceHandler();
                    break;
                case StreamImageSource _:
                    returnValue = new StreamImagesourceHandler();
                    break;
                case SKBitmapImageSource _:
                    returnValue = new SKImageSourceHandler();
                    break;
            }
            return returnValue;
        }
    }
}