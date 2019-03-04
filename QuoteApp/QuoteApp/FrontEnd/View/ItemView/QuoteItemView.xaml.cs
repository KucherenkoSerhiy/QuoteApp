using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using QuoteApp.Backend.BusinessLogic.Manager;
using QuoteApp.Backend.BusinessLogic.Subsystem.PersistentProperties;
using QuoteApp.Backend.BusinessLogic.Subsystem.QuoteExport;
using QuoteApp.Backend.BusinessLogic.Subsystem.StaticExtensions;
using QuoteApp.Backend.Model;
using QuoteApp.Backend.Services;
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
        private DatabaseManager _databaseManager;

        private EnQuoteSource _quoteSource;
        private Quote _quoteItem = new Quote();

        private SKCanvasView _background;

        public Quote QuoteItem
        {
            get => _quoteItem;
            set
            {
                if (!string.IsNullOrWhiteSpace(_quoteItem.Text)) BackupQuote = _quoteItem;

                _quoteItem = value; 
                _databaseManager.SetQuoteRead(QuoteItem);
            }
        }

        public Quote BackupQuote { get; set; }

        public Autor AutorItem { get; set; } = new Autor();
        public Theme ThemeItem { get; set; } = new Theme
        {
            Name = "Life",
            Id = 9000,
            DayLineColor = QuoteAppConstants.DefaultDayLineColor,
            DayTextColor = QuoteAppConstants.DefaultDayTextColor,
            NightLineColor = QuoteAppConstants.DefaultNightLineColor,
            NightTextColor = QuoteAppConstants.DefaultNightTextColor
        };
        
        public List<ThemeColor> ThemeDayBackgroundColorItems { get; set; }
        public List<ThemeColor> ThemeNightBackgroundColorItems { get; set; }
        
        #region Getter Properties

        public int QuoteTextSize => QuoteAppUtils.PxToPt(App.ScreenHeight/50);
        public int ThemeTextSize => QuoteAppUtils.PxToPt(App.ScreenHeight/25);
        public int ButtonTextSize => QuoteAppUtils.PxToPt(App.ScreenHeight/40);
        public string ButtonBackText => BackupQuote == null? "Back" : "Prev Quote";

        public Color LineColor => PersistentProperties.Instance.NightModeActivated
            ? Color.FromHex(ThemeItem.NightLineColor)
            : Color.FromHex(ThemeItem.DayLineColor);

        public Color TextColor => PersistentProperties.Instance.NightModeActivated
            ? Color.FromHex(ThemeItem.NightTextColor)
            : Color.FromHex(ThemeItem.DayTextColor);
        
        #endregion

        public QuoteItemView()
        {
            InitializeDefaultValues();

            InitializeComponent();
        }

        
        public void SetAutor(Autor autor)
        {
            _quoteSource = EnQuoteSource.Autor;
            AutorItem = autor;
            
            GetNextQuote();
        }

        public void SetTheme(Theme theme)
        {
            _quoteSource = EnQuoteSource.Theme;
            ThemeItem = theme;

            GetNextQuote();
        }
        
        public void SetQuote(Quote quote)
        {
            _quoteSource = EnQuoteSource.Random;
            QuoteItem = quote;
            AutorItem = _databaseManager.GetAutorByQuote(quote);
            ThemeItem = _databaseManager.GetThemeByQuote(quote);

            OnPropertyChanged("");
        }

        #region Initialization

        private void InitializeDefaultValues()
        {
            _databaseManager = DatabaseManager.Instance;
            ThemeDayBackgroundColorItems = QuoteAppConstants.DefaultDayBackgroundColorGradientItems;
            ThemeNightBackgroundColorItems = QuoteAppConstants.DefaultNightBackgroundColorGradientItems;
        }

        private void SetPageContent()
        {
            NavigationPage.SetHasNavigationBar(this, false);

            ShareButtonImage.Source = PersistentProperties.Instance.NightModeActivated
                ? "icon_link_night.png" : "icon_link_day.png";

            SKColor[] themeColors = PersistentProperties.Instance.NightModeActivated
                ? ThemeNightBackgroundColorItems.Select(x => SKColor.Parse(x.ColorCode)).ToArray()
                : ThemeDayBackgroundColorItems.Select(x => SKColor.Parse(x.ColorCode)).ToArray();

            float[] gradientPositions =  PersistentProperties.Instance.NightModeActivated
                ? ThemeNightBackgroundColorItems.Select(x => x.GradientPosition).ToArray()
                : ThemeDayBackgroundColorItems.Select(x => x.GradientPosition).ToArray();

            _background = QuoteAppUtils.CreateGradientBackground(themeColors, gradientPositions);

            Content = new AbsoluteLayout
            {
                Children =
                {
                    {_background, new Rectangle(0, 0, 1, 1), AbsoluteLayoutFlags.All},
                    {ContentRoot, new Rectangle(0, 0, 1, 1), AbsoluteLayoutFlags.All}
                }
            };
        }

        #endregion

        #region UI Event Handling
        
        protected override void OnAppearing()
        {
            SetPageContent();
            OnPropertyChanged("");
        }
        
        /// <summary>
        /// Shows previous viewed quote or if there is no such the one, pops back in the navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ButtonBack_OnClicked(object sender, EventArgs e)
        {
            if (BackupQuote != null)
            {
                QuoteItem = BackupQuote;
                BackupQuote = null;
                AutorItem = _databaseManager.GetAutorByQuote(QuoteItem);
                ThemeItem = _databaseManager.GetThemeByQuote(QuoteItem);
                OnPropertyChanged("");
                return;
            }

            await Navigation.PopAsync();
        }
        
        private void ButtonNextQuote_OnClicked(object sender, EventArgs e)
        {
            GetNextQuote();
        }

        
        private void ShareButton_OnTapped(object sender, EventArgs e)
        {
            //new Thread(() =>
            //{
            //    ShareButtonImage.Source = PersistentProperties.Instance.NightModeActivated
            //        ? "icon_link_day.png" : "icon_link_night.png";
            //    Thread.Sleep(250);
            //    ShareButtonImage.Source = PersistentProperties.Instance.NightModeActivated
            //        ? "icon_link_night.png" : "icon_link_day.png";
            //}).Start();

            SKBitmap imageSource = GetImageToExport();
            QuoteAppUtils.SKBitmap = imageSource;
            _background.InvalidateSurface();
            //DependencyService.Get<IShareService>()
            //    .Share("First try", "Zero deaths. Bring it on.", GetImageToExport());
        }
        
        #endregion

        private void GetNextQuote()
        {
            switch (_quoteSource)
            {
                case EnQuoteSource.Autor:
                    QuoteItem = _databaseManager.GetNextQuoteByAutor(AutorItem);
                    break;
                case EnQuoteSource.Theme:
                    QuoteItem = _databaseManager.GetNextQuoteByTheme(ThemeItem);
                    break;
                case EnQuoteSource.Random:
                    QuoteItem = _databaseManager.GetRandomQuote();
                    break;
            }

            AutorItem = _databaseManager.GetAutorByQuote(QuoteItem);
            ThemeItem = _databaseManager.GetThemeByQuote(QuoteItem);
            OnPropertyChanged("");
        }

        private SKBitmap GetImageToExport()
        {
            string colorHex = PersistentProperties.Instance.NightModeActivated ? ThemeItem.NightTextColor : ThemeItem.DayTextColor;

            string imageName = PersistentProperties.Instance.NightModeActivated
                ? "BackgroundImageNight.png" : "BackgroundImageDay.png";
            string folderSpace = "FrontEnd.Resources.ResourcesRaw.Icons";
            SKBitmap background = BitmapExtensions.LoadBitmapResource(this.GetType(),
                $"QuoteApp.{folderSpace}.{imageName}");

            var image = ImageGenerator.GenerateImageWithQuote(
                background,
                QuoteItem.Text,
                AutorItem.FullName,
                colorHex
            );

            return image;
        }
    }
}