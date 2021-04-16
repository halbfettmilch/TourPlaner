using System.Collections.Generic;
using TourPlaner_andreas.Models;

namespace TourPlaner_andreas.BL {
    public interface IWpfAppManager {
        TourFolder GetTourFolder(string url);
        IEnumerable<TourItem> GetItems(TourFolder folder);
        IEnumerable<TourItem> SearchForItems(string itemName, TourFolder folder, bool caseSensitive = false);
    }
}
