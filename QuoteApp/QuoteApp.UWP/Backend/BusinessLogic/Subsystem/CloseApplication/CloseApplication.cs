using Windows.UI.Xaml;
using QuoteApp.Backend.BusinessLogic.Subsystem.CloseApplication;

namespace QuoteApp.UWP.Backend.BusinessLogic.Subsystem.CloseApplication
{
    public class CloseApplication : ICloseApplication
    {
        public void Close()
        {
            Application.Current.Exit();
        }
    }
}