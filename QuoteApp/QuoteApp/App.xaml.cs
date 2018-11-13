using System;
using QuoteApp.Backend.BusinessLogic.Manager;
using QuoteApp.Backend.BusinessLogic.Subsystem.PersistentProperties;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace QuoteApp
{
    public partial class App : Application
    {
        public static int ScreenWidth;
        public static int ScreenHeight;

        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
            var databaseManager =  DatabaseManager.Instance;
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
            PersistentProperties.Instance.SerializeToXml();
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
