using System.Collections.Generic;
using System.Linq;
using TourPlaner_andreas.Models;
using TourPlaner_andreas.DAL;

namespace TourPlaner_andreas.BL {
    internal class WpfAppManagerImpl : IWpfAppManager
    {

        TourItemDAL tourItemDal = new TourItemDAL();

        public IEnumerable<TourItem> GetItems(TourFolder folder)
        {
            return tourItemDal.GetItems(folder);
        }

        public TourFolder GetTourFolder(string url)
        {
            // usally located somewhere on the disk
            return new TourFolder();
        }

        public IEnumerable<TourItem> SearchForItems(string itemName, TourFolder folder, bool caseSensitive = false)
        {
            IEnumerable<TourItem> items = GetItems(folder);

            if (caseSensitive)
            {
                return items.Where(x => x.Name.Contains(itemName));
            }
            return items.Where(x => x.Name.ToLower().Contains(itemName.ToLower()));
        }

        public void CreateLogs(TourItem item, TourLog logs)
        {
            tourItemDal.AddLogToTour(item, logs);
        }
    }
}
