using Moq;
using NUnit.Framework;
using TourPlaner_andreas.DAL.common;

namespace TourPlanerAndreasTests
{
    public class DatabaseTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void DatabaseTest()
        {
            var database = DALFactory.GetDatabase();
            var SQL_GET_ALL_ITEMS = "SELECT * from  public.\"tours\";";
            var itemsCommand = database.createCommand(SQL_GET_ALL_ITEMS);
            Assert.AreEqual(0, itemsCommand);
        }

        [Test]
        public void AddItemTest()
        {
            var mockedDatabasae = new Mock<IDatabase>();
            var database = DALFactory.GetDatabase();
            var SQL_GET_ALL_ITEMS = "SELECT * from  public.\"tours\";";
            mockedDatabasae.Object.createCommand(SQL_GET_ALL_ITEMS);
            var itemsCommand = database.createCommand(SQL_GET_ALL_ITEMS);
            Assert.AreEqual(0, itemsCommand);
        }
    }
}