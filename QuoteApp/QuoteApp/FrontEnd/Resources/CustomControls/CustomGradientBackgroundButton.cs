using QuoteApp.Backend.BusinessLogic.Subsystem.PersistentProperties;
using QuoteApp.Globals;
using Xamarin.Forms;

namespace QuoteApp.FrontEnd.Resources.CustomControls
{
    public class CustomGradientBackgroundButton : Button
    {
        public static readonly BindableProperty StartColorProperty =
            BindableProperty.Create(nameof(StartColor),
                typeof(Color), typeof(CustomGradientBackgroundButton), Color.Default);

        public Color StartColor
        {
            get => IsPressed? Color.Transparent : (Color)GetValue(StartColorProperty);
            set => SetValue(StartColorProperty, value);
        }

        public static readonly BindableProperty EndColorProperty =
            BindableProperty.Create(nameof(EndColor),
                typeof(Color), typeof(CustomGradientBackgroundButton), Color.Default);

        public Color EndColor
        {
            get => IsPressed? Color.Transparent : (Color)GetValue(EndColorProperty);
            set => SetValue(EndColorProperty, value);
        }

        public new bool IsPressed { get; set; }

        public CustomGradientBackgroundButton()
        {
            PersistentProperties.Instance.NightModeSwitched += (sender, e) => { SetColors(); };

            SetColors();
        }

        private void SetColors()
        {
            BackgroundColor = Color.Transparent;

            var isNightMode = PersistentProperties.Instance.NightModeActivated;

            StartColor = Color.FromHex(isNightMode
                ? QuoteAppConstants.DefaultNightButtonBackgroundStartColor
                : QuoteAppConstants.DefaultDayButtonBackgroundStartColor);

            EndColor = Color.FromHex(isNightMode
                ? QuoteAppConstants.DefaultNightButtonBackgroundEndColor
                : QuoteAppConstants.DefaultDayButtonBackgroundEndColor);
        }
    }
}
