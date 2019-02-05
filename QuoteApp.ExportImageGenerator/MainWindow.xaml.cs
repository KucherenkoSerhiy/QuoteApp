using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace QuoteApp.ExportImageGenerator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _textDay;
        private string _textNight;
        private string _autor;

        private BitmapImage _backgroundDay;
        private BitmapImage _backgroundNight;

        public MainWindow()
        {
            InitializeComponent();

            _textDay = "\"Come on, this is the sample text. Oh, I see you finally managed to draw me. All right, but what if I am too long for such the image so you have actually to manage my lines? And don't forget about vertical space, buddy. Let's make me really extra-big-large-super-mega-king size! Now I AM GODZILLA! All right, this is the final step to test how well you space the text. After this one, exam is passed and you may continue your journey. Good luck!\"";
            _textNight = "\"I am short.\"";
            _autor = "- Some VERY Talkative Person";

            _backgroundDay = new BitmapImage(new Uri("pack://application:,,,/QuoteApp.ExportImageGenerator;component/BackgroundImageDay.png"));
            GeneratedImageDay.Source = new ImageGenerator().GenerateImageWithQuote(_backgroundDay, _textDay, _autor, Color.FromRgb(14, 14, 14));
            
            _backgroundNight = new BitmapImage(new Uri("pack://application:,,,/QuoteApp.ExportImageGenerator;component/BackgroundImageNight.png"));
            GeneratedImageNight.Source = new ImageGenerator().GenerateImageWithQuote(_backgroundNight, _textNight, _autor, Color.FromRgb(255, 255, 236));
        }
    }
}
