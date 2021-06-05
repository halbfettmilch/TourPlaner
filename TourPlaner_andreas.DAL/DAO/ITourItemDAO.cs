using System;
using System.Collections.Generic;
using TourPlaner_andreas.Models;

//DAO = DataAccessObject
namespace TourPlaner_andreas.DAL.DAO
{
    public interface ITourItemDAO
    {
        TourItem FindById(int itemId);

        void DeleteById(int itemID);

        TourItem AddNewItem(int tourId, string name, string fromstart,string to, DateTime creationTime, int tourLength, int duration, string description);

        IEnumerable<TourItem> GetItems(TourFolder folder);
    }
}
