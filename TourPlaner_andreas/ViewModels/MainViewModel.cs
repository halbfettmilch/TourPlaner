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
    public class MainViewModel : ViewModelBase {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IAppManager tourManager;
        private TourItem currentItem;
        private TourFolder folder;
        private string searchName;

        public ICommand SearchCommand { get; set; }
        public ICommand ClearCommand { get; set; }
        public ICommand AddTourCommand { get; set; }
        public ICommand DelTourCommand { get; set; }
        public ICommand AddLogCommand { get; set; }
        public ICommand PrintPdf { get; set; }
        public ObservableCollection<TourItem> Items { get; set; }
        private ObservableCollection<TourLog> logs;
        private ObservableCollection<TourItem> currentItemInfos;
        

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
                    InitLogListView(currentItem);
                    InitCurrentItemInfosView(currentItem);

                }
            }
        }

        public ObservableCollection<TourLog> Logs
        {
            get
            {
                return logs;

            }

            set {
                if (logs != value)
                {
                    logs = value;
                    RaisePropertyChangedEvent(nameof(Logs));
                }
                    
            }
        }
        public ObservableCollection<TourItem> CurrentItemInfos
        {
            get
            {
                return currentItemInfos;

            }

            set
            {
                if (currentItemInfos != value)
                {
                    currentItemInfos = value;
                    RaisePropertyChangedEvent(nameof(CurrentItemInfos));
                }
            }
        }

        public MainViewModel(IAppManager tourManager) {
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

                FillTourListView();
            });
            this.PrintPdf = new RelayCommand(o => {
                log.Info("Report Created");
                tourManager.CreatePdf(tourManager.GetItems(folder));
               
            });

            this.AddTourCommand = new RelayCommand(o =>
            {
                TourItem genItem = tourManager.CreateItem(1, "testTour", "C:/keinfolder", DateTime.Now , 1, 1);
                log.Info("New Tour added");
                Items.Add(genItem);
            });
            this.AddLogCommand = new RelayCommand(o =>
            {
                TourLog genItem = tourManager.CreateItemLog(1, DateTime.Today, 59, 2, 20, 1,1,currentItem);// touritemId?
                log.Info("New Log added to Tour");
                Logs.Add(genItem);
            });

            this.DelTourCommand = new RelayCommand(o =>
            {   
                tourManager.DeleteTourWithId(currentItem);
                Items.Remove(currentItem);
                log.Info("Tour Deleted");
            });


            InitTourListView();
        }


        public void InitTourListView() {
            Items = new ObservableCollection<TourItem>();
            FillTourListView();
        }

        private void FillTourListView() {
            foreach (TourItem item in tourManager.GetItems(folder)) {
                Items.Add(item);
            }
        }
        public void InitLogListView(TourItem currentItem)
        {
            Logs = new ObservableCollection<TourLog>();
            FillLogListView(currentItem);
        }

        private void FillLogListView(TourItem currentitem)
        {
            foreach (TourLog item in tourManager.GetLogsForTourItem(currentitem))
            {   
                Logs.Add(item);
            }
        }
        public void InitCurrentItemInfosView(TourItem currentItem)
        {
            CurrentItemInfos = new ObservableCollection<TourItem>();
            CurrentItemInfos.Add(currentItem);
            
        }







    }
}
