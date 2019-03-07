using System;
using System.Collections.Generic;
using System.Linq;
using QuoteApp.Backend.BusinessLogic.Manager;
using QuoteApp.Backend.BusinessLogic.Subsystem.CloseApplication;
using QuoteApp.Backend.BusinessLogic.Subsystem.PersistentProperties;
using QuoteApp.Backend.Model;
using QuoteApp.FrontEnd.View.ItemView;
using QuoteApp.FrontEnd.View.ListView;
using QuoteApp.Globals;
using SkiaSharp;
using Xamarin.Forms;

namespace QuoteApp.FrontEnd.View
{
    public partial class MainPage : ContentPage
    {
        public List<ThemeColor> ThemeDayBackgroundColorItems { get; set; }
        public List<ThemeColor> ThemeNightBackgroundColorItems { get; set; }

        public bool FirstTimeEnteredMainMenu
        {
            get => PersistentProperties.Instance.FirstTimeEnteredMainMenu;
            set => PersistentProperties.Instance.FirstTimeEnteredMainMenu = value;
        }

        #region Getter Properties

        public int TitleTextSize => QuoteAppUtils.PxToPt(App.ScreenHeight / 25);
        public int BriefTextSize => QuoteAppUtils.PxToPt(App.ScreenHeight / 50);
        public int ButtonTextSize => QuoteAppUtils.PxToPt(QuoteAppConstants.DefaultButtonTextSize);
        
        public Color LineColor => PersistentProperties.Instance.NightModeActivated
            ? Color.FromHex(QuoteAppConstants.DefaultNightLineColor)
            : Color.FromHex(QuoteAppConstants.DefaultDayLineColor);
        public Color TextColor => PersistentProperties.Instance.NightModeActivated
            ? Color.FromHex(QuoteAppConstants.DefaultNightTextColor)
            : Color.FromHex(QuoteAppConstants.DefaultDayTextColor);

        public string MainMenuTitle => FirstTimeEnteredMainMenu ? "Welcome" : "Main Menu";

        #endregion

        public MainPage()
        {
            InitializeDefaultValues();
            RetrieveDependencies();

            InitializeComponent();
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
        
        protected override void OnAppearing()
        {
            SetPageContent();
            OnPropertyChanged("");
        }

        protected override void OnDisappearing()
        {
            if (FirstTimeEnteredMainMenu)
                FirstTimeEnteredMainMenu = false;
        }

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

        private void ExitButton_OnClicked(object sender, EventArgs e)
        {
            var closer = DependencyService.Get<ICloseApplication>();
            closer?.Close();
        }

        #endregion
    }
}
