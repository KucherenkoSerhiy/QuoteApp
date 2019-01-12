using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QuoteApp.ColorLab
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            /*TODO List:
             * 1. Color generating by formula subsystem (model + business logics)
             * 2. Create list of results of that subsystem
             * 3. Show them in UI:
             *      3.1. Create the templated list
             *      3.2. Pass the values to it
             * 4. Formula purification:
             *      4.1. This will be another subsystem that manages formulas, their evaluation and maintains their historic
             *      4.2. Optional, extention: bind it to neural network to have fun
             */
        }
    }
}
