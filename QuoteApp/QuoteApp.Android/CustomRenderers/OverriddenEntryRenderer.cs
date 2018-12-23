using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using QuoteApp.Backend.BusinessLogic.Subsystem.PersistentProperties;
using QuoteApp.Droid.CustomRenderers;
using QuoteApp.Globals;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Color = Xamarin.Forms.Color;

[assembly: ExportRenderer(typeof(Entry), typeof(OverriddenEntryRenderer))]
namespace QuoteApp.Droid.CustomRenderers
{
    public class OverriddenEntryRenderer : EntryRenderer
    {
        public OverriddenEntryRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            var color = Color.FromHex(PersistentProperties.Instance.NightModeActivated
                ? QuoteAppConstants.DefaultNightLineColor
                : QuoteAppConstants.DefaultDayLineColor).ToAndroid();

            if (Control == null || e.NewElement == null) return;

            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
                Control.BackgroundTintList = ColorStateList.ValueOf(color);
            else
                Control.Background.SetColorFilter(color, PorterDuff.Mode.SrcAtop);
        }    
    }
}