using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using TourPlaner_andreas.Models;

namespace TourPlaner_andreas.BL {
    public interface IAppManager {
        TourFolder GetTourFolder(string url);
        IEnumerable<TourItem> GetItems(TourFolder folder);
        IEnumerable<TourItem> SearchForItems(string itemName, TourFolder folder, bool caseSensitive = false);
        IEnumerable<TourLog> GetLogsForTourItem( TourItem tour);
        TourLog CreateItemLog(DateTime date, int maxVelocity, int minVelocity, int avVelocity, int caloriesBurnt, int duration, string author, string comment, TourItem loggedItem);
        TourItem CreateItem( string name, string fromstart,string to, DateTime creationTime, int tourLength, int duration, string description);
        public bool CreateTourPdf(TourItem tourItem);
        public bool CreateTourLogsPdf(ObservableCollection<TourLog> tourLogs, TourItem tourItem);
        public void DeleteTourWithId(TourItem touritem);
        public void DeleteLogWithId(TourLog logItem);
        public int ExportFile(ObservableCollection<TourLog> tourLogs,TourItem item,string path);
        public TourItem ImportFile(string path);
        public bool checkForEmptyTour(string name, string fromstart, string to, DateTime creationTime, int tourLength,
            int duration, string description);
       
        



    }
}
