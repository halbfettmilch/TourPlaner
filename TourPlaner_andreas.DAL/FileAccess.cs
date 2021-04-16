using System.Collections.Generic;
using TourPlaner_andreas.Models;


namespace TourPlaner_andreas.DAL {
    class FileAccess : DataAccess {

        private string filePath;

        public FileAccess() {
            // get filepath from config file
            filePath = "...";
        }

        public List<TourItem> GetItems() {
            // get items from file path
            return new List<TourItem>() {
                new TourItem() { Name = "Item1" },
                new TourItem() { Name = "Item2" },
                new TourItem() { Name = "Another" },
                new TourItem() { Name = "SWEI" },
                new TourItem() { Name = "FHTW" }
            };
        }

        public void AddLogToItem(TourItem item, TourLog logs) {
            // Insert/Update logic here
        }
    }
}
