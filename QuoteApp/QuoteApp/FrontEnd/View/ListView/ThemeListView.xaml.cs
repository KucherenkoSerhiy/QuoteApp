using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using QuoteApp.Backend.BusinessLogic.Manager;
using QuoteApp.Backend.BusinessLogic.Subsystem.PersistentProperties;
using QuoteApp.Backend.Model;
using QuoteApp.Globals;
using SkiaSharp;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace QuoteApp.FrontEnd.View.ListView
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ThemeListView : ContentPage
    {
        public ObservableCollection<Theme> Themes { get; set; }
        private DatabaseManager DatabaseManager { get; }

        #region Getter Properties

        public int HeaderTextSize => QuoteAppUtils.PxToPt(App.ScreenHeight/25);
        public int ThemeItemTextSize => QuoteAppUtils.PxToPt(App.ScreenHeight/50);
        public int ButtonTextSize => QuoteAppUtils.PxToPt(App.ScreenHeight/40);

        public int ThemeItemHeight => QuoteAppUtils.PxToPt(App.ScreenHeight/25);

        public Color LineColor => PersistentProperties.Instance.NightModeActivated
            ? Color.FromHex(QuoteAppConstants.DefaultNightLineColor)
            : Color.FromHex(QuoteAppConstants.DefaultDayLineColor);

        public Color TextColor => PersistentProperties.Instance.NightModeActivated
            ? Color.FromHex(QuoteAppConstants.DefaultNightTextColor)
            : Color.FromHex(QuoteAppConstants.DefaultDayTextColor);
        
        #endregion

        public ThemeListView()
        {
            DatabaseManager = DatabaseManager.Instance;
            InitializeDefaultValues();
            RetrieveDependencies();

            InitializeComponent();
            SetPageContent();
        }

        private void InitializeDefaultValues()
        {
            
        }

        private void RetrieveDependencies()
        {
            //Themes = new ObservableCollection<Theme>(DatabaseManager.Themes);
            
            //Themes = new ObservableCollection<Theme>
            //{
            //    new Theme{Name = "Marilyn Monroe"},
            //    new Theme{Name = "Harold Abelson"},
            //    new Theme{Name = "Napoleon Bonaparte"},
            //    new Theme{Name = "Friedrich Nietsche"},
            //    new Theme{Name = "A person with a very very long first name or last name"},
            //    new Theme{Name = "Cat"}
            //};
        }

        private void SetPageContent()
        {
            NavigationPage.SetHasNavigationBar(this, false);

            var dayBackgroundColorGradientItems = QuoteAppConstants.DefaultDayBackgroundColorGradientItems;
            var nightBackgroundColorGradientItems = QuoteAppConstants.DefaultNightBackgroundColorGradientItems;

            SKColor[] themeColors = PersistentProperties.Instance.NightModeActivated
                ? nightBackgroundColorGradientItems.Select(x => SKColor.Parse(x.ColorCode)).ToArray()
                : dayBackgroundColorGradientItems.Select(x => SKColor.Parse(x.ColorCode)).ToArray();

            float[] gradientPositions = PersistentProperties.Instance.NightModeActivated
                ? nightBackgroundColorGradientItems.Select(x => x.GradientPosition).ToArray()
                : dayBackgroundColorGradientItems.Select(x => x.GradientPosition).ToArray();

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

        #region UI Event Handling

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            await DisplayAlert("Item Tapped", "An item was tapped.", "OK");

            //Deselect Item
            ((Xamarin.Forms.ListView)sender).SelectedItem = null;
        }

        private async void ButtonBack_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        #endregion
    }
}
