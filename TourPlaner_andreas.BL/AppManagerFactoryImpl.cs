using System;
using System.Collections.Generic;
using System.Linq;
using TourPlaner_andreas.Models;
using TourPlaner_andreas.DAL;
using TourPlaner_andreas.DAL.common;
using TourPlaner_andreas.DAL.DAO;

namespace TourPlaner_andreas.BL {
    internal class AppManagerFactoryImpl : AppManager
    {

      

        public IEnumerable<TourItem> GetItems(TourFolder folder)
        {
            ITourItemDAO tourItemDAO = DALFactory.CreateTourItemDAO();
            return tourItemDAO.GetItems(folder);
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

        public TourLog CreateItemLog(string logText, TourItem item)
        {
            ITourLogDAO tourLogDAO = DALFactory.CreateTourLogDAO();
            return tourLogDAO.AddNewItemLog(logText, item);
        }

        public TourItem CreateItem(int tourId, string name, string url, DateTime creationTime, int tourLength, int duration)
        {
            ITourItemDAO tourItemDAO = DALFactory.CreateTourItemDAO();
            return tourItemDAO.AddNewItem(tourId,name, url, creationTime, tourLength,duration);
        }
    }
}
