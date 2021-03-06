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

namespace TourPlaner_andreas.Views
{
    /// <summary>
    /// Interaction logic for AddLogWindow.xaml
    /// </summary>
    public partial class AddLogWindow : Window
    {
        public string date;
        public string maxVelocity;
        public string minVelocity;
        public string avVelocity;
        public string caloriesBurnt;
        public string duration;
        public string author;
        public string comment;
        public AddLogWindow()
        {
            InitializeComponent();
        }
        private void addLog_Click(object sender, EventArgs e)
        {
            this.date = Date.Text.ToString();
            this.maxVelocity = MaxVelocity.Text.ToString();
            this.minVelocity = MinVelocity.Text.ToString();
            this.avVelocity = AvVelocity.Text.ToString();
            this.caloriesBurnt = CaloriesBurnt.Text.ToString();
            this.duration = Duration.Text.ToString();
            this.author = Author.Text.ToString();
            this.comment = Comment.Text.ToString();
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
