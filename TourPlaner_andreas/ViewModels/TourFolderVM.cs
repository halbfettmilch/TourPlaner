using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TourPlaner_andreas.ViewModels;
using TourPlaner_andreas.BL;
using TourPlaner_andreas.Models;

namespace TourPlaner_andreas.ViewModels {
    public class TourFolderVM : ViewModelBase {

        private IWpfAppManager tourManager;
        private TourItem currentItem;
        private TourFolder folder;
        private string searchName;

        public ICommand SearchCommand { get; set; }

        public ICommand ClearCommand { get; set; }

        public ObservableCollection<TourItem> Items { get; set; }

        public string SearchName {
            get { return searchName; }
            set {
                if (searchName != value) {
                    searchName = value;
                    RaisePropertyChangedEvent(nameof(SearchName));
                }
            }
        }

        public TourItem CurrentItem {
            get { return currentItem; }
            set {
                if ((currentItem != value) && (value != null)) {
                    currentItem = value;
                    RaisePropertyChangedEvent(nameof(CurrentItem));
                }
            }
        }

        public TourFolderVM(IWpfAppManager tourManager) {
            this.tourManager = tourManager;
            Items = new ObservableCollection<TourItem>();
            folder = tourManager.GetTourFolder("Get Tour Folder From Disk");

            this.SearchCommand = new RelayCommand(o => {
                IEnumerable<TourItem> items = tourManager.SearchForItems(SearchName, folder);
                Items.Clear();
                foreach (TourItem item in items) {
                    Items.Add(item);
                }
            });

            this.ClearCommand = new RelayCommand(o => {
                Items.Clear();
                SearchName = "";

                FillListView();
            });

            InitListView();
        }


        public void InitListView() {
            Items = new ObservableCollection<TourItem>();
            FillListView();
        }

        private void FillListView() {
            foreach (TourItem item in tourManager.GetItems(folder)) {
                Items.Add(item);
            }
        }
    }
}
