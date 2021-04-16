using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlaner_andreas.Models;

namespace TourPlaner_andreas.DAL
{
    class DbConnection : DataAccess
    {
        private string connectionString;

        public DbConnection()
        {
            // get connection string from config file
            connectionString = "...";
            // establish connection to DB
        }

        public void AddLogToItem(TourItem item, TourLog logs)
        {
            // add insert/update SQL statement/query here
        }

        public List<TourItem> GetItems()
        {
            // add select SQL query here
            return new List<TourItem>() {
                new TourItem() { Name = "Item1" },
                new TourItem() { Name = "Item2" },
                new TourItem() { Name = "Another" },
                new TourItem() { Name = "SWEI" },
                new TourItem() { Name = "FHTW" }
            };
        }
    }
}
