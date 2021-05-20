using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Annotations;
using TourPlaner_andreas.DAL.common;
using TourPlaner_andreas.DAL.DAO;
using TourPlaner_andreas.Models;

namespace TourPlaner.DataAcessLayer.FileAccess
{
    public class TourItemFileDAO : ITourItemDAO
    {
        private IFileAccess fileAccess;

        public TourItemFileDAO()
        {
            this.fileAccess = DALFactory.GetFileAccess();
        }

        public TourItem AddNewItem(int tourId, string name, string url, DateTime creationTime, int tourLength, int duration)
        {
            if (FindById(tourId) != null)
            {
                return null;
            }
          
                int id = fileAccess.CreateNewTourItemFile(tourId, name, url, creationTime, tourLength, duration);
                return FindById(id);
            
               
        }

        public TourItem FindById(int itemId)
        {
            IEnumerable<FileInfo> foundFiles = fileAccess.SearchFiles(itemId.ToString(), FileTypes.Touritem);
            return QueryTourItemFromFileSystem(foundFiles).FirstOrDefault();
        }

        public void DeleteById(int itemId)
        {
            //Keine Ahnung wie das gehen soll!
        }

        public IEnumerable<TourItem> GetItems(TourFolder folder)
        {
            IEnumerable<FileInfo> foundFiles = fileAccess.GetAllFiles(FileTypes.Touritem);
            return QueryTourItemFromFileSystem(foundFiles);
        }

        private IEnumerable<TourItem> QueryTourItemFromFileSystem(IEnumerable<FileInfo> foundFiles)
        {
            List<TourItem> foundTourItems = new List<TourItem>();

            foreach (FileInfo file in foundFiles)
            {
                string[] fileLines = File.ReadAllLines(file.FullName);
                foundTourItems.Add(new TourItem(
                    int.Parse(fileLines[0]),
                    fileLines[1],
                    fileLines[2],
                    DateTime.Parse(fileLines[3]),
                    int.Parse(fileLines[4]),
                    int.Parse(fileLines[5])

                    ));
            }

            return foundTourItems;
        }
    }
}
