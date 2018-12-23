using CoreAnimation;
using CoreGraphics;
using QuoteApp.Backend.BusinessLogic.Subsystem.PersistentProperties;
using QuoteApp.Globals;
using QuoteApp.iOS.CustomRenderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer (typeof(Entry), typeof(OverriddenEntryRenderer))]
namespace QuoteApp.iOS.CustomRenderers
{
    public class OverriddenEntryRenderer : EntryRenderer
    {
        private CALayer _line;

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged (e);

            var color = Color.FromHex(PersistentProperties.Instance.NightModeActivated
                ? QuoteAppConstants.DefaultNightLineColor
                : QuoteAppConstants.DefaultDayLineColor).ToUIColor().CGColor;


            _line = null;

            if (Control == null || e.NewElement == null)
                return;

            Control.BorderStyle = UITextBorderStyle.None;

            _line = new CALayer {
                BorderColor = color,
                BackgroundColor = color,
                Frame = new CGRect (0, Frame.Height / 2, Frame.Width * 2, 1f)
            };

            Control.Layer.AddSublayer (_line);
        }
    }
}