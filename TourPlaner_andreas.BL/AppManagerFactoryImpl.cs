using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TourPlaner_andreas.DAL.common;
using TourPlaner_andreas.Models;
using FileAccess = TourPlaner.DataAcessLayer.FileAccess.FileAccess;
using Image = System.Drawing.Image;

namespace TourPlaner_andreas.BL
{
    internal class AppManagerFactoryImpl : IAppManager
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly FileAccess access =
            new("C:\\Users\\Andre\\source\\repos\\TourPlaner_andreas\\TourPlaner_andreas\\TourFiles\\");

        public IEnumerable<TourItem> GetItems(TourFolder folder)
        {
            var tourItemDAO = DALFactory.CreateTourItemDAO();
            return tourItemDAO.GetItems(folder);
        }

        public TourFolder GetTourFolder(string url)
        {
            // usally located somewhere on the disk
            return new();
        }

        public IEnumerable<TourLog> GetLogsForTourItem(TourItem touritem)
        {
            var tourLogDao = DALFactory.CreateTourLogDAO();
            return tourLogDao.GetLogsForTourItem(touritem);
        }

        public IEnumerable<TourItem> SearchForItems(string itemName, TourFolder folder, bool caseSensitive = false)
        {
            var items = GetItems(folder);

            if (caseSensitive) return items.Where(x => x.Name.Contains(itemName));
            return items.Where(x => x.Name.ToLower().Contains(itemName.ToLower()));
        }

        public TourLog CreateItemLog(DateTime date, int maxVelocity, int minVelocity, int avVelocity, int caloriesBurnt,
            int duration, string author, string comment, TourItem loggedItem)
        {
            try
            {
                if (checkForEmptyTour(loggedItem.Name, loggedItem.Fromstart, loggedItem.To, loggedItem.CreationTime,
                    loggedItem.TourLength, loggedItem.Duration, loggedItem.Description))
                {
                    var rnd = new Random();
                    var tourLogDAO = DALFactory.CreateTourLogDAO();
                    TourLog logToReturn = null;
                    do
                    {
                        var logID = rnd.Next(999999);
                        logToReturn = tourLogDAO.AddNewItemLog(logID, date, maxVelocity, minVelocity, avVelocity, caloriesBurnt,
                            duration, author, comment, loggedItem);
                    } while (logToReturn == null);

                    return logToReturn;
                }
                else throw new ArgumentException("Some Tour Infos are Empty");
            }
            catch (Exception e)
            {
                log.Error(e.Message);
            }

            return null;

        }

        public TourItem CreateItem(string name, string fromstart, string to, DateTime creationTime, int tourLength,
            int duration, string description)
        {
            try
            {
                if (checkForEmptyTour(name, fromstart, to, creationTime, tourLength, duration, description))
                {
                    var rnd = new Random();
                    var tourItemDAO = DALFactory.CreateTourItemDAO();
                    TourItem itemToReturn = null;
                    do
                    {
                        var id = rnd.Next(999999);
                        itemToReturn = tourItemDAO.AddNewItem(id, name, fromstart, to, creationTime, tourLength, duration,
                            description);
                    } while (itemToReturn == null);

                    getApiAsync(itemToReturn);
                    return itemToReturn;
                }
                else throw new ArgumentException("Some Tour Infos are Empty");
            }
            catch (Exception e)
            {
                log.Error(e.Message);
            }

            return null;

        }

        public bool checkForEmptyTour(string name, string fromstart, string to, DateTime creationTime, int tourLength,
            int duration, string description)
        {
            if (name != "\"\"" && creationTime.ToString() != "\"\"" &&
                fromstart != "\"\"" && to != "\"\"" && tourLength.ToString() != "\"\"" &&
                duration.ToString() != "\"\"" && description != "\"\"")
            {
                return true;
            }
           else return false;
        }

        public void DeleteTourWithId(TourItem touritem)
        {
            try
            {
                var tourItemDAO = DALFactory.CreateTourItemDAO();
                tourItemDAO.DeleteById(touritem.TourID);
                access.DeletePictureOfTour(touritem.TourID);
                
            }
            catch (Exception ex)
            {
               log.Error(ex.Message);
                
            }
           
        }


        public void DeleteLogWithId(TourLog tourLog)
        {
            var tourLogDao = DALFactory.CreateTourLogDAO();
            tourLogDao.DeleteById(tourLog.LogId);
        }

        public bool CreateTourPdf(TourItem tourItem) // is in this class because of system.drawings error
        {
            try
            {
                return access.CreateTourPdf(tourItem);
            }
            catch (Exception e)
            {
                log.Error(e.Message);
            }

            return false;

        }

        public bool CreateTourLogsPdf(ObservableCollection<TourLog> tourLogs, TourItem tourItem)
        {
            try
            {
                return access.CreateTourLogsPdf(tourLogs, tourItem);
            }
            catch (Exception e)
            {
               log.Error(e.Message);
            }

            return false;

        }


        public int ExportFile(ObservableCollection<TourLog> tourLogs, TourItem item,string path)
        {
            access.CreateTourItemLog(tourLogs, item,path);
            return item.TourID;

        }

        public TourItem ImportFile(string path)
        {
            string[] lines = access.ImportFile(path);
            TourItem tourItem=new TourItem((int.Parse(lines[0])), lines[1], lines[2], lines[3], DateTime.Parse(lines[4]), int.Parse(lines[5]), int.Parse(lines[6]), lines[7]);
            TourItem newtourItem=CreateItem(tourItem.Name, tourItem.Fromstart, tourItem.To, tourItem.CreationTime, tourItem.TourLength,
                tourItem.Duration, tourItem.Description);
            for (int i = 8; i < lines.Length; i+=10)
            {
                CreateItemLog(DateTime.Parse(lines[i + 1]), int.Parse(lines[i + 2]), int.Parse(lines[i + 3]),
                    int.Parse(lines[i + 4]), int.Parse(lines[i + 5]), int.Parse(lines[i + 6]), lines[i + 7],
                    lines[i + 8], newtourItem);
            }
            
            
            foreach (string line in lines)
            {
                // Use a tab to indent each line of the file.
                Debug.WriteLine("\t" + line);
            }
            return null;
        }

        private async Task getApiAsync(TourItem item)
        {
            var mapi = new AppManagerWebApi();
            var responseBody = await mapi.getApiRoute(item);
            var data = (JObject) JsonConvert.DeserializeObject(responseBody);
            var route = data["route"].Value<JObject>();
            var sessionId = route["sessionId"].Value<string>();
            var boundingBox = route["boundingBox"].Value<JObject>();
            mapi.getApiImage(sessionId, boundingBox, item.TourID);
        }

      

    }
}