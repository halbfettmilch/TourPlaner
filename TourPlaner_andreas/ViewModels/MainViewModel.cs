using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using log4net;
using Microsoft.Win32;
using TourPlaner_andreas.BL;
using TourPlaner_andreas.Commands;
using TourPlaner_andreas.Models;
using TourPlaner_andreas.Views;

//enthält die Funktionen die ein Binding auf das MainWindow haben
namespace TourPlaner_andreas.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private TourItem currentItem;
        private ObservableCollection<TourItem> currentItemInfos;
        private TourLog currentLog;

        private string displayedImage =
            "C:\\Users\\Andre\\source\\repos\\TourPlaner_andreas\\TourPlaner_andreas\\Pics\\214335.jpg";

        private readonly TourFolder folder;
        public ObservableCollection<TourLog> logs;
        public bool logSelected;
        private string searchName;
        private readonly IAppManager tourManager;
        public bool tourSelected;

        public MainViewModel(IAppManager tourManager)
        {
            this.tourManager = tourManager;
            Items = new ObservableCollection<TourItem>();
            folder = tourManager.GetTourFolder("Get Tour Folder From Disk");

            SearchCommand = new RelayCommand(o =>
            {
                var searchTerm = SearchName ?? string.Empty;
                var items = tourManager.SearchForItems(searchTerm, folder);
                Items.Clear();
                foreach (var item in items) Items.Add(item);
            });
            //wird nicht verwendet
            ClearCommand = new RelayCommand(o =>
            {
                Items.Clear();
                SearchName = "";

                FillTourListView();
            });
            ExportFile = new RelayCommand(o =>
            {
                SaveFileDialog open = new SaveFileDialog();
                // image filters  
                open.Filter = "Text Files | *.txt";
                if (open.ShowDialog() == true)
                {
                    string path = open.FileName;
                    log.Info("File Exported");
                    tourManager.ExportFile(Logs, currentItem,path);
                }
               
            });
            ImportFile = new RelayCommand(o =>
            {
                OpenFileDialog open = new OpenFileDialog();
                // image filters  
                open.Filter = "Text Files | *.txt";
                if (open.ShowDialog() == true)
                {
                    string path = open.FileName;
                    tourManager.ImportFile(path);
                    var searchTerm = SearchName ?? string.Empty;
                    var items = tourManager.SearchForItems(searchTerm, folder);
                    Items.Clear();
                    foreach (var item in items) Items.Add(item);
                    log.Info("File Imported");
                }

            });
            PrintPdf = new RelayCommand(o =>
            {
                log.Info("Report Created");
                tourManager.CreateTourPdf(currentItem);
            });
            PrintAllPdf = new RelayCommand(o =>
            {
                log.Info("Report for all Created");
                tourManager.CreateTourLogsPdf(logs, currentItem);
            });


            CloseWindow = new RelayCommand(o => ((Window) o).Close()
            );

            AddTourCommand = new RelayCommand(o =>
            {
                var atw = new AddTourWindow();
                var result = atw.ShowDialog();
                if (result == true && atw.name != "" && atw.creationTime != "" && atw.length != "" &&
                    atw.expectedDuration != "" && atw.fromstart != "" && atw.to != "")
                {
                    var length = Convert.ToInt32(atw.length);
                    var eDuration = Convert.ToInt32(atw.expectedDuration);

                    var genItem = tourManager.CreateItem(atw.name, atw.fromstart, atw.to, DateTime.Now, length,
                        eDuration, atw.description);
                    log.Info("New Tour added");
                    Items.Add(genItem);
                }
            });
            AddLogCommand = new RelayCommand(o =>
            {
                var alw = new AddLogWindow();
                var result = alw.ShowDialog();
                if (result == true && alw.date != "" && alw.duration != "" && alw.maxVelocity != "" &&
                    alw.minVelocity != "" && alw.avVelocity != "" && alw.date != "" && alw.caloriesBurnt != "" &&
                    alw.author != "" && alw.comment != "")
                {
                    var date = DateTime.Parse(alw.date);
                    var maxVel = Convert.ToInt32(alw.maxVelocity);
                    var minVel = Convert.ToInt32(alw.minVelocity);
                    var avVel = Convert.ToInt32(alw.avVelocity);
                    var calBurnt = Convert.ToInt32(alw.caloriesBurnt);
                    var dur = Convert.ToInt32(alw.duration);
                    var genItem = tourManager.CreateItemLog(date, maxVel, minVel, avVel, calBurnt, dur, alw.author,
                        alw.comment, currentItem); // touritemId?
                    log.Info("New Log added to Tour");
                    Logs.Add(genItem);
                }
            });

            DelTourCommand = new RelayCommand(o =>
            {
                tourManager.DeleteTourWithId(currentItem);
                Items.Remove(currentItem);
                log.Info("Tour Deleted");
            });

            DelLogCommand = new RelayCommand(o =>
            {
                tourManager.DeleteLogWithId(currentLog);
                logs.Remove(currentLog);
                log.Info("Log Deleted");
            });
            SetDbAccess = new RelayCommand(o => { SetSetting("useFileSystem", "false"); });
            SetFileAccess = new RelayCommand(o => { SetSetting("useFileSystem", "true"); });


            InitTourListView();
        }


        public ICommand SearchCommand { get; set; }
        public ICommand ClearCommand { get; set; }
        public ICommand AddTourCommand { get; set; }
        public ICommand DelTourCommand { get; set; }
        public ICommand AddLogCommand { get; set; }
        public ICommand DelLogCommand { get; set; }
        public ICommand ExportFile { get; set; }
        public ICommand ImportFile { get; set; }
        public ICommand PrintPdf { get; set; }
        public ICommand PrintAllPdf { get; set; }
        public ICommand CloseWindow { get; set; }
        public ICommand SetDbAccess { get; set; }
        public ICommand SetFileAccess { get; set; }
        public ObservableCollection<TourItem> Items { get; set; }


        public bool TourSelected
        {
            get => tourSelected;
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
            get => logSelected;
            set
            {
                if (value != logSelected)
                {
                    logSelected = value;
                    RaisePropertyChangedEvent(nameof(LogSelected));
                }
            }
        }

        public string SearchName
        {
            get => searchName;
            set
            {
                if (searchName != value)
                {
                    searchName = value;
                    RaisePropertyChangedEvent(nameof(SearchName));
                }
            }
        }


        public TourItem CurrentItem
        {
            get => currentItem;
            set
            {
                if (currentItem != value && value != null)
                {
                    currentItem = value;
                    RaisePropertyChangedEvent(nameof(CurrentItem));
                    InitLogListView(currentItem);
                    InitCurrentItemInfosView(currentItem);
                    tourSelected = true;
                    RaisePropertyChangedEvent(nameof(TourSelected));
                }
                else
                {
                    tourSelected = false;
                }

                RaisePropertyChangedEvent(nameof(TourSelected));
            }
        }

        public string DisplayedImage
        {
            get => displayedImage;

            set
            {
                if (displayedImage != value)
                {
                    displayedImage = value;
                    RaisePropertyChangedEvent(nameof(DisplayedImage));
                    log.Info("Image displayed");
                }
            }
        }

        public TourLog CurrentLog
        {
            get => currentLog;
            set
            {
                if (currentLog != value && value != null)
                {
                    currentLog = value;
                    RaisePropertyChangedEvent(nameof(CurrentLog));
                    logSelected = true;
                    RaisePropertyChangedEvent(nameof(LogSelected));
                }
                else
                {
                    logSelected = false;
                }

                RaisePropertyChangedEvent(nameof(LogSelected));
            }
        }


        public ObservableCollection<TourLog> Logs
        {
            get => logs;

            set
            {
                if (logs != value)
                {
                    logs = value;
                    RaisePropertyChangedEvent(nameof(Logs));
                }
            }
        }

        public ObservableCollection<TourItem> CurrentItemInfos
        {
            get => currentItemInfos;

            set
            {
                if (currentItemInfos != value)
                {
                    currentItemInfos = value;
                    RaisePropertyChangedEvent(nameof(CurrentItemInfos));
                }
            }
        }

        private static void SetSetting(string key, string value)
        {
            //  Configuration configuration =
            //     ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            //  configuration.AppSettings.Settings[key].Value = value;
            //  configuration.Save(ConfigurationSaveMode.Full);
            //  ConfigurationManager.RefreshSection("appSettings");
        }


        public void InitTourListView()
        {
            Items = new ObservableCollection<TourItem>();
            FillTourListView();
        }

        private void FillTourListView()
        {
            foreach (var item in tourManager.GetItems(folder)) Items.Add(item);
        }

        public void InitLogListView(TourItem currentItem)
        {
            Logs = new ObservableCollection<TourLog>();
            FillLogListView(currentItem);
        }

        private void FillLogListView(TourItem currentitem)
        {
            foreach (var item in tourManager.GetLogsForTourItem(currentitem)) Logs.Add(item);
        }

        public void InitCurrentItemInfosView(TourItem currentItem)
        {
            CurrentItemInfos = new ObservableCollection<TourItem>();
            CurrentItemInfos.Add(currentItem);
            DisplayedImage = "C:\\Users\\Andre\\source\\repos\\TourPlaner_andreas\\TourPlaner_andreas\\Pics\\" +
                             currentItem.TourID + ".jpg";
            log.Info("pic Updated");
        }
    }
}