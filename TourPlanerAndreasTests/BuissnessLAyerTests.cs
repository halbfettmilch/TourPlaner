using System;
using System.Collections.ObjectModel;
using Moq;
using NUnit.Framework;
using TourPlaner.DataAcessLayer.FileAccess;
using TourPlaner_andreas.BL;
using TourPlaner_andreas.DAL.common;
using TourPlaner_andreas.Models;

namespace TourPlanerAndreasTests
{
    public class BuissnessLayerTests
    {
        private IAppManager manager;
        private static readonly TourItem tourItem1 = new(5, "correct", "Wien", "Bratislava", DateTime.Now, 5, 4,
            "nice tour");

        private static readonly TourItem tourItem2 = new(2, "Some Value Empty", "Wien", "\"\"", DateTime.Now, 5, 4, "");

        [SetUp]
        public void Setup()
        {
            
            var manager = AppManagerFactory.GetFactoryManager();
        }
        [Test]
        public void AddItemTestFail1()
        {
            var manager = AppManagerFactory.GetFactoryManager();
            var check = manager.checkForEmptyTour("\"\"", "\"\"", "\"\"", DateTime.Today, 5, 5, "\"\"");
            Assert.AreEqual(false, check);
        }
        [Test]
        public void AddItemTestFail2()
        {
            var manager = AppManagerFactory.GetFactoryManager();
            var check = manager.checkForEmptyTour("dasd", "\"\"", "asdasdasd", DateTime.Today, 5, 5, "asdasdasd");
            Assert.AreEqual(false, check);
        }
        [Test]
        public void AddItemTestSuccess()
        {
            var manager = AppManagerFactory.GetFactoryManager();
            var check = manager.checkForEmptyTour("dasd", "dasdas", "asdasdasd", DateTime.Today, 5, 5, "asdasdasd");
            Assert.AreEqual(true, check);
        }
        [Test]
        public void AddTourItemTest()
        {
            var manager = AppManagerFactory.GetFactoryManager();
            var check = manager.CreateItem("\"\"", "\"\"", "\"\"", DateTime.Today, 5, 5, "\"\"");
            Assert.AreEqual(null, check);
        }
        [Test]
        public void AddLogItemTest()
        {
            var manager = AppManagerFactory.GetFactoryManager();
            var check = manager.CreateItemLog(DateTime.Today, 5, 5, 5, 5, 5, "\"\"", "\"\"",tourItem2);
            Assert.AreEqual(null, check);
        }
    }



    }
