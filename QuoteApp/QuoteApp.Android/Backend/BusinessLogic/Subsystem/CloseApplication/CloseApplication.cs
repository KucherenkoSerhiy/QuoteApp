using Android.App;
using QuoteApp.Backend.BusinessLogic.Subsystem.CloseApplication;
using QuoteApp.Droid.Backend.BusinessLogic.Subsystem.CloseApplication;
using Xamarin.Forms;

[assembly: Dependency(typeof(CloseApplication))]
namespace QuoteApp.Droid.Backend.BusinessLogic.Subsystem.CloseApplication
{
    public class CloseApplication : ICloseApplication
    {
        public void Close()
        {
            var activity = (Activity)Forms.Context;
            activity.FinishAffinity();
        }
    }
}