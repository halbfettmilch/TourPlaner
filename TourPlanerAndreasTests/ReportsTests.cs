using System;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Runtime.CompilerServices;
using NUnit.Framework;
using TourPlaner_andreas.BL;
using TourPlaner_andreas.DAL.common;
using TourPlaner_andreas.Models;

namespace TourPlanerAndreasTests
{
    public class ReportTests
    {
        private static TourItem tourItem1 = new TourItem(5, "correct", "Wien","Bratislava", DateTime.Now, 5, 4, "nice tour");
        private static TourItem tourItem2 = new TourItem(2, "Some Value Empty", "Wien", "\"\"", DateTime.Now, 5, 4, "");
        private static TourItem tourItem3 = new TourItem(1, "", "", "\"\"", DateTime.Now, 5, 4, "");
        private static TourLog logItem1 = new TourLog(5, DateTime.Now, 5, 5, 5, 5, 5, "andi", "comment", tourItem1);
        private static TourLog logItem2 = new TourLog(6, DateTime.Now, 5, 5, 5, 5, 5, "\"\"", "\"\"", tourItem1);
        private static TourLog logItem3 = new TourLog(7, DateTime.Now, 5, 5, 5, 5, 5, "andi", "comment", tourItem2);
        private static TourLog logItem4 = new TourLog(8, DateTime.Now, 5, 5, 5, 5, 5, "", "\"\"", tourItem3);
        private IAppManager manager;
        public ObservableCollection<TourLog> logs;

        [SetUp]
        public void Setup()
        {
            logs = new ObservableCollection<TourLog>();
            IAppManager manager = AppManagerFactory.GetFactoryManager();
        }


        [Test]
        public void ReportWorks()
        {
            IAppManager manager = AppManagerFactory.GetFactoryManager();
            bool check = manager.CreateTourPdf(tourItem1);
            Assert.AreEqual(true, check);
        }
        [Test]
        public void ReportFails1()
        {
            IAppManager manager = AppManagerFactory.GetFactoryManager();
            bool check = manager.CreateTourPdf(tourItem2);
            Assert.AreEqual(false,check);
           
        }
        [Test]
        public void ReportFails2()
        {
            IAppManager manager = AppManagerFactory.GetFactoryManager();
            bool check = manager.CreateTourPdf(tourItem3);
            Assert.AreEqual(false, check);
            
        }
        [Test]
        public void ReportFails3()
        {
            IAppManager manager = AppManagerFactory.GetFactoryManager();
            bool check = manager.CreateTourPdf(new TourItem(5, ",", "", "\"\"", DateTime.Now, 5, 5, ""));
            Assert.AreEqual(false, check);
        }
        [Test]
        public void ReportLogsWorks()
        {
            logs.Add(logItem1);
            IAppManager manager = AppManagerFactory.GetFactoryManager();
            bool check = manager.CreateTourLogsPdf(logs, tourItem1);
            Assert.AreEqual(true, check);
        }
        [Test]
        public void ReportLogFails1()
        {
            logs.Add(logItem1);
            logs.Add(logItem2);
            logs.Add(logItem3);
            IAppManager manager = AppManagerFactory.GetFactoryManager();
            bool check = manager.CreateTourLogsPdf(logs, tourItem2);
            Assert.AreEqual(false, check);
        }
        [Test]
        public void ReportLogFails2()
        {
            logs.Add(logItem1);
            IAppManager manager = AppManagerFactory.GetFactoryManager();
            bool check = manager.CreateTourLogsPdf(logs, tourItem2);
            Assert.AreEqual(false, check);
          
        }
        [Test]
        public void ReportLogFails3()
        {
            logs.Add(logItem2);
            logs.Add(logItem3);
            logs.Add(logItem3);
            IAppManager manager = AppManagerFactory.GetFactoryManager();
            bool check = manager.CreateTourLogsPdf(logs, tourItem1);
            Assert.AreEqual(false, check);
        }



    }
}