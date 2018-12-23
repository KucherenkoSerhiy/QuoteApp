using QuoteApp.FrontEnd.Resources.CustomControls;
using QuoteApp.iOS.CustomRenderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomSwitch), typeof(CustomSwitchRenderer))]
namespace QuoteApp.iOS.CustomRenderers
{
    class CustomSwitchRenderer : SwitchRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Switch> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || e.NewElement == null)  return;

            CustomSwitch s = Element as CustomSwitch;

            UISwitch sw = new UISwitch();
            //sw.ThumbTintColor = s.SwitchThumbColor.ToUIColor();
            sw.OnTintColor = s.SwitchOnColor.ToUIColor();

            SetNativeControl(sw);
        }
    }
}