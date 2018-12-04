using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using QuoteApp.Backend.BusinessLogic.Manager;
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
        private DatabaseManager _databaseManager;

        private int _quoteIndex;

        public Autor AutorItem { get; set; }
        public ObservableCollection<Quote> QuoteItems { get; set; }
        public Theme ThemeItem { get; set; }

        public List<ThemeColor> ThemeDayBackgroundColorItems { get; set; }
        public List<ThemeColor> ThemeNightBackgroundColorItems { get; set; }
        
        #region Getter Properties

        public Quote QuoteItem => QuoteItems.ElementAt(_quoteIndex);

        public int QuoteTextSize => QuoteAppUtils.PxToPt(App.ScreenHeight/50);
        public int ThemeTextSize => QuoteAppUtils.PxToPt(App.ScreenHeight/25);
        public int ButtonTextSize => QuoteAppUtils.PxToPt(App.ScreenHeight/40);

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
            RetrieveDependencies();

            InitializeComponent();
            SetPageContent();
        }
        /*
        public QuoteItemView(Theme theme)
        {
            InitializeDefaultValues();
            RetrieveDependencies(theme);

            InitializeComponent();
            SetPageContent();
        }

        public QuoteItemView(Autor autor)
        {
            InitializeDefaultValues();
            RetrieveDependencies(autor);

            InitializeComponent();
            SetPageContent();
        }
        */

        #region Initialization

        private void InitializeDefaultValues()
        {
            _databaseManager = DatabaseManager.Instance;
            ThemeDayBackgroundColorItems = QuoteAppConstants.DefaultDayBackgroundColorGradientItems;
            ThemeNightBackgroundColorItems = QuoteAppConstants.DefaultNightBackgroundColorGradientItems;
        }

        private void RetrieveDependencies()
        {
            AutorItem = new Autor { FullName = "Indecisive anonymous" };
            QuoteItems = new ObservableCollection<Quote>
            {
                new Quote { Text = "This is the first dummy quote." },
                new Quote { Text = "I used to think I was indecisive, but now I'm not too sure." },
                new Quote { Text = "A dummy quote again." }
            };
            ThemeItem = new Theme
            {
                Name = "Life",
                Id = 9000,
                DayLineColor = QuoteAppConstants.DefaultDayLineColor,
                DayTextColor = QuoteAppConstants.DefaultDayTextColor,
                NightLineColor = QuoteAppConstants.DefaultNightLineColor,
                NightTextColor = QuoteAppConstants.DefaultNightTextColor
            };
        }

        private void RetrieveDependencies(Autor autor)
        {
            AutorItem = autor;
            QuoteItems = new ObservableCollection<Quote>(_databaseManager.GetQuotesByAutor(autor));
            ThemeItem = _databaseManager.GetThemeByAutor(autor);
        }

        private void RetrieveDependencies(Theme theme)
        {
            AutorItem = _databaseManager.GetAutorByTheme(theme);
            QuoteItems = new ObservableCollection<Quote>(_databaseManager.GetQuotesByTheme(theme));
            ThemeItem = theme;
        }

        private void SetPageContent()
        {
            NavigationPage.SetHasNavigationBar(this, false);

            SKColor[] themeColors = PersistentProperties.Instance.NightModeActivated
                ? ThemeNightBackgroundColorItems.Select(x => SKColor.Parse(x.ColorCode)).ToArray()
                : ThemeDayBackgroundColorItems.Select(x => SKColor.Parse(x.ColorCode)).ToArray();

            float[] gradientPositions =  PersistentProperties.Instance.NightModeActivated
                ? ThemeNightBackgroundColorItems.Select(x => x.GradientPosition).ToArray()
                : ThemeDayBackgroundColorItems.Select(x => x.GradientPosition).ToArray();

            var background = QuoteAppUtils.CreateGradientBackground(themeColors, gradientPositions);

            Content = new AbsoluteLayout
            {
                Children =
                {
                    {background, new Rectangle(0, 0, 1, 1), AbsoluteLayoutFlags.All},
                    {ContentRoot, new Rectangle(0, 0, 1, 1), AbsoluteLayoutFlags.All}
                }
            };
        }

        #endregion

        #region UI Event Handling


        // clicking autor label should go to autor list

        /// <summary>
        /// TODO: goes to theme list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ThemeItemNameButton_OnClicked(object sender, EventArgs e)
        {
            
        }

        /// <summary>
        /// TODO: Previous quote until the first one viewed here or if no more pops back in the navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ButtonBack_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        /// <summary>
        /// TODO: Selects next quote from list. If no more, changes autor or offers tochange theme
        /// TODO: maybe replace buttons from view by sliding to like/dislike (like in tinder) 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ButtonNextQuote_OnClicked(object sender, EventArgs e)
        {
            if (_quoteIndex < QuoteItems.Count)
            {
                _quoteIndex++;
                OnPropertyChanged(string.Empty);
            }
            else
            {
                // TODO: select next autor/theme
            }
        }

        #endregion
    }
}