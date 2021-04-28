using System;
using System.Collections.Generic;
using TourPlaner_andreas.Models;

//DAO = DataAccessObject
namespace TourPlaner_andreas.DAL.DAO
{
    public interface ITourItemDAO
    {
        TourItem FindById(int itemId);

        TourItem AddNewItem(string name, string url,string annotaition, DateTime creationTime);

        IEnumerable<TourItem> GetItems(TourFolder folder);
    }
}
