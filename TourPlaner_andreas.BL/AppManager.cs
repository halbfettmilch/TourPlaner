using System;
using System.Collections.Generic;
using TourPlaner_andreas.Models;

namespace TourPlaner_andreas.BL {
    public interface AppManager {
        TourFolder GetTourFolder(string url);
        IEnumerable<TourItem> GetItems(TourFolder folder);
        IEnumerable<TourItem> SearchForItems(string itemName, TourFolder folder, bool caseSensitive = false);
        TourLog CreateItemLog(string logText, TourItem item);
        TourItem CreateItem(string name, string annotation, string url, DateTime creationDate);
    }
}
