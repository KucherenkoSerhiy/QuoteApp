using Android.Content;
using Android.Graphics;
using Android.Widget;
using QuoteApp.Droid.CustomRenders;
using QuoteApp.FrontEnd.Resources.CustomControls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Color = Android.Graphics.Color;
using Switch = Xamarin.Forms.Switch;

[assembly: ExportRenderer(typeof(Switch), typeof(CustomSwitchRenderer))]
namespace QuoteApp.Droid.CustomRenders
{
    public class CustomSwitchRenderer : SwitchRenderer
    {
        private Color _offColor = new Color(215, 218, 220);
        private Color _onColor = new Color(32, 156, 68);

        public CustomSwitchRenderer(Context context) : base(context)
        {
        }

        protected override void Dispose(bool disposing)
        {
            Control.CheckedChange -= OnCheckedChange;
            base.Dispose(disposing);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Switch> e)
        {
            base.OnElementChanged(e);

            if (Control == null) return;

            var customSwich = (CustomSwitch) e.NewElement;
            _offColor = customSwich.SwitchOffColor.ToAndroid();
            _onColor = customSwich.SwitchOnColor.ToAndroid();

            Control.ThumbDrawable.SetColorFilter(Control.Checked 
                ? _onColor : _offColor, PorterDuff.Mode.SrcAtop);

            Control.CheckedChange += OnCheckedChange;
        }

        private void OnCheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            Control.ThumbDrawable.SetColorFilter(Control.Checked 
                ? _onColor : _offColor, PorterDuff.Mode.SrcAtop);
        }
    }
}