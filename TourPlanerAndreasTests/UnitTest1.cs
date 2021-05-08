using System.Data.Common;
using System.Runtime.CompilerServices;
using NUnit.Framework;
using TourPlaner_andreas.DAL.common;

namespace TourPlanerAndreasTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void DatabaseTests()
        { IDatabase database= DALFactory.GetDatabase();
        string SQL_GET_ALL_ITEMS = "SELECT * from  public.\"tours\";";
        DbCommand itemsCommand = database.createCommand(SQL_GET_ALL_ITEMS);
        Assert.AreEqual(0, itemsCommand);

        }

        public void AddItemTest()
        {
            IDatabase database = DALFactory.GetDatabase();
            string SQL_GET_ALL_ITEMS = "SELECT * from  public.\"tours\";";
            DbCommand itemsCommand = database.createCommand(SQL_GET_ALL_ITEMS);
            Assert.AreEqual(0, itemsCommand);

        }
    }
}