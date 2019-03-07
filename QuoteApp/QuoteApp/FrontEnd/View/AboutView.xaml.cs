using System;
using System.Collections.Generic;
using System.Linq;
using QuoteApp.Backend.BusinessLogic.Subsystem.PersistentProperties;
using QuoteApp.Backend.Model;
using QuoteApp.Globals;
using SkiaSharp;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace QuoteApp.FrontEnd.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AboutView : ContentPage
    {
        public List<ThemeColor> ThemeDayBackgroundColorItems { get; set; }
        public List<ThemeColor> ThemeNightBackgroundColorItems { get; set; }
        
        #region Getter Properties

        public int TitleTextSize => QuoteAppUtils.PxToPt(App.ScreenHeight / 25);
        public int ContentTextSize => QuoteAppUtils.PxToPt(App.ScreenHeight / 50);
        public int ButtonTextSize => QuoteAppUtils.PxToPt(QuoteAppConstants.DefaultButtonTextSize);
        
        public Color LineColor => PersistentProperties.Instance.NightModeActivated
            ? Color.FromHex(QuoteAppConstants.DefaultNightLineColor)
            : Color.FromHex(QuoteAppConstants.DefaultDayLineColor);
        public Color TextColor => PersistentProperties.Instance.NightModeActivated
            ? Color.FromHex(QuoteAppConstants.DefaultNightTextColor)
            : Color.FromHex(QuoteAppConstants.DefaultDayTextColor);

        #endregion

        public AboutView()
        {
            InitializeDefaultValues();

            InitializeComponent();
        }
        
        #region Initialization

        private void InitializeDefaultValues()
        {
            ThemeDayBackgroundColorItems = QuoteAppConstants.DefaultDayBackgroundColorGradientItems;
            ThemeNightBackgroundColorItems = QuoteAppConstants.DefaultNightBackgroundColorGradientItems;
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
        
        protected override void OnAppearing()
        {
            SetPageContent();
            OnPropertyChanged("");
        }
        
        private async void ButtonBack_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
        
        #endregion
        
    }
}