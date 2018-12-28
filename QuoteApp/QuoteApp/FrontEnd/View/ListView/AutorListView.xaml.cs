using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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
    public partial class AutorListView : ContentPage
    {
        private DatabaseManager DatabaseManager { get; }
        private int _selectionIndex = 0;
        private int _numberOfSelectedItems = 20;
        private string _searchText;

        public Dictionary<string, Autor> Autors { get; set; }

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                _selectionIndex = 0;
                FilteredAutors = Autors.Where(FilterCondition).ToDictionary(x => x.Key, x => x.Value);
                OnPropertyChanged(nameof(ShownAutors));
                OnPropertyChanged(nameof(ListAlphabetIndex));
                OnPropertyChanged(nameof(ListNumberIndex));
                OnPropertyChanged(nameof(ListNumberIndexIsVisible));
            }
        }

        #region Getter Properties

        public Dictionary<string, Autor> FilteredAutors {get; private set; }

        public ObservableCollection<Autor> ShownAutors => FilteredAutors.Count == 0
            ? new ObservableCollection<Autor>()
            : new ObservableCollection<Autor>(FilteredAutors.Values.Skip(_selectionIndex * _numberOfSelectedItems)
                .Take(_numberOfSelectedItems));

        
        public string ListAlphabetIndex
        {
            get
            {
                if (ShownAutors.Count <= 1) return "";

                string start = "", end = "";
                int length = 0;

                while (start == end)
                {
                    length++;

                    start = ShownAutors.First().FullName.Substring(0, length);
                    end = ShownAutors.Last().FullName.Substring(0, length);
                }

                return start + " - " + end;
            }
        }

        public string ListNumberIndex => FilteredAutors.Count + "/" + Autors.Count;
        public bool ListNumberIndexIsVisible => FilteredAutors.Count != Autors.Count;


        public int HeaderTextSize => QuoteAppUtils.PxToPt(App.ScreenHeight/25);
        public int AutorItemTextSize => QuoteAppUtils.PxToPt(App.ScreenHeight/50);
        public int ButtonTextSize => QuoteAppUtils.PxToPt(App.ScreenHeight/40);
        public int AlphabetNavigationTextSize => QuoteAppUtils.PxToPt(App.ScreenHeight/45);
        public int NumberNavigationTextSize => QuoteAppUtils.PxToPt(App.ScreenHeight/60);

        public int AutorItemHeight => QuoteAppUtils.PxToPt(App.ScreenHeight/25);

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

        public AutorListView()
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
            Autors = DatabaseManager.Autors.ToDictionary(x => x.Key, x => x.Value);
            FilteredAutors = Autors;
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
            SetPageContent();
            OnPropertyChanged("");
        }
        
        private async void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
        {
            var itemBackgroundColor = Color.FromHex(PersistentProperties.Instance.NightModeActivated
                ? QuoteAppConstants.DefaultNightListItemSelectionBackgroundColor
                : QuoteAppConstants.DefaultDayListItemSelectionBackgroundColor);

            var entity =((Grid)sender);
            entity.BackgroundColor = itemBackgroundColor;
            var item = (Autor) entity.BindingContext;
            var view = new QuoteItemView();
            view.SetAutor(item);

            await Navigation.PushAsync(view);

            AutorsView.SelectedItem = null;
        }

        private async void ButtonBack_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        
        private void SelectPreviousItems_OnTapped(object sender, EventArgs e)
        {
            if (_selectionIndex <= 0) return;

            _selectionIndex--;
            
            OnPropertyChanged(nameof(ShownAutors));
            OnPropertyChanged(nameof(ListAlphabetIndex));
            OnPropertyChanged(nameof(ListNumberIndex));
            OnPropertyChanged(nameof(ListNumberIndexIsVisible));
        }

        private void SelectNextItems_OnTapped(object sender, EventArgs e)
        {
            if ((_selectionIndex + 1) * _numberOfSelectedItems >= FilteredAutors.Count) return;

            _selectionIndex++;
            
            OnPropertyChanged(nameof(ShownAutors));
            OnPropertyChanged(nameof(ListAlphabetIndex));
            OnPropertyChanged(nameof(ListNumberIndex));
            OnPropertyChanged(nameof(ListNumberIndexIsVisible));
        }

        #endregion

        private bool FilterCondition(KeyValuePair<string, Autor> fullName)
        {
            string[] nameParts = fullName.Key.Split(' ');

            return nameParts.Any(x => x.ToUpper().StartsWith(SearchText == null ? "" : SearchText.ToUpper()));
        }
    }
}
