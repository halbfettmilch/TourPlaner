using System;
using System.Collections.ObjectModel;
using Moq;
using NUnit.Framework;
using TourPlaner.DataAcessLayer.FileAccess;
using TourPlaner_andreas.DAL.common;
using TourPlaner_andreas.Models;

namespace TourPlanerAndreasTests
{
    public class FileAccessTests
    {
        private IFileAccess fileAccess = new FileAccess("C:\\Users\\Andre\\source\\repos\\TourPlaner_andreas\\TourPlaner_andreas\\Testdata\\");
        private static readonly TourItem tourItem1 = new(5, "correct", "Wien", "Bratislava", DateTime.Now, 5, 4,
            "nice tour");
        
        private static readonly TourItem tourItem2 = new(2, "Some Value Empty", "Wien", "\"\"", DateTime.Now, 5, 4, "");
        private static readonly TourItem tourItem3 = new(1, "", "", "\"\"", DateTime.Now, 5, 4, "");
        private static readonly TourLog logItem1 = new(5, DateTime.Now, 5, 5, 5, 5, 5, "andi", "comment", tourItem1);
        private static readonly TourLog logItem2 = new(6, DateTime.Now, 5, 5, 5, 5, 5, "\"\"", "\"\"", tourItem1);
        private static readonly TourLog logItem3 = new(7, DateTime.Now, 5, 5, 5, 5, 5, "andi", "comment", tourItem2);
        private static TourLog logItem4 = new(8, DateTime.Now, 5, 5, 5, 5, 5, "", "\"\"", tourItem3);
        public ObservableCollection<TourLog> logs;
        [SetUp]
        public void Setup()
        {
            logs = new ObservableCollection<TourLog>();
        }

        [Test]
        public void fileAccessTest1()
        {
            string path = "C:\\Users\\Andre\\source\\repos\\TourPlaner_andreas\\TourPlaner_andreas\\Testdata\\964011_TourItem.txt";
            var check=fileAccess.CreateTourItemLog(logs, tourItem1, path);
            Assert.AreEqual(true, check);
        }
        [Test]
        public void fileAccessTest2()
        {
            string path = "";
            var check = fileAccess.CreateTourItemLog(logs, tourItem1, path);
            Assert.AreEqual(false, check);
        }
        [Test]
        public void fileAccessTest3()
        {
            string path = "C:\\Users\\Andre\\source\\repos\\TourPlaner_andreas\\TourPlaner_andreas\\Testdata\\964011_TourItem.txt";
            var check = fileAccess.CreateTourItemLog(logs, tourItem2, path);
            Assert.AreEqual(false, check);
        }
        [Test]
        public void fileAccessTest4()
        {
            string path = "C:\\Users\\Andre\\source\\repos\\TourPlaner_andreas\\TourPlaner_andreas\\Testdata\\964011_TourItem.txt";
            var check = fileAccess.CreateTourItemLog(logs, tourItem3, path);
            Assert.AreEqual(false, check);
        }
        [Test]
        public void fileAccessTest5()
        {
            string path = "";
            var check = fileAccess.CreateTourItemLog(logs, tourItem3, path);
            Assert.AreEqual(false, check);
        }
        [Test]
        public void fileAccessTestrightFormat1()
        {
            string path = "C:\\Users\\Andre\\source\\repos\\TourPlaner_andreas\\TourPlaner_andreas\\Testdata\\964011_TourItem.txt";
            var check = fileAccess.ImportFile(path);
            Assert.AreEqual("5", check[0]);

        }
        [Test]
        public void fileAccessTestrightFormat2()
        {
            string path = "C:\\Users\\Andre\\source\\repos\\TourPlaner_andreas\\TourPlaner_andreas\\Testdata\\964011_TourItem.txt";
            var check = fileAccess.ImportFile(path);
            Assert.AreEqual("correct", check[1]);

        }
        [Test]
        public void fileAccessTestrightFormat3()
        {
            string path = "C:\\Users\\Andre\\source\\repos\\TourPlaner_andreas\\TourPlaner_andreas\\Testdata\\964011_TourItem.txt";
            var check = fileAccess.ImportFile(path);
            Assert.AreEqual("Wien", check[2]);

        }
        [Test]
        public void fileAccessTestrightFormat4()
        {
            string path = "C:\\Users\\Andre\\source\\repos\\TourPlaner_andreas\\TourPlaner_andreas\\Testdata\\964011_TourItem.txt";
            var check = fileAccess.ImportFile(path);
            Assert.AreEqual("Bratislava", check[3]);

        }
        [Test]
        public void fileAccessTestrightFormat6()
        {
            string path = "C:\\Users\\Andre\\source\\repos\\TourPlaner_andreas\\TourPlaner_andreas\\Testdata\\964011_TourItem.txt";
            var check = fileAccess.ImportFile(path);
            Assert.AreEqual("5", check[5]);

        }
        [Test]
        public void fileAccessTestrightFormat7()
        {
            string path = "C:\\Users\\Andre\\source\\repos\\TourPlaner_andreas\\TourPlaner_andreas\\Testdata\\964011_TourItem.txt";
            var check = fileAccess.ImportFile(path);
           
            Assert.AreEqual("4", check[6]);

        }
        [Test]
        public void fileAccessTestrightFormat8()
        {
            string path = "C:\\Users\\Andre\\source\\repos\\TourPlaner_andreas\\TourPlaner_andreas\\Testdata\\964011_TourItem.txt";
            var check = fileAccess.ImportFile(path);
            
            Assert.AreEqual( "nice tour", check[7]);

        }
        [Test]
        public void fileAccessDeletePicFail()
        {
            int id = 1;
            var check = fileAccess.DeletePictureOfTour(id);
            Assert.AreEqual(false, check);

        }
        [Test]
        public void fileAccessGetImagePathFail()
        {
            int id = 1;
            var check = fileAccess.GetImagePath(id);
            Assert.AreEqual(null, check);

        }
        [Test]
        public void fileAccessGetImagePathSuccess()
        {
            int id = 44129;
            var check = fileAccess.GetImagePath(id);
            Assert.AreEqual("C:\\Users\\Andre\\source\\repos\\TourPlaner_andreas\\TourPlaner_andreas\\Pics\\44129.jpg", check);

        }


    }
}