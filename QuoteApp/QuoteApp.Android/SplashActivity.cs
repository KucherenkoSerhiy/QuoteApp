﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using QuoteApp.Backend.BusinessLogic.Subsystem.SubsystemInitializer;

namespace QuoteApp.Droid
{
    [Activity(Label = "Pocket Quotes", Theme = "@style/MyTheme.Splash", MainLauncher = true, NoHistory = true)]
    public class SplashActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        static readonly string TAG = "X:" + typeof(SplashActivity).Name;

        public override void OnCreate(Bundle savedInstanceState, PersistableBundle persistentState)
        {
            base.OnCreate(savedInstanceState, persistentState);
            Log.Debug(TAG, "SplashActivity.OnCreate");
        }

        // Launches the startup task
        protected override void OnResume()
        {
            base.OnResume();
            Task startupWork = new Task(SimulateStartup);
            startupWork.Start();
        }

        // Performs background work that happens behind the splash screen
        async void SimulateStartup ()
        {
            Log.Debug(TAG, "Performing some startup work that takes a bit of time.");
            await Task.Run(() =>
            {
                // Run long time initialization here
                var initializer = SubsystemInitializer.Instance;
            });
            Log.Debug(TAG, "Startup work is finished - starting MainActivity.");
            StartActivity(new Intent(Application.Context, typeof (MainActivity)));
        }
    }
}