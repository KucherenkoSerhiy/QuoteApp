using Android.Content;
using Android.Graphics;
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
        private Color StartColor { get; set; }
        private Color EndColor { get; set; }
        private int CornerRadius { get; set; }

        public CustomGradientBackgroundButtonRenderer(Context context) : base(context)
        {
        }

        protected override void DispatchDraw(Canvas canvas)
        {
            var gradient = new LinearGradient(0, 0, 0, Height,
                StartColor.ToAndroid(),
                EndColor.ToAndroid(),
                Shader.TileMode.Mirror);
            //gradient.CornerRadius = CornerRadius;
            var paint = new Paint
            {
                Dither = true,
            };
            paint.SetShader(gradient);
            canvas.DrawRoundRect(new RectF(0, 0, Width, Height), CornerRadius*2.9f, CornerRadius*2.9f, paint);
            base.DispatchDraw(canvas);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Button> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || Element == null)
            {
                return;
            }

            if (e.NewElement is CustomGradientBackgroundButton button)
            {
                StartColor = button.StartColor;
                EndColor = button.EndColor;
                CornerRadius = button.CornerRadius;
            }
        }
    }
}