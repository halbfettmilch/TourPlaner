using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using TourPlaner_andreas.ViewModels;
using TourPlaner_andreas.BL;
using TourPlaner_andreas.Models;
//enthält die Funktionen die ein Binding auf das MainWindow haben
namespace TourPlaner_andreas.ViewModels {
    public class TourFolderVM : ViewModelBase {

        private IAppManager tourManager;
        private TourItem currentItem;
        private TourFolder folder;
        private string searchName;

        public ICommand SearchCommand { get; set; }
        public ICommand ClearCommand { get; set; }
        public ICommand AddTourCommand { get; set; }
        public ICommand DelTourCommand { get; set; }
        public ICommand PrintPdf { get; set; }
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

        public TourFolderVM(IAppManager tourManager) {
            this.tourManager = tourManager;
            Items = new ObservableCollection<TourItem>();
            folder = tourManager.GetTourFolder("Get Tour Folder From Disk");

            this.SearchCommand = new RelayCommand(o =>
            {
                string searchTerm = SearchName ?? string.Empty;
                IEnumerable<TourItem> items = tourManager.SearchForItems(searchTerm, folder);
                Items.Clear();
                foreach (TourItem item in items) {
                    Items.Add(item);
                }
            });
            //wird nicht verwendet
            this.ClearCommand = new RelayCommand(o => {
                Items.Clear();
                SearchName = "";

                FillListView();
            });
            this.PrintPdf = new RelayCommand(o => {
               tourManager.CreatePdf(tourManager.GetItems(folder));
               
            });

            this.AddTourCommand = new RelayCommand(o =>
            {
                TourItem genItem = tourManager.CreateItem(1, "testTour", "C:/keinfolder", DateTime.Now , 1, 1);
                Items.Add(genItem);
            });

            this.DelTourCommand = new RelayCommand(o =>
            {
               
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
