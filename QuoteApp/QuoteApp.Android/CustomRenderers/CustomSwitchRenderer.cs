using System.Linq;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Widget;
using QuoteApp.Backend.BusinessLogic.Subsystem.PersistentProperties;
using QuoteApp.Droid.CustomRenderers;
using QuoteApp.FrontEnd.Resources.CustomControls;
using QuoteApp.Globals;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Color = Xamarin.Forms.Color;

[assembly: ExportRenderer(typeof(CustomSwitch), typeof(CustomSwitchRenderer))]
namespace QuoteApp.Droid.CustomRenderers
{
    public class CustomSwitchRenderer : SwitchRenderer
    {
        private CustomSwitch _view;

        public CustomSwitchRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Switch> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement != null || e.NewElement == null)
                return;
            _view = (CustomSwitch)Element;
            if (Android.OS.Build.VERSION.SdkInt < Android.OS.BuildVersionCodes.JellyBean) return;
            if (this.Control == null) return;
            
            Control.Checked = _view.IsToggled;

            this.Control.TrackDrawable.SetColorFilter(this.Control.Checked 
                ? _view.SwitchOnColor.ToAndroid() 
                : _view.SwitchOffColor.ToAndroid(), PorterDuff.Mode.SrcAtop);

            this.Control.CheckedChange += this.OnCheckedChange;
            UpdateSwitchThumbImage(_view);
        }

        private void UpdateSwitchThumbImage(CustomSwitch view)  
        {  
            if (!string.IsNullOrEmpty(view.SwitchThumbImage))  
            {  
                view.SwitchThumbImage = view.SwitchThumbImage.Replace(".jpg", "").Replace(".png", "");  
                int imgid = (int)typeof(Resource.Drawable).GetField(view.SwitchThumbImage).GetValue(null);  
                Control.SetThumbResource(Resource.Drawable.icon_switch_thumb);
            }  
            else  
            {  
                Control.ThumbDrawable.SetColorFilter(view.SwitchThumbColor.ToAndroid(), PorterDuff.Mode.Multiply);  
                // Control.SetTrackResource(Resource.Drawable.track);  
            }  
        }

        private void OnCheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            _view.IsToggled = Control.Checked;
            this.Control.TrackDrawable.SetColorFilter(
                this.Control.Checked ? _view.SwitchOnColor.ToAndroid() : _view.SwitchOffColor.ToAndroid(),
                PorterDuff.Mode.SrcAtop);
        }
        protected override void Dispose(bool disposing)
        {
            this.Control.CheckedChange -= this.OnCheckedChange;
            base.Dispose(disposing);
        }
    }
}