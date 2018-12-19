using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace QuoteApp.FrontEnd.Resources.CustomControls
{
    public class CustomSwitch: Switch
    {
        public static readonly BindableProperty SwitchOffColorProperty =  
            BindableProperty.Create(nameof(SwitchOffColor),  
                typeof(Color), typeof(CustomSwitch),  
                Color.Default);  
  
        public Color SwitchOffColor  
        {  
            get => (Color)GetValue(SwitchOffColorProperty);
            set => SetValue(SwitchOffColorProperty, value);
        }  
  
        public static readonly BindableProperty SwitchOnColorProperty =  
            BindableProperty.Create(nameof(SwitchOnColor),  
                typeof(Color), typeof(CustomSwitch),  
                Color.Default);  
  
        public Color SwitchOnColor  
        {  
            get => (Color)GetValue(SwitchOnColorProperty);
            set => SetValue(SwitchOnColorProperty, value);
        }  
  
        public static readonly BindableProperty SwitchThumbColorProperty =  
            BindableProperty.Create(nameof(SwitchThumbColor),  
                typeof(Color), typeof(CustomSwitch),  
                Color.Default);  
  
        public Color SwitchThumbColor  
        {  
            get => (Color)GetValue(SwitchThumbColorProperty);
            set => SetValue(SwitchThumbColorProperty, value);
        } 
    }
}
