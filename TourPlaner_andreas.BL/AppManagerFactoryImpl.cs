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
            //CreatePdf(tourItemDAO.GetItems(folder));
            return tourItemDAO.GetItems(folder);
            

        }

        public TourFolder GetTourFolder(string url)
        {
            // usally located somewhere on the disk
            return new TourFolder();
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

        public TourLog CreateItemLog(string logText, TourItem item)
        {
            ITourLogDAO tourLogDAO = DALFactory.CreateTourLogDAO();
            return tourLogDAO.AddNewItemLog(logText, item);
        }

        public TourItem CreateItem(int tourId, string name, string url, DateTime creationTime, int tourLength, int duration)
        {
            ITourItemDAO tourItemDAO = DALFactory.CreateTourItemDAO();
            return tourItemDAO.AddNewItem(tourId,name, url, creationTime, tourLength,duration);
        }

        public void CreatePdf(IEnumerable<TourItem> tourItems)
        {
            
            PdfWriter writer = new PdfWriter("demo.pdf");
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
