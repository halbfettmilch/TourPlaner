using System;
using System.Collections.Generic;
using TourPlaner_andreas.Models;

//DAO = DataAccessModel
namespace TourPlaner_andreas.DAL.DAO
{
    public interface ITourItemDAO
    {
        TourItem FindById(int itemId);

        TourItem AddNewItem(string name, string url, DateTime creationTime);

        IEnumerable<TourItem> GetItems();
    }
}
