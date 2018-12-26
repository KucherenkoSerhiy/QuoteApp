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

        public bool IsPressed { get; set; }
    }
}
