using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TourPlaner_andreas.ViewModels;

namespace TourPlaner_andreas
{
    public class MainViewModel : INotifyPropertyChanged
    {
       // public ObservableCollection<HighscoreEntry> Data { get; }
      //      = new ObservableCollection<HighscoreEntry>();

        public string CurrentUsername { get; set; }
        public string CurrentPoints { get; set; }
        public RelayCommand AddCommand { get; }
        public RelayCommand AddTour { get; }
        public RelayCommand DeleteTour { get; }
        public RelayCommand ImportTour { get; }
        public RelayCommand ExportTour { get; }

        private bool _isUsernameFocused;
        public bool IsUsernameFocused
        {
            get => _isUsernameFocused;
            set
            {
                // it needs to flip, else it does not execute properly, so let's reset here
                _isUsernameFocused = false;
                OnPropertyChanged();
                _isUsernameFocused = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            AddCommand = new RelayCommand((_) =>
            {
               // Data.Add(new HighscoreEntry(this.CurrentUsername, this.CurrentPoints));
                CurrentUsername = string.Empty;
                CurrentPoints = string.Empty;
                OnPropertyChanged(nameof(CurrentUsername));
                OnPropertyChanged("CurrentPoints");
                IsUsernameFocused = true;
            });
            IsUsernameFocused = true;
            AddTour = new RelayCommand((_) =>
            {
                
            });
            DeleteTour = new RelayCommand((_) =>
            {
               
            });
            ImportTour = new RelayCommand((_) =>
            {

            });
            ExportTour = new RelayCommand((_) =>
            {

            });
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
