using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace QuoteApp.FrontEnd.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoadingView : ContentView
	{
	    public List<(string, string)> PatienceAutorsAndQuotes = new List<(string, string)>
	    {
	        ("Patience, persistence and perspiration make an unbeatable combination for success.", "Napoleon Hill"),
	        ("Patience is a virtue, and I'm learning patience. It's a tough lesson.", "Elon Musk"),
            ("I have just three things to teach: simplicity, patience, compassion. These three are your greatest treasures.", "Lao Tzu"),
            ("I am a slow walker, but I never walk back", "Abraham Lincoln."),
            ("First they ignore you. Then they laugh at you. Then they fight you. Then you win.", "Mahatma Gandhi"),
            ("Patience is not passive, on the contrary, it is concentrated strength.", "Bruce Lee"),
            ("Be patient and tough; someday this pain will be useful to you.", "Ovid"),
            ("Patience is not simply the ability to wait - it's how we behave while we're waiting.", "Joyce Meyer"),
            ("The two most powerful warriors are patience and time.", "Leo Tolstoy"),
            ("Never cut a tree down in the wintertime. Never make a negative decision in the low time. Never make your most important decisions when you are in your worst moods. Wait. Be patient. The storm will pass. The spring will come.", "Robert H. Schuller")
	    };

	    public string SelectedQuoteText { get; set; }
	    public string SelectedQuoteAutor { get; set; }

		public LoadingView ()
        {
            InitializeValues();

            InitializeComponent();
        }

        private void InitializeValues()
        {
            var selected = PatienceAutorsAndQuotes.ElementAt((int)(new Random().NextDouble() * 10));
            SelectedQuoteText = selected.Item1;
            SelectedQuoteAutor = selected.Item2;

            OnPropertyChanged(string.Empty);
        }
    }
}