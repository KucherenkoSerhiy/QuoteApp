using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace QuoteApp.Backend.Services
{
    public interface  IShareService
    {
        void Share(string subject, string message, ImageSource image);
    }
}
