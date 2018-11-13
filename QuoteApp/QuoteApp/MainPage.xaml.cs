using System;
using QuoteApp.Backend.BusinessLogic.Subsystem.PersistentProperties;
using QuoteApp.FrontEnd.View;
using QuoteApp.FrontEnd.View.ItemView;
using Xamarin.Forms;

namespace QuoteApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        private async void RotationSliderPageNavigationButton_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RotationSliderPage());
        }

        private async void QuoteItemDayPageNavigationButton_OnClicked(object sender, EventArgs e)
        {
            PersistentProperties.Instance.NightModeActivated = false;
            await Navigation.PushAsync(new QuoteItemView());
        }

        private async void QuoteItemNightPageNavigationButton_OnClicked(object sender, EventArgs e)
        {
            PersistentProperties.Instance.NightModeActivated = true;
            await Navigation.PushAsync(new QuoteItemView());
        }

        private void ThemeListDayPageNavigationButton_OnClicked(object sender, EventArgs e)
        {
            PersistentProperties.Instance.NightModeActivated = false;

        }

        private void ThemeListNightPageNavigationButton_OnClicked(object sender, EventArgs e)
        {
            PersistentProperties.Instance.NightModeActivated = true;

        }

        private void AutorListDayPageNavigationButton_OnClicked(object sender, EventArgs e)
        {
            PersistentProperties.Instance.NightModeActivated = false;

        }

        private void AutorListNightPageNavigationButton_OnClicked(object sender, EventArgs e)
        {
            PersistentProperties.Instance.NightModeActivated = true;

        }

        private void SettingsDayPageNavigationButton_OnClicked(object sender, EventArgs e)
        {
            PersistentProperties.Instance.NightModeActivated = false;

        }

        private void SettingsNightPageNavigationButton_OnClicked(object sender, EventArgs e)
        {
            PersistentProperties.Instance.NightModeActivated = true;

        }
    }
}
