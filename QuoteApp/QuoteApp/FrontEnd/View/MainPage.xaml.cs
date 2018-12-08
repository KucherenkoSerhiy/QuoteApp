using System;
using System.Collections.Generic;
using System.Linq;
using QuoteApp.Backend.BusinessLogic.Manager;
using QuoteApp.Backend.BusinessLogic.Subsystem.PersistentProperties;
using QuoteApp.Backend.Model;
using QuoteApp.FrontEnd.View.ItemView;
using QuoteApp.FrontEnd.View.ListView;
using QuoteApp.Globals;
using SkiaSharp;
using Xamarin.Forms;

namespace QuoteApp.FrontEnd.View
{
    // TODO: Search for the most efficient way to change views
    // TODO: Bindings not working
    public partial class MainPage : ContentPage
    {
        public List<ThemeColor> ThemeDayBackgroundColorItems { get; set; }
        public List<ThemeColor> ThemeNightBackgroundColorItems { get; set; }

        #region Getter Properties

        public int GreetingTextSize => QuoteAppUtils.PxToPt(App.ScreenHeight / 25);
        public int BriefTextSize => QuoteAppUtils.PxToPt(App.ScreenHeight / 50);
        public int ButtonTextSize => QuoteAppUtils.PxToPt(App.ScreenHeight / 40);
        
        public Color LineColor => PersistentProperties.Instance.NightModeActivated
            ? Color.FromHex(QuoteAppConstants.DefaultNightLineColor)
            : Color.FromHex(QuoteAppConstants.DefaultDayLineColor);
        public Color TextColor => PersistentProperties.Instance.NightModeActivated
            ? Color.FromHex(QuoteAppConstants.DefaultNightTextColor)
            : Color.FromHex(QuoteAppConstants.DefaultDayTextColor);

        #endregion

        public MainPage()
        {
            InitializeDefaultValues();
            RetrieveDependencies();

            InitializeComponent();
            SetPageContent();

        }

        #region Initialization

        
        private void InitializeDefaultValues()
        {
            ThemeDayBackgroundColorItems = QuoteAppConstants.DefaultDayBackgroundColorGradientItems;
            ThemeNightBackgroundColorItems = QuoteAppConstants.DefaultNightBackgroundColorGradientItems;
        }

        private void RetrieveDependencies()
        {

        }

        private void SetPageContent()
        {
            NavigationPage.SetHasNavigationBar(this, false);

            SKColor[] themeColors = PersistentProperties.Instance.NightModeActivated
                ? ThemeNightBackgroundColorItems.Select(x => SKColor.Parse(x.ColorCode)).ToArray()
                : ThemeDayBackgroundColorItems.Select(x => SKColor.Parse(x.ColorCode)).ToArray();

            float[] gradientPositions = PersistentProperties.Instance.NightModeActivated
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
        
        private async void ViewThemesButton_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ThemeListView());
        }

        private async void ViewAutorsButton_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AutorListView());
        }

        private async void ViewRandomQuoteButton_OnClicked(object sender, EventArgs e)
        {
            var view = new QuoteItemView();
            view.SetQuote(DatabaseManager.Instance.GetRandomQuote());

            await Navigation.PushAsync(view);
        }

        private async void ViewSettingsButton_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SettingsView());
        }

        #endregion
    }
}
