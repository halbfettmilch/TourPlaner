using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlaner_andreas.DAL.common;
using TourPlaner_andreas.DAL.DAO;
using TourPlaner_andreas.Models;

namespace TourPlaner.DataAcessLayer.FileAccess
{
    public class TourLogFileDAO : ITourLogDAO
    {
        private IFileAccess fileAccess;
        private ITourItemDAO tourItemDAO;


        public TourLogFileDAO()
        {
            this.fileAccess = DALFactory.GetFileAccess();
            this.tourItemDAO = DALFactory.CreateTourItemDAO();
        }

        public TourLog AddNewItemLog(int logId, DateTime date, int maxVelocity, int minVelocity, int avVelocity, int caloriesBurnt, int duration, TourItem loggedItem)
        {
            int id = fileAccess.CreateNewTourLogFile(logId, date, maxVelocity, minVelocity, avVelocity, caloriesBurnt,duration,loggedItem);
            return FindById(id);
        }

        public TourLog FindById(int logId)
        {
            IEnumerable<FileInfo> foundFiles = fileAccess.SearchFiles(logId.ToString(), FileTypes.TourLog);
            return QueryFromFileSystem(foundFiles).FirstOrDefault();
        }

        public IEnumerable<TourLog> GetLogsForTourItem(TourItem item)
        {
            IEnumerable<FileInfo> foundFiles = fileAccess.SearchFiles(item.TourID.ToString(), FileTypes.TourLog);
            return QueryFromFileSystem(foundFiles);
        }

        private IEnumerable<TourLog> QueryFromFileSystem(IEnumerable<FileInfo> foundFiles)
        {
            List<TourLog> foundTourLogs = new List<TourLog>();

            foreach (FileInfo file in foundFiles)
            {
                string[] fileLines = File.ReadAllLines(file.FullName);
                foundTourLogs.Add(new TourLog(
                    int.Parse(fileLines[0]),        
                    DateTime.Parse(fileLines[1]),
                    int.Parse(fileLines[2]),
                    int.Parse(fileLines[3]),
                    int.Parse(fileLines[4]),
                    int.Parse(fileLines[5]),
                    int.Parse(fileLines[6]),
                    tourItemDAO.FindById(int.Parse(fileLines[7]))         // mediaItemId
                ));
            }

            return foundTourLogs;
        }
    }
}
