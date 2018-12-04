using System;
using System.Collections.Generic;
using System.Linq;
using QuoteApp.Backend.BusinessLogic.Subsystem.PersistentProperties;
using QuoteApp.Backend.Model;
using QuoteApp.FrontEnd.View;
using QuoteApp.FrontEnd.View.ItemView;
using QuoteApp.FrontEnd.View.ListView;
using QuoteApp.Globals;
using SkiaSharp;
using Xamarin.Forms;

namespace QuoteApp
{
    // TODO: Search for the most efficient way to change views
    // TODO: Bindings not working
    public partial class MainPage : ContentPage
    {
        public List<ThemeColor> ThemeDayBackgroundColorItems { get; set; }
        public List<ThemeColor> ThemeNightBackgroundColorItems { get; set; }

        public int GreetingTextSize { get; private set; }
        public int BriefTextSize { get; private set; }
        public int ButtonTextSize { get; private set; }

        public Color LineColor { get; private set; }
        public Color TextColor { get; private set; }

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

            GreetingTextSize = QuoteAppUtils.PxToPt(App.ScreenHeight / 25);
            BriefTextSize = QuoteAppUtils.PxToPt(App.ScreenHeight / 50);
            ButtonTextSize = QuoteAppUtils.PxToPt(App.ScreenHeight / 40);

            LineColor = PersistentProperties.Instance.NightModeActivated
                ? Color.FromHex(QuoteAppConstants.DefaultNightLineColor)
                : Color.FromHex(QuoteAppConstants.DefaultDayLineColor);

            TextColor = PersistentProperties.Instance.NightModeActivated
               ? Color.FromHex(QuoteAppConstants.DefaultNightTextColor)
               : Color.FromHex(QuoteAppConstants.DefaultDayTextColor);
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

        #region UI Events

        
        private async void QuoteItemDayPageNavigationButton_OnClicked(object sender, EventArgs e)
        {
            PersistentProperties.Instance.NightModeActivated = false;
            //await Navigation.PushAsync(new QuoteItemView());
        }

        private async void QuoteItemNightPageNavigationButton_OnClicked(object sender, EventArgs e)
        {
            PersistentProperties.Instance.NightModeActivated = true;
            //await Navigation.PushAsync(new QuoteItemView());
        }

        private async void ViewThemesButton_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ThemeListView());
        }

        private async void ViewAutorsButton_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AutorListView());
        }

        #endregion
    }
}
