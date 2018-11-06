using System;
using QuoteApp.Backend.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace QuoteApp.FrontEnd.View.ItemView
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QuoteItemView : ContentPage
    {
        public Quote QuoteItem { get; set; }

        public QuoteItemView()
        {
            QuoteItem = new Quote
            {
                Date = DateTime.Now,
                Text = "I used to think I was indecisive, but now I'm not too sure.",
                Context = "Said by some anonymous person in the street, let's suppos it's said by a cat."
            };

            InitializeComponent();
        }
    }
}