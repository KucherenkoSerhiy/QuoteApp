using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuoteApp.Backend.BusinessLogic.Subsystem.PersistentProperties;
using QuoteApp.Backend.Model;
using QuoteApp.Globals;
using SkiaSharp;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace QuoteApp.FrontEnd.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SettingsView : ContentPage
	{
	    public bool OnlyUnreadQuotes
	    {
	        get => PersistentProperties.Instance.OnlyUnreadQuotes;
	        set => PersistentProperties.Instance.OnlyUnreadQuotes = value;
	    }

	    public List<ThemeColor> ThemeDayBackgroundColorItems { get; set; }
	    public List<ThemeColor> ThemeNightBackgroundColorItems { get; set; }

	    #region Getter Properties

	    public int TitleTextSize => QuoteAppUtils.PxToPt(App.ScreenHeight / 25);
	    public int ButtonTextSize => QuoteAppUtils.PxToPt(App.ScreenHeight / 40);
	    public int ToggleButtonTextSize => QuoteAppUtils.PxToPt(App.ScreenHeight / 50);
	    public int LabelTextSize => QuoteAppUtils.PxToPt(App.ScreenHeight / 60);
        
	    public Color LineColor => PersistentProperties.Instance.NightModeActivated
	        ? Color.FromHex(QuoteAppConstants.DefaultNightLineColor)
	        : Color.FromHex(QuoteAppConstants.DefaultDayLineColor);
	    public Color TextColor => PersistentProperties.Instance.NightModeActivated
	        ? Color.FromHex(QuoteAppConstants.DefaultNightTextColor)
	        : Color.FromHex(QuoteAppConstants.DefaultDayTextColor);

	    public Color OnSwitchColor => Color.FromHex(QuoteAppConstants.OnSwitchColor);

	    #endregion


		public SettingsView ()
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

	    private void SetDayModeButton_OnClicked(object sender, EventArgs e)
	    {
	        if (!PersistentProperties.Instance.NightModeActivated) return;

	        PersistentProperties.Instance.NightModeActivated = false;
	        SetPageContent();
	        OnPropertyChanged("");
	    }

	    private void SetNightModeButton_OnClicked(object sender, EventArgs e)
	    {
	        if (PersistentProperties.Instance.NightModeActivated) return;

	        PersistentProperties.Instance.NightModeActivated = true;
	        SetPageContent();
	        OnPropertyChanged("");
	    }

	    private void ViewAboutButton_OnClicked(object sender, EventArgs e)
	    {
	        // TODO: about view
	    }

	    private async void ButtonBack_OnClicked(object sender, EventArgs e)
	    {
	        await Navigation.PopAsync();
	    }

	    #endregion
	}
}