using System;
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
            MyImage.Source = Device.RuntimePlatform == Device.Android 
                ? ImageSource.FromFile("quote_item_header.png") 
                : ImageSource.FromFile("Images/quote_item_header.png");
        }

        private async void RotationSliderPageNavigationButton_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RotationSliderPage());
        }

        private async void QuoteItemPageNavigationButton_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new QuoteItemView());
        }
    }
}
