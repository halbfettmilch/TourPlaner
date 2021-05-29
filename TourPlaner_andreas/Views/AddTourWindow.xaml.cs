using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
namespace TourPlaner_andreas.Views
{
    /// <summary>
    /// Interaction logic for AddTourWindow.xaml
    /// </summary>
    public partial class AddTourWindow : Window
    {
        public string name;
        public string creationTime;
        public string length;
        public string expectedDuration;
        public AddTourWindow()
        {
            InitializeComponent();
        }
        private void addTour_Click(object sender, EventArgs e)
        {
            name = Name.Text.ToString();
            length = Length.Text.ToString();
            expectedDuration = Expected_Duration.Text.ToString();
            DialogResult = true;
            Close();
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }


    }
}
