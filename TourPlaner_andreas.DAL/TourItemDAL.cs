using System.Collections.Generic;
using TourPlaner_andreas.Models;

namespace TourPlaner_andreas.DAL {
    public class TourItemDAL {

        private DataAccess dataAccess;

        public TourItemDAL() {
            // check which datasource to use
            // e.g. use config file for this
            dataAccess = new DbConnection();
            //dataAccess = new FileAccess();
        }
            
        public IEnumerable<TourItem> GetItems(TourFolder folder) {
            // usually querying the disk, or from a DB, or ...
            return dataAccess.GetItems();
        }

        public void AddLogToTour(TourItem item, TourLog logs) {
            dataAccess.AddLogToItem(item, logs);
        }
    }
}
