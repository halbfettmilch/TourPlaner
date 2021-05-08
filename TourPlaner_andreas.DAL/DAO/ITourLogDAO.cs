
using System;
using System.Collections.Generic;
using TourPlaner_andreas.Models;

//DAO = DataAccessObject
namespace TourPlaner_andreas.DAL.DAO
{
    public interface ITourLogDAO
    {
        TourLog FindById(int LogId);

        TourLog AddNewItemLog(int logId, DateTime date, int maxVelocity, int minVelocity, int avVelocity, int caloriesBurnt, int duration, TourItem loggedItem);  //weil im Touritem steht die dazugehörige ID

        IEnumerable<TourLog> GetLogsForTourItem(TourItem item);

    }
}
