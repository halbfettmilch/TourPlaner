using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace TourPlaner_andreas.Views
{
    /// <summary>
    ///     Interaction logic for AddTourWindow.xaml
    /// </summary>
    public partial class AddTourWindow : Window
    {
        public string creationTime;
        public string description;
        public string expectedDuration;
        public string fromstart;
        public string length;
        public string name;
        public string to;

        public AddTourWindow()
        {
            InitializeComponent();
        }

        private void addTour_Click(object sender, EventArgs e)
        {
            name = Name.Text;
            fromstart = Fromstart.Text;
            to = To.Text;
            length = Length.Text;
            expectedDuration = Expected_Duration.Text;
            description = Description.ToString();
            DialogResult = true;
            Close();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}