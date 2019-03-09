using Android.App;
using Android.Content.PM;
using Android.Content.Res;
using Android.Views;
using Android.OS;

namespace QuoteApp.Droid
{
    [Activity(Label = "Pocket Quotes", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TypedArray styledAttributes = this.Theme.ObtainStyledAttributes(
                new int[] { Android.Resource.Attribute.ActionBarSize });
            var actionbarHeight = (int) styledAttributes.GetDimension(0, 0);

            App.ScreenWidth = (int)Resources.DisplayMetrics.WidthPixels; // real pixels
            App.ScreenHeight = (int)Resources.DisplayMetrics.HeightPixels + actionbarHeight; // real pixels

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            RequestWindowFeature(WindowFeatures.NoTitle);

            StrictMode.VmPolicy.Builder builder = new StrictMode.VmPolicy.Builder();
            StrictMode.SetVmPolicy(builder.Build());

            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            //====================================
            int uiOptions = (int)Window.DecorView.SystemUiVisibility;

            uiOptions |= (int)SystemUiFlags.LowProfile;
            uiOptions |= (int)SystemUiFlags.Fullscreen;
            uiOptions |= (int)SystemUiFlags.HideNavigation;
            uiOptions |= (int)SystemUiFlags.ImmersiveSticky;

            Window.DecorView.SystemUiVisibility = (StatusBarVisibility)uiOptions;
            //====================================

            LoadApplication(new App());
        }
    }
}