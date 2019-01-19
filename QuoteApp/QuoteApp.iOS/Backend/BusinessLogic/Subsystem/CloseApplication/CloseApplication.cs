using System.Threading;
using QuoteApp.Backend.BusinessLogic.Subsystem.CloseApplication;

namespace QuoteApp.iOS.Backend.BusinessLogic.Subsystem.CloseApplication
{
    public class CloseApplication : ICloseApplication
    {
        public void Close()
        {
            Thread.CurrentThread.Abort();
        }
    }
}