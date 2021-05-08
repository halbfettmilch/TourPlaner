﻿using System;
using System.Collections.Generic;
using TourPlaner_andreas.Models;

//DAO = DataAccessObject
namespace TourPlaner_andreas.DAL.DAO
{
    public interface ITourItemDAO
    {
        TourItem FindById(int itemId);

        void DeleteById(int itemID);

        TourItem AddNewItem(int tourId, string name, string url, DateTime creationTime, int tourLength, int duration);

        IEnumerable<TourItem> GetItems(TourFolder folder);
    }
}
