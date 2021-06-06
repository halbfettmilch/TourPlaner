using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using PdfSharp.Drawing;
using TourPlaner_andreas.Models;
using TourPlaner_andreas.DAL;
using TourPlaner_andreas.DAL.common;
using TourPlaner_andreas.DAL.DAO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TourPlaner.DataAcessLayer.FileAccess;
using Image = System.Drawing.Image;


namespace TourPlaner_andreas.BL {
    internal class AppManagerFactoryImpl : IAppManager
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private FileAccess access = new FileAccess("C:\\Users\\Andre\\source\\repos\\TourPlaner_andreas\\TourPlaner_andreas\\TourFiles\\");
        public IEnumerable<TourItem> GetItems(TourFolder folder)
        {
            ITourItemDAO tourItemDAO = DALFactory.CreateTourItemDAO();
            return tourItemDAO.GetItems(folder);
        }
       
        public TourFolder GetTourFolder(string url)
        {   
            // usally located somewhere on the disk
            return new TourFolder();
        }

        public IEnumerable<TourLog> GetLogsForTourItem(TourItem touritem)
        {
            ITourLogDAO tourLogDao = DALFactory.CreateTourLogDAO();
            return tourLogDao.GetLogsForTourItem(touritem);
        }

        public IEnumerable<TourItem> SearchForItems(string itemName, TourFolder folder, bool caseSensitive = false)
        {
            IEnumerable<TourItem> items = GetItems(folder);

            if (caseSensitive)
            {
                return items.Where(x => x.Name.Contains(itemName));
            }
            return items.Where(x => x.Name.ToLower().Contains(itemName.ToLower()));
        }

        public TourLog CreateItemLog( DateTime date, int maxVelocity, int minVelocity, int avVelocity, int caloriesBurnt, int duration, string author, string comment, TourItem loggedItem)
        {
            Random rnd = new Random();
            ITourLogDAO tourLogDAO = DALFactory.CreateTourLogDAO();
            TourLog logToReturn = null;
            do
            {
                int logID = rnd.Next(999999);
                logToReturn = tourLogDAO.AddNewItemLog(logID,date, maxVelocity, minVelocity, avVelocity, caloriesBurnt,
                    duration, author,comment, loggedItem);
            } while (logToReturn == null);

            return logToReturn;
        }

        public TourItem CreateItem( string name, string fromstart, string to, DateTime creationTime, int tourLength, int duration, string description)
        {

           
             //_____________________________________________________
             Random rnd = new Random();
            ITourItemDAO tourItemDAO = DALFactory.CreateTourItemDAO();
            TourItem itemToReturn=null;
            do
            {
                int id = rnd.Next(999999);
                itemToReturn = tourItemDAO.AddNewItem(id, name, fromstart, to, creationTime, tourLength, duration, description);
            } while (itemToReturn == null);
            getApiAsync(itemToReturn);
            return itemToReturn;
        }
        public void DeleteTourWithId(TourItem touritem)
        {
            ITourItemDAO tourItemDAO = DALFactory.CreateTourItemDAO();
            tourItemDAO.DeleteById(touritem.TourID);
        }
        public void DeleteLogWithId(TourLog tourLog)
        {
            ITourLogDAO tourLogDao = DALFactory.CreateTourLogDAO();
            tourLogDao.DeleteById(tourLog.LogId);
        }

         public bool CreateTourPdf(TourItem tourItem)
        {
            
            try
            {
                if (tourItem.Name != "\"\"" && tourItem.CreationTime.ToString() != "\"\"" && tourItem.Fromstart != "\"\"" && tourItem.To != "\"\"" && tourItem.TourLength.ToString() != "\"\"" && tourItem.Duration.ToString() != "\"\"" && tourItem.Description != "\"\"")
                { 
                
                PdfWriter writer = new PdfWriter("Reports.pdf");
                PdfDocument pdf = new PdfDocument(writer);
                Document document = new Document(pdf);



                Paragraph header = new Paragraph("Tour Report: ")
                    .SetFontSize(30);

                document.Add(header);



                Paragraph lines1 =
                    new Paragraph("-------------------------------------------------------------------------- ")
                        .SetFontSize(20);
                Paragraph tmpContent1 = new Paragraph("Name: " + tourItem.Name)
                    .SetFontSize(10);
                Paragraph tmpContent2 = new Paragraph("Created on: " + tourItem.CreationTime.ToString())
                    .SetFontSize(10);
                Paragraph tmpContent3 = new Paragraph("Tour length " + tourItem.TourLength.ToString() + " km")
                    .SetFontSize(10);
                Paragraph tmpContent4 = new Paragraph("Expected duration " + tourItem.Duration.ToString() + " h")
                    .SetFontSize(10);
                Paragraph tmpContent5 = new Paragraph("Description: " + tourItem.Description)
                    .SetFontSize(8);
                Paragraph lines2 =
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
                    throw new ArgumentException("Tour length or Expected Duration of "+ tourItem.Name +" in not in the right Format");
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
            PdfWriter writer = new PdfWriter("Reports.pdf");
            PdfDocument pdf = new PdfDocument(writer);
            Document document = new Document(pdf);
            
            try
            {

                if (tourItem.Name != "\"\"" && tourItem.CreationTime.ToString() != "\"\"" && tourItem.Fromstart != "\"\"" && tourItem.To != "\"\"" && tourItem.TourLength.ToString() != "\"\"" && tourItem.Duration.ToString() != "\"\"" && tourItem.Description != "\"\"")
                {

                   



                    Paragraph header = new Paragraph("Tour Report: ")
                        .SetFontSize(30);

                    document.Add(header);



                    Paragraph lines = new Paragraph("-------------------------------------------------------------------------- ")
                            .SetFontSize(20);
                    Paragraph tmpContent1 = new Paragraph("Name: " + tourItem.Name)
                        .SetFontSize(10);
                    Paragraph tmpContent2 = new Paragraph("Created on: " + tourItem.CreationTime.ToString())
                        .SetFontSize(10);
                    Paragraph tmpContent3 = new Paragraph("Tour length " + tourItem.TourLength.ToString() + " km")
                        .SetFontSize(10);
                    Paragraph tmpContent4 = new Paragraph("Expected duration " + tourItem.Duration.ToString() + " h")
                        .SetFontSize(10);
                    Paragraph tmpContent5 = new Paragraph("Description: " + tourItem.Description)
                        .SetFontSize(8);
                   

                    document.Add(lines);
                    document.Add(tmpContent1);
                    document.Add(tmpContent2);
                    document.Add(tmpContent3);
                    document.Add(tmpContent4);
                    document.Add(tmpContent5);
                    document.Add(lines);



                   
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

            foreach (TourLog tourLog in tourLogs)
            {
                try
                {
                    if (tourLog.LogId.ToString() != "\"\"" && tourLog.Date.ToString() != "\"\"" && tourLog.MaxVelocity.ToString() != "\"\"" && tourLog.MinVelocity.ToString() != "\"\"" && tourLog.AvVelocity.ToString() != "\"\"" && tourLog.CaloriesBurnt.ToString() != "\"\"" && tourLog.Duration.ToString() != "\"\"" && tourLog.Author != "\"\"" && tourLog.Comment != "\"\"")
                    {
                        Paragraph tmplog1 = new Paragraph("LogID: " + tourLog.LogId.ToString())
                            .SetFontSize(20);
                        Paragraph tmplog2 = new Paragraph("Date: " + tourLog.Date.ToString())
                            .SetFontSize(10);
                        Paragraph tmplog3 = new Paragraph("Max Velocity: " + tourLog.MaxVelocity.ToString())
                            .SetFontSize(10);
                        Paragraph tmplog4 = new Paragraph("Min Velocity: " + tourLog.MinVelocity.ToString())
                            .SetFontSize(10);
                        Paragraph tmplog5 = new Paragraph("Av Velocity: " + tourLog.AvVelocity.ToString())
                            .SetFontSize(10);
                        Paragraph tmplog6 = new Paragraph("CaloriesBurnt: " + tourLog.CaloriesBurnt.ToString())
                            .SetFontSize(10);
                        Paragraph tmplog7 = new Paragraph("Duration: " + tourLog.Duration.ToString())
                            .SetFontSize(10);
                        Paragraph tmplog8 = new Paragraph("Author: " + tourLog.Author)
                            .SetFontSize(10);
                        Paragraph tmplog9 = new Paragraph("Comment: " + tourLog.Comment)
                            .SetFontSize(8);
                        Paragraph lines =
                            new Paragraph("-------------------------------------------------------------------------- ")
                                .SetFontSize(20);

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
                        throw new NullReferenceException("Some Value of Log with ID: " + tourLog.LogId + " is empty");
                    }
                }
                catch (Exception ex)
                {
                    document.Close();
                    log.Error(ex.Message);
                    return false;
                }
            }



            document.Close();
            return true;
        }

        private async System.Threading.Tasks.Task getApiAsync(TourItem item)
        {
            AppManagerWebApi mapi = new AppManagerWebApi();
            string responseBody = await mapi.getApiRoute(item);
            var data = (JObject)JsonConvert.DeserializeObject(responseBody);
            JObject route = data["route"].Value<JObject>();
            string sessionId = route["sessionId"].Value<string>();
            JObject boundingBox = route["boundingBox"].Value<JObject>();
            mapi.getApiImage(sessionId, boundingBox,item.TourID);
        }

        public Image GetImage(int id)
        {
            return System.Drawing.Image.FromFile("C:\\Users\\Andre\\source\\repos\\TourPlaner_andreas\\TourPlaner_andreas\\TourPics\\" + id + ".jpg");
        }

        public int ExportFile(ObservableCollection<TourLog> tourLogs,TourItem item)
        {
            int itemId=access.CreateNewTourItemFile(item.TourID, item.Name, item.Fromstart, item.To, item.CreationTime,
                item.TourLength, item.Duration, item.Description);
            foreach (TourLog tourLog in tourLogs)
            {   
                access.CreateNewTourLogFile(tourLog.LogId, tourLog.Date, tourLog.MaxVelocity, tourLog.MinVelocity,
                    tourLog.AvVelocity, tourLog.CaloriesBurnt, tourLog.Duration, tourLog.Author, tourLog.Comment,
                    tourLog.LogTourItem);
            }
            return itemId;
        }
        public TourItem ImportFile(TourItem item)
        {
           
            return null;
        }

    }


        
    
}
