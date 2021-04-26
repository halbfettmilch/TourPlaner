using System;
using System.Collections.Generic;
using TourPlaner_andreas.Models;

namespace TourPlaner_andreas.DAL
{
    
    // declare Functions to interact with DB
        interface DataAccess
        {
            public List<TourItem> GetItems();
            public void AddLogToItem(TourItem item, TourLog logs);
        }
    
}
