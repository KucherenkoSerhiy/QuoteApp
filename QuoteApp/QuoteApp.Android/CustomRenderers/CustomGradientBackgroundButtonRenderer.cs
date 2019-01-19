using Android.Content;
using Android.Graphics;
using Android.Views;
using QuoteApp.Droid.CustomRenderers;
using QuoteApp.FrontEnd.Resources.CustomControls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Color = Xamarin.Forms.Color;

[assembly: ExportRenderer(typeof(CustomGradientBackgroundButton), typeof(CustomGradientBackgroundButtonRenderer))]
namespace QuoteApp.Droid.CustomRenderers
{
    public class CustomGradientBackgroundButtonRenderer : ButtonRenderer
    {
        private CustomGradientBackgroundButton Button { get; set; }

        public CustomGradientBackgroundButtonRenderer(Context context) : base(context)
        {
        }

        protected override void DispatchDraw(Canvas canvas)
        {
            var gradient = new LinearGradient(0, 0, 0, Height,
                Button.StartColor.ToAndroid(),
                Button.EndColor.ToAndroid(),
                Shader.TileMode.Mirror);
            var paint = new Paint
            {
                Dither = true,
            };
            paint.SetShader(gradient);
            canvas.DrawRoundRect(new RectF(0, 0, Width, Height), Button.CornerRadius*2.9f, Button.CornerRadius*2.9f, paint);
            Button.BorderWidth = Button.IsPressed ? Button.NominalBorderWidth : 0;
            base.DispatchDraw(canvas);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Button> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || Element == null)
                return;

            if (e.NewElement is CustomGradientBackgroundButton button)
            {
                Button = button;

                Control.Touch += (object sender, TouchEventArgs args) =>
                {
                    switch (args.Event.Action)
                    {
                        case MotionEventActions.Down:
                            button.IsPressed = true;
                            Invalidate();
                            break;
                        case MotionEventActions.Up:
                            button.IsPressed = false;
                            Invalidate();
                            button.SendClicked();
                            break;
                    }
                };
            }


        }
    }
}