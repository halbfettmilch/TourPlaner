using System;
using System.Collections.Generic;
using TourPlaner_andreas.Models;

namespace TourPlaner_andreas.BL {
    public interface IAppManager {
        TourFolder GetTourFolder(string url);
        IEnumerable<TourItem> GetItems(TourFolder folder);
        IEnumerable<TourItem> SearchForItems(string itemName, TourFolder folder, bool caseSensitive = false);
        IEnumerable<TourLog> GetLogsForTourItem( TourItem tour);
        TourLog CreateItemLog(int logId, DateTime date, int maxVelocity, int minVelocity, int avVelocity, int caloriesBurnt, int duration, TourItem loggedItem);
        TourItem CreateItem(int tourId, string name, string url, DateTime creationTime, int tourLength, int duration);
        public void CreatePdf(IEnumerable<TourItem> tourItems);
        public void DeleteTourWithId(TourItem touritem);
    }
}
