using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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


namespace TourPlaner_andreas.BL {
    internal class AppManagerFactoryImpl : IAppManager
    {

      

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

        public TourLog CreateItemLog( DateTime date, int maxVelocity, int minVelocity, int avVelocity, int caloriesBurnt, int duration, TourItem loggedItem)
        {
            Random rnd = new Random();
            ITourLogDAO tourLogDAO = DALFactory.CreateTourLogDAO();
            TourLog logToReturn = null;
            do
            {
                int logID = rnd.Next(999999);
                logToReturn = tourLogDAO.AddNewItemLog(logID,date, maxVelocity, minVelocity, avVelocity, caloriesBurnt,
                    duration, loggedItem);
            } while (logToReturn == null);

            return logToReturn;
        }

        public TourItem CreateItem( string name, string url, DateTime creationTime, int tourLength, int duration)
        {
            Random rnd = new Random();
            ITourItemDAO tourItemDAO = DALFactory.CreateTourItemDAO();
            TourItem itemToReturn=null;
            do
            {
                int id = rnd.Next(999999);
                itemToReturn = tourItemDAO.AddNewItem(id, name, url, creationTime, tourLength, duration);
            } while (itemToReturn == null);
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

        public void CreatePdf(IEnumerable<TourItem> tourItems)
        {
            
            PdfWriter writer = new PdfWriter("Reports.pdf");
            PdfDocument pdf = new PdfDocument(writer);
            Document document = new Document(pdf);

            
           
            Paragraph header = new Paragraph("hello world")
                .SetFontSize(20);

            document.Add(header);


            foreach (var item in tourItems)
            {
                
                Paragraph tmpContent = new Paragraph(item.Name)
                    .SetFontSize(20);

                document.Add(tmpContent);
            }


            document.Close();
        }

       
    }

        
    
}
