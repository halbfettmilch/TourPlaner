using System;
using System.Collections.ObjectModel;
using NUnit.Framework;
using TourPlaner_andreas.BL;
using TourPlaner_andreas.Models;

namespace TourPlanerAndreasTests
{
    public class BuisnessLayerTests
    {
        private static readonly TourItem tourItem1 = new(5, "correct", "Wien", "Bratislava", DateTime.Now, 5, 4,
            "nice tour");

        private static readonly TourItem tourItem2 = new(2, "Some Value Empty", "Wien", "\"\"", DateTime.Now, 5, 4, "");
        private static readonly TourItem tourItem3 = new(1, "", "", "\"\"", DateTime.Now, 5, 4, "");
        private static readonly TourLog logItem1 = new(5, DateTime.Now, 5, 5, 5, 5, 5, "andi", "comment", tourItem1);
        private static readonly TourLog logItem2 = new(6, DateTime.Now, 5, 5, 5, 5, 5, "\"\"", "\"\"", tourItem1);
        private static readonly TourLog logItem3 = new(7, DateTime.Now, 5, 5, 5, 5, 5, "andi", "comment", tourItem2);
        private static TourLog logItem4 = new(8, DateTime.Now, 5, 5, 5, 5, 5, "", "\"\"", tourItem3);
        public ObservableCollection<TourLog> logs;
        private IAppManager manager;

        [SetUp]
        public void Setup()
        {
            logs = new ObservableCollection<TourLog>();
            var manager = AppManagerFactory.GetFactoryManager();
        }


        [Test]
        public void getManagerTest()
        {
           
            IAppManager testmanager= AppManagerFactory.GetFactoryManager();
            Assert.AreEqual(testmanager, manager);
        }

        [Test]
        public void ReportFails1()
        {
            var manager = AppManagerFactory.GetFactoryManager();
            var check = manager.CreateTourPdf(tourItem2);
            Assert.AreEqual(false, check);
        }

        [Test]
        public void ReportFails2()
        {
            var manager = AppManagerFactory.GetFactoryManager();
            var check = manager.CreateTourPdf(tourItem3);
            Assert.AreEqual(false, check);
        }

        [Test]
        public void ReportFails3()
        {
            var manager = AppManagerFactory.GetFactoryManager();
            var check = manager.CreateTourPdf(new TourItem(5, ",", "", "\"\"", DateTime.Now, 5, 5, ""));
            Assert.AreEqual(false, check);
        }

        [Test]
        public void ReportLogsWorks()
        {
            logs.Add(logItem1);
            var manager = AppManagerFactory.GetFactoryManager();
            var check = manager.CreateTourLogsPdf(logs, tourItem1);
            Assert.AreEqual(true, check);
        }

        [Test]
        public void ReportLogFails1()
        {
            logs.Add(logItem1);
            logs.Add(logItem2);
            logs.Add(logItem3);
            var manager = AppManagerFactory.GetFactoryManager();
            var check = manager.CreateTourLogsPdf(logs, tourItem1);
            Assert.AreEqual(false, check);
        }

        [Test]
        public void ReportLogWorks2()
        {
            logs.Add(logItem1);
            logs.Add(logItem3);
            var manager = AppManagerFactory.GetFactoryManager();
            var check = manager.CreateTourLogsPdf(logs, tourItem1);
            Assert.AreEqual(true, check);
        }

        [Test]
        public void ReportLogFails3()
        {
            logs.Add(logItem2);
            logs.Add(logItem3);
            logs.Add(logItem3);
            var manager = AppManagerFactory.GetFactoryManager();
            var check = manager.CreateTourLogsPdf(logs, tourItem1);
            Assert.AreEqual(false, check);
        }
    }
}