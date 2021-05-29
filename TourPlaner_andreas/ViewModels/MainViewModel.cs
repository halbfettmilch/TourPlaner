using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using TourPlaner_andreas.Commands;
using TourPlaner_andreas.BL;
using TourPlaner_andreas.Models;
using TourPlaner_andreas.Views;

//enthält die Funktionen die ein Binding auf das MainWindow haben
namespace TourPlaner_andreas.ViewModels {
    public class MainViewModel : ViewModelBase {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IAppManager tourManager;
        private TourItem currentItem;
        private TourLog currentLog;
        private TourFolder folder;
        private string searchName;
        

        public ICommand SearchCommand { get; set; }
        public ICommand ClearCommand { get; set; }
        public ICommand AddTourCommand { get; set; }
        public ICommand DelTourCommand { get; set; }
        public ICommand AddLogCommand { get; set; }
        public ICommand DelLogCommand { get; set; }
        public ICommand PrintPdf { get; set; }
        public ICommand CloseWindow { get; set; }
        public ObservableCollection<TourItem> Items { get; set; }
        public ObservableCollection<TourLog> logs;
        private ObservableCollection<TourItem> currentItemInfos;
        private bool tourSelected = false;
        private bool logSelected = false;

        public bool TourSelected
        {
            get { return tourSelected; }
            set
            {
                if (value != tourSelected)
                {
                    tourSelected = value;
                    RaisePropertyChangedEvent(nameof(TourSelected));
                }
            }
        }
        public bool LogSelected
        {
            get { return logSelected; }
            set
            {
                if (value != logSelected)
                {
                    logSelected = value;
                    RaisePropertyChangedEvent(nameof(LogSelected));
                }
            }
        }

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
            set
            {
                if ((currentItem != value) && (value != null))
                {
                    currentItem = value;
                    RaisePropertyChangedEvent(nameof(CurrentItem));
                    InitLogListView(currentItem);
                    InitCurrentItemInfosView(currentItem);
                    tourSelected = true;
                }
                else tourSelected = false;
            }
        }

        public TourLog CurrentLog
        {
            get { return currentLog; }
            set
            {
                if ((currentLog != value) && (value != null))
                {
                    currentLog = value;
                    RaisePropertyChangedEvent(nameof(CurrentLog));
                    logSelected = true;

                }
                else logSelected = false;
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
      

            this.CloseWindow = new RelayCommand(o => ((Window)o).Close()

            );

            this.AddTourCommand = new RelayCommand(o =>
            {
                AddTourWindow atw = new AddTourWindow();
                var result = atw.ShowDialog();
                if (result == true && atw.name != "" && atw.creationTime != "" && atw.length != "" && atw.expectedDuration != "")
                {
                    int length = Convert.ToInt32(atw.length);
                    int eDuration = Convert.ToInt32(atw.expectedDuration);
                    
                    TourItem genItem = tourManager.CreateItem( atw.name, "C:/keinfolder", DateTime.Now, length, eDuration, atw.description);
                    log.Info("New Tour added");
                    Items.Add(genItem);
                }
              
            });
            this.AddLogCommand = new RelayCommand(o =>
            {   
                AddLogWindow alw = new AddLogWindow();
                var result = alw.ShowDialog();
                if (result == true && alw.date != "" && alw.duration != "" && alw.maxVelocity != "" && alw.minVelocity != "" && alw.avVelocity != "" && alw.date != "" && alw.caloriesBurnt != "")
                {
                    
                    var date = DateTime.Parse(alw.date);
                    int maxVel = Convert.ToInt32(alw.maxVelocity);
                    int minVel = Convert.ToInt32(alw.minVelocity);
                    int avVel = Convert.ToInt32(alw.avVelocity);
                    int calBurnt = Convert.ToInt32(alw.caloriesBurnt);
                    int dur = Convert.ToInt32(alw.duration);
                    TourLog genItem = tourManager.CreateItemLog( date, maxVel, minVel, avVel, calBurnt, dur, currentItem);// touritemId?
                    log.Info("New Log added to Tour");
                    Logs.Add(genItem);
                }
               
            });

            this.DelTourCommand = new RelayCommand(o =>
            {   
                tourManager.DeleteTourWithId(currentItem);
                Items.Remove(currentItem);
                log.Info("Tour Deleted");
            });

            this.DelLogCommand = new RelayCommand(o =>
            {
                tourManager.DeleteLogWithId(currentLog);
                logs.Remove(currentLog);
                log.Info("Log Deleted");
                
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
