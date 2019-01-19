using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using QuoteApp.Backend.BusinessLogic.Manager;
using QuoteApp.Backend.BusinessLogic.Subsystem.PersistentProperties;
using QuoteApp.Backend.Model;
using QuoteApp.FrontEnd.View.ItemView;
using QuoteApp.Globals;
using SkiaSharp;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace QuoteApp.FrontEnd.View.ListView
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ThemeListView : ContentPage
    {
        private DatabaseManager DatabaseManager { get; }
        private int _selectionIndex = 0;
        private int _numberOfSelectedItems = 20;
        private string _searchText;

        public Dictionary<string, Theme> Themes { get; set; }

        public Dictionary<string, Theme> FilteredThemes {get; private set; }

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                _selectionIndex = 0;
                FilteredThemes = string.IsNullOrWhiteSpace(_searchText)? Themes : Themes.Where(FilterCondition).ToDictionary(x => x.Key, x => x.Value);
                OnPropertyChanged(nameof(ShownThemes));
                OnPropertyChanged(nameof(ListAlphabetIndex));
                OnPropertyChanged(nameof(ListNumberIndex));
                OnPropertyChanged(nameof(ListNumberIndexIsVisible));
            }
        }

        public string ListAlphabetIndex
        {
            get
            {
                if (ShownThemes.Count <= 1) return "";

                string start = "", end = "";
                int length = 0;

                while (start == end)
                {
                    length++;

                    start = ShownThemes.First().Name.Substring(0, length);
                    end = ShownThemes.Last().Name.Substring(0, length);
                }

                return start + " - " + end;
            }
        }

        public string ListNumberIndex => FilteredThemes.Count + "/" + Themes.Count;
        public bool ListNumberIndexIsVisible => FilteredThemes.Count != Themes.Count;

        #region Getter Properties

        public ObservableCollection<Theme> ShownThemes => FilteredThemes.Count == 0
            ? new ObservableCollection<Theme>()
            : new ObservableCollection<Theme>(FilteredThemes.Values.Skip(_selectionIndex * _numberOfSelectedItems)
                .Take(_numberOfSelectedItems));

        public bool CanSelectPreviousItems => _selectionIndex > 0;
        public bool CanSelectNextItems => (_selectionIndex + 1) * _numberOfSelectedItems < FilteredThemes.Count;
        public bool CanNavigate => CanSelectNextItems || CanSelectPreviousItems;

        public int HeaderTextSize => QuoteAppUtils.PxToPt(App.ScreenHeight/25);
        public int ThemeItemTextSize => QuoteAppUtils.PxToPt(App.ScreenHeight/50);
        public int ButtonTextSize => QuoteAppUtils.PxToPt(App.ScreenHeight/40);
        public int AlphabetNavigationTextSize => QuoteAppUtils.PxToPt(App.ScreenHeight/45);
        public int NumberNavigationTextSize => QuoteAppUtils.PxToPt(App.ScreenHeight/60);

        public int ThemeItemHeight => QuoteAppUtils.PxToPt(App.ScreenHeight/25);

        public Color LineColor => PersistentProperties.Instance.NightModeActivated
            ? Color.FromHex(QuoteAppConstants.DefaultNightLineColor)
            : Color.FromHex(QuoteAppConstants.DefaultDayLineColor);
        public Color TextColor => PersistentProperties.Instance.NightModeActivated
            ? Color.FromHex(QuoteAppConstants.DefaultNightTextColor)
            : Color.FromHex(QuoteAppConstants.DefaultDayTextColor);
        public Color GrayedOutTextColor => PersistentProperties.Instance.NightModeActivated
            ? Color.FromHex(QuoteAppConstants.DefaultNightGrayedOutTextColor)
            : Color.FromHex(QuoteAppConstants.DefaultDayGrayedOutTextColor);
        
        #endregion

        public ThemeListView()
        {
            DatabaseManager = DatabaseManager.Instance;
            InitializeDefaultValues();
            RetrieveDependencies();

            InitializeComponent();
        }

        private void InitializeDefaultValues()
        {
        }

        private void RetrieveDependencies()
        {
            Themes = DatabaseManager.GetThemesList().ToDictionary(x => x.Key, x => x.Value);
            FilteredThemes = Themes;
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
        
        protected override void OnAppearing()
        {
            RetrieveDependencies();
            SetPageContent();
            UpdatePage();
        }

        private async void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
        {
            var itemBackgroundColor = Color.FromHex(PersistentProperties.Instance.NightModeActivated
                ? QuoteAppConstants.DefaultNightListItemSelectionBackgroundColor
                : QuoteAppConstants.DefaultDayListItemSelectionBackgroundColor);

            var entity =((Grid)sender);
            entity.BackgroundColor = itemBackgroundColor;
            var item = (Theme) entity.BindingContext;
            var view = new QuoteItemView();
            view.SetTheme(item);

            await Navigation.PushAsync(view);

            ThemesView.SelectedItem = null;
        }

        private async void ButtonBack_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        
        private void SelectPreviousItems_OnTapped(object sender, EventArgs e)
        {
            if (!CanSelectPreviousItems) return;

            _selectionIndex--;

            ScrollView.ScrollToAsync(0, 0, true);
            new Thread(() => 
            {
                Thread.CurrentThread.IsBackground = true;

                SelectNextItems.Opacity = CanSelectNextItems? 1 : 0;
                SelectPreviousItems.Opacity = 0.4;
                Thread.Sleep(250);
                SelectPreviousItems.Opacity = CanSelectPreviousItems? 1 : 0;
            }).Start();
            
            OnPropertyChanged(nameof(ShownThemes));
            OnPropertyChanged(nameof(CanSelectNextItems));
            OnPropertyChanged(nameof(CanSelectPreviousItems));
            OnPropertyChanged(nameof(ListAlphabetIndex));
            OnPropertyChanged(nameof(ListNumberIndex));
            OnPropertyChanged(nameof(ListNumberIndexIsVisible));
        }

        private void SelectNextItems_OnTapped(object sender, EventArgs e)
        {
            if (!CanSelectNextItems) return;

            _selectionIndex++;

            ScrollView.ScrollToAsync(0, 0, true);
            new Thread(() => 
            {
                Thread.CurrentThread.IsBackground = true;

                SelectPreviousItems.Opacity = CanSelectPreviousItems? 1 : 0;
                SelectNextItems.Opacity = 0.4;
                Thread.Sleep(250);
                SelectNextItems.Opacity = CanSelectNextItems? 1 : 0;
            }).Start();
            
            OnPropertyChanged(nameof(ShownThemes));
            OnPropertyChanged(nameof(CanSelectNextItems));
            OnPropertyChanged(nameof(CanSelectPreviousItems));
            OnPropertyChanged(nameof(ListAlphabetIndex));
            OnPropertyChanged(nameof(ListNumberIndex));
            OnPropertyChanged(nameof(ListNumberIndexIsVisible));
        }

        #endregion

        private bool FilterCondition(KeyValuePair<string, Theme> fullName)
        {
            return fullName.Key.ToUpper().Contains(SearchText.ToUpper());
        }
        
        private void UpdatePage()
        {
            SelectPreviousItems.Opacity = CanSelectPreviousItems ? 1 : 0;
            SelectNextItems.Opacity = CanSelectNextItems ? 1 : 0;
            OnPropertyChanged(string.Empty);
        }
    }
}
