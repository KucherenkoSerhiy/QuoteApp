using CoreAnimation;
using CoreGraphics;
using QuoteApp.FrontEnd.Resources.CustomControls;
using QuoteApp.iOS.CustomRenderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomGradientBackgroundButton), typeof(CustomGradientBackgroundButtonRenderer))]
namespace QuoteApp.iOS.CustomRenderers
{
    class CustomGradientBackgroundButtonRenderer : ButtonRenderer
    {
        private CustomGradientBackgroundButton _button;

        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null) return;
            if (Equals(_button, null))
                _button = e.NewElement as CustomGradientBackgroundButton;

            Control.TouchDown += delegate
            {
                _button.IsPressed = true;
                SetNeedsDisplay();
            };
            Control.TouchUpInside += delegate
            {
                _button.IsPressed = false;
                SetNeedsDisplay();
            };
        }

        public override void Draw(CGRect rect)
        {
            var gradientView = new UIView(Control.Frame);
            Control.BackgroundColor = UIColor.Clear;

            var gradientLayer = new CAGradientLayer
            {
                Frame = gradientView.Layer.Bounds,
                Colors = new[] { _button.StartColor.ToCGColor(), _button.EndColor.ToCGColor() }
            };

            gradientView.Layer.AddSublayer(gradientLayer);
            gradientView.AddSubview(Control);

            var gradientButton = new UIButton(Control.Frame);
            gradientButton.AddSubview(gradientView);
            SetNativeControl(gradientButton);

            gradientLayer.StartPoint = new CGPoint(0.5, 0);
            gradientLayer.EndPoint = new CGPoint(0.5, 1.0);

            base.Draw(rect);
        }
    }
}