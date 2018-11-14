using System;
using System.Collections.Generic;
using System.Linq;
using QuoteApp.Backend.BusinessLogic.Subsystem.PersistentProperties;
using QuoteApp.Backend.Model;
using QuoteApp.Globals;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace QuoteApp.FrontEnd.View.ItemView
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QuoteItemView : ContentPage
    {
        public Autor AutorItem { get; set; }
        public Quote QuoteItem { get; set; }
        public Theme ThemeItem { get; set; }

        public List<ThemeColor> ThemeDayBackgroundColorItems { get; set; }
        public List<ThemeColor> ThemeNightBackgroundColorItems { get; set; }
        public Color LineColor => PersistentProperties.Instance.NightModeActivated ? NightLineColor : DayLineColor;
        public Color TextColor => PersistentProperties.Instance.NightModeActivated ? NightTextColor : DayTextColor;

        private Color DayLineColor { get; set; }
        private Color DayTextColor { get; set; }
        private Color NightLineColor { get; set; }
        private Color NightTextColor { get; set; }

        public QuoteItemView()
        {
            InitializeDefaultValues();
            RetrieveDependencies();

            InitializeComponent();
            SetPageContent();
        }

        private void SetPageContent()
        {
            NavigationPage.SetHasNavigationBar(this, false);

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

        private void InitializeDefaultValues()
        {
            ThemeDayBackgroundColorItems = QuoteAppConstants.DefaultDayBackgroundColorGradientItems;
            DayLineColor = Color.FromHex(QuoteAppConstants.DefaultDayLineColor);
            DayTextColor = Color.FromHex(QuoteAppConstants.DefaultDayTextColor);
            ThemeNightBackgroundColorItems = QuoteAppConstants.DefaultNightBackgroundColorGradientItems;
            NightLineColor = Color.FromHex(QuoteAppConstants.DefaultNightLineColor);
            NightTextColor = Color.FromHex(QuoteAppConstants.DefaultNightTextColor);
        }

        private void RetrieveDependencies()
        {
            AutorItem = new Autor { FullName = "Indecisive anonymous" };
            QuoteItem = new Quote { Text = "I used to think I was indecisive, but now I'm not too sure." };
            ThemeItem = new Theme { Name = "Life" };
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
                paint.Shader = CreateGradientShader(ref rect);

                // Draw the gradient on the rectangle
                canvas.DrawRect(rect, paint);
            }
        }

        private SKShader CreateGradientShader(ref SKRect rect)
        {
            SKColor[] themeColors = PersistentProperties.Instance.NightModeActivated
                ? ThemeNightBackgroundColorItems.Select(x => SKColor.Parse(x.ColorCode)).ToArray()
                : ThemeDayBackgroundColorItems.Select(x => SKColor.Parse(x.ColorCode)).ToArray();

            float[] gradientPositions =  PersistentProperties.Instance.NightModeActivated
                ? ThemeNightBackgroundColorItems.Select(x => x.GradientPosition).ToArray()
                : ThemeDayBackgroundColorItems.Select(x => x.GradientPosition).ToArray();

            return SKShader.CreateLinearGradient(
                                new SKPoint(rect.Left, rect.Top),
                                new SKPoint(rect.Right, rect.Bottom),
                                themeColors,
                                gradientPositions,
                                SKShaderTileMode.Repeat);
        }
    }
}