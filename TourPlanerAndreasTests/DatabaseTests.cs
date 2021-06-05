using System.Data.Common;
using System.Runtime.CompilerServices;
using Moq;
using Moq.Protected;
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
           
            IDatabase database= DALFactory.GetDatabase();
        string SQL_GET_ALL_ITEMS = "SELECT * from  public.\"tours\";";
        DbCommand itemsCommand = database.createCommand(SQL_GET_ALL_ITEMS);
        Assert.AreEqual(0, itemsCommand);

        }
        [Test]
        public void AddItemTest()
        {
            var mockedDatabasae = new Mock<IDatabase>();
            IDatabase database = DALFactory.GetDatabase();
            string SQL_GET_ALL_ITEMS = "SELECT * from  public.\"tours\";";
            mockedDatabasae.Object.createCommand(SQL_GET_ALL_ITEMS);
            DbCommand itemsCommand = database.createCommand(SQL_GET_ALL_ITEMS);
            Assert.AreEqual(0, itemsCommand);

        }

    }
}