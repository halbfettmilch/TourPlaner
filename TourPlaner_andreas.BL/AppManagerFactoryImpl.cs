using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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

        public TourItem CreateItem(string name, string fromstart, string to, DateTime creationTime, int tourLength,
            int duration, string description)
        {
            //_____________________________________________________
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

        public void DeleteTourWithId(TourItem touritem)
        {
            var tourItemDAO = DALFactory.CreateTourItemDAO();
            tourItemDAO.DeleteById(touritem.TourID);
        }

        public void DeleteLogWithId(TourLog tourLog)
        {
            var tourLogDao = DALFactory.CreateTourLogDAO();
            tourLogDao.DeleteById(tourLog.LogId);
        }

        public bool CreateTourPdf(TourItem tourItem)
        {
            try
            {
                if (tourItem.Name != "\"\"" && tourItem.CreationTime.ToString() != "\"\"" &&
                    tourItem.Fromstart != "\"\"" && tourItem.To != "\"\"" && tourItem.TourLength.ToString() != "\"\"" &&
                    tourItem.Duration.ToString() != "\"\"" && tourItem.Description != "\"\"")
                {
                    var writer = new PdfWriter("Reports.pdf");
                    var pdf = new PdfDocument(writer);
                    var document = new Document(pdf);


                    var header = new Paragraph("Tour Report: ")
                        .SetFontSize(30);

                    document.Add(header);


                    var lines1 =
                        new Paragraph("-------------------------------------------------------------------------- ")
                            .SetFontSize(20);
                    var tmpContent1 = new Paragraph("Name: " + tourItem.Name)
                        .SetFontSize(10);
                    var tmpContent2 = new Paragraph("Created on: " + tourItem.CreationTime)
                        .SetFontSize(10);
                    var tmpContent3 = new Paragraph("Tour length " + tourItem.TourLength + " km")
                        .SetFontSize(10);
                    var tmpContent4 = new Paragraph("Expected duration " + tourItem.Duration + " h")
                        .SetFontSize(10);
                    var tmpContent5 = new Paragraph("Description: " + tourItem.Description)
                        .SetFontSize(8);
                    var lines2 =
                        new Paragraph("-------------------------------------------------------------------------- ")
                            .SetFontSize(20);

                    document.Add(lines1);
                    document.Add(tmpContent1);
                    document.Add(tmpContent2);
                    document.Add(tmpContent3);
                    document.Add(tmpContent4);
                    document.Add(tmpContent5);
                    document.Add(lines2);


                    document.Close();
                }


                else
                {
                    throw new ArgumentException("Tour length or Expected Duration of " + tourItem.Name +
                                                " in not in the right Format");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return false;
            }

            return true;
        }

        public bool CreateTourLogsPdf(ObservableCollection<TourLog> tourLogs, TourItem tourItem)
        {
            var writer = new PdfWriter("Reports.pdf");
            var pdf = new PdfDocument(writer);
            var document = new Document(pdf);

            try
            {
                if (tourItem.Name != "\"\"" && tourItem.CreationTime.ToString() != "\"\"" &&
                    tourItem.Fromstart != "\"\"" && tourItem.To != "\"\"" && tourItem.TourLength.ToString() != "\"\"" &&
                    tourItem.Duration.ToString() != "\"\"" && tourItem.Description != "\"\"")
                {
                    var header = new Paragraph("Tour Report: ")
                        .SetFontSize(30);

                    document.Add(header);


                    var lines = new Paragraph(
                            "-------------------------------------------------------------------------- ")
                        .SetFontSize(20);
                    var tmpContent1 = new Paragraph("Name: " + tourItem.Name)
                        .SetFontSize(10);
                    var tmpContent2 = new Paragraph("Created on: " + tourItem.CreationTime)
                        .SetFontSize(10);
                    var tmpContent3 = new Paragraph("Tour length " + tourItem.TourLength + " km")
                        .SetFontSize(10);
                    var tmpContent4 = new Paragraph("Expected duration " + tourItem.Duration + " h")
                        .SetFontSize(10);
                    var tmpContent5 = new Paragraph("Description: " + tourItem.Description)
                        .SetFontSize(8);


                    document.Add(lines);
                    document.Add(tmpContent1);
                    document.Add(tmpContent2);
                    document.Add(tmpContent3);
                    document.Add(tmpContent4);
                    document.Add(tmpContent5);
                    document.Add(lines);
                    foreach (var tourLog in tourLogs)
                        try
                        {
                            if (tourLog.LogId.ToString() != "\"\"" && tourLog.Date.ToString() != "\"\"" &&
                                tourLog.MaxVelocity.ToString() != "\"\"" && tourLog.MinVelocity.ToString() != "\"\"" &&
                                tourLog.AvVelocity.ToString() != "\"\"" && tourLog.CaloriesBurnt.ToString() != "\"\"" &&
                                tourLog.Duration.ToString() != "\"\"" && tourLog.Author != "\"\"" &&
                                tourLog.Comment != "\"\"")
                            {
                                var tmplog1 = new Paragraph("LogID: " + tourLog.LogId)
                                    .SetFontSize(20);
                                var tmplog2 = new Paragraph("Date: " + tourLog.Date)
                                    .SetFontSize(10);
                                var tmplog3 = new Paragraph("Max Velocity: " + tourLog.MaxVelocity)
                                    .SetFontSize(10);
                                var tmplog4 = new Paragraph("Min Velocity: " + tourLog.MinVelocity)
                                    .SetFontSize(10);
                                var tmplog5 = new Paragraph("Av Velocity: " + tourLog.AvVelocity)
                                    .SetFontSize(10);
                                var tmplog6 = new Paragraph("CaloriesBurnt: " + tourLog.CaloriesBurnt)
                                    .SetFontSize(10);
                                var tmplog7 = new Paragraph("Duration: " + tourLog.Duration)
                                    .SetFontSize(10);
                                var tmplog8 = new Paragraph("Author: " + tourLog.Author)
                                    .SetFontSize(10);
                                var tmplog9 = new Paragraph("Comment: " + tourLog.Comment)
                                    .SetFontSize(8);
                              

                                document.Add(tmplog1);
                                document.Add(tmplog2);
                                document.Add(tmplog3);
                                document.Add(tmplog4);
                                document.Add(tmplog5);
                                document.Add(tmplog6);
                                document.Add(tmplog7);
                                document.Add(tmplog8);
                                document.Add(tmplog9);
                                document.Add(lines);
                            }
                            else
                            {
                                throw new NullReferenceException("Some Value of Log with ID: " + tourLog.LogId +
                                                                 " is empty");
                            }


                        }
                        catch (Exception ex)
                        {
                            document.Close();
                            log.Error(ex.Message);
                            return false;
                        }



                }
                else
                {
                    throw new NullReferenceException("Some Value of " + tourItem.Name + " is empty");
                }
            }
            catch (Exception ex)
            {
                document.Close();
                log.Error(ex.Message);
                return false;
            }

          


            document.Close();
            return true;
        }

        public Image GetImage(int id)
        {
            return Image.FromFile(
                "C:\\Users\\Andre\\source\\repos\\TourPlaner_andreas\\TourPlaner_andreas\\TourPics\\" + id + ".jpg");
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