using System;
using QuoteApp.Backend.Model;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace QuoteApp.FrontEnd.View.ItemView
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QuoteItemView : ContentPage
    {
        public Quote QuoteItem { get; set; }

        public QuoteItemView()
        {
            QuoteItem = new Quote
            {
                Date = DateTime.Now,
                Text = "I used to think I was indecisive, but now I'm not too sure.",
                Context = "Said by some anonymous person in the street, let's suppose it's said by a cat."
            };

            InitializeComponent();

            Title = "Corner-to-Corner Gradient";

            SKCanvasView canvasView = new SKCanvasView();
            canvasView.PaintSurface += OnCanvasViewPaintSurface;

            Content = new AbsoluteLayout
            {
                Children =
                {
                    {canvasView, new Rectangle(0, 0, 1, 1), AbsoluteLayoutFlags.All},
                    {ContentRoot, new Rectangle(0, 0, 1, 1), AbsoluteLayoutFlags.All}
                }
            };
        }

        void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();

            using (SKPaint paint = new SKPaint())
            {
                // Create 300-pixel square centered rectangle
                SKRect rect = new SKRect(0, 0, App.ScreenWidth, App.ScreenHeight);

                // Create linear gradient from upper-left to lower-right
                paint.Shader = SKShader.CreateLinearGradient(
                    new SKPoint(rect.Left, rect.Top),
                    new SKPoint(rect.Right, rect.Bottom),
                    new[] { SKColors.Red, SKColors.Green, SKColors.Blue },
                    new[] { 0, (float)0.75, 1 },
                    SKShaderTileMode.Repeat);

                // Draw the gradient on the rectangle
                canvas.DrawRect(rect, paint);
            }
        }
    }
}