
using System.Collections.Generic;
using TourPlaner_andreas.Models;

//DAO = DataAccessObject
namespace TourPlaner_andreas.DAL.DAO
{
    public interface ITourLogDAO
    {
        TourLog FindById(int LogId);

        TourLog AddNewItemLog(string logText, TourItem item);  //weil im Touritem steht die dazugehörige ID

        IEnumerable<TourLog> GetLogsForTourItem(TourItem item);

    }
}
