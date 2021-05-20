using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlaner_andreas.DAL.common;
using TourPlaner_andreas.DAL.DAO;
using TourPlaner_andreas.Models;

namespace TourPlaner.DataAccessLayer.PostgresSqlServer
{
    public class TourItemPostgresDAO : ITourItemDAO
    {
        private IDatabase database;
        private const string SQL_FIND_BY_ID = "SELECT * from  public.\"tours\" WHERE \"tourid\"=@tourid;";
        private const string SQL_GET_ALL_ITEMS = "SELECT * from  public.\"tours\";";
        private const string SQL_INSERT_NEW_ITEM = "INSERT INTO public.\"tours\" (\"tourid\", \"name\", \"url\",\"creationtime\",\"tourlength\",\"duration\") VALUES (@TourId,@name,@url,@CreationTime,@TourLength,@Duration) RETURNING \"tourid\";"; // need further work
        private const string SQL_DELETE_TOUR = "DELETE from public.\"tours\" WHERE \"tourid\"=@tourid;"; //work in Progress
        public TourItemPostgresDAO()
        {
            this.database = DALFactory.GetDatabase();
           
        }

        

        public TourItem AddNewItem(int tourId,string name, string url, DateTime creationTime, int tourLength, int duration)
        {
            if (FindById(tourId) != null)
            {
                return null;
            }
            DbCommand insertCommand = database.createCommand(SQL_INSERT_NEW_ITEM);
            database.DefineParameter(insertCommand, "@tourid",DbType.Int32, tourId);
            database.DefineParameter(insertCommand, "@name", DbType.String, name);
            database.DefineParameter(insertCommand, "@url", DbType.String, url);
            database.DefineParameter(insertCommand, "@creationtime", DbType.Date, creationTime);
            database.DefineParameter(insertCommand, "@tourlength", DbType.Int32, tourLength);
            database.DefineParameter(insertCommand, "@duration", DbType.Int32, duration);
            return FindById(database.ExecuteScalar(insertCommand));
        }

        public TourItem FindById(int itemId)
        {
            DbCommand findCommand = database.createCommand(SQL_FIND_BY_ID);
            database.DefineParameter(findCommand, "@tourid",DbType.Int32, itemId);
            IEnumerable<TourItem> tourItems = QueryTourItemsFromDb(findCommand);
            return tourItems.FirstOrDefault();

        }
        public void DeleteById(int itemId) //work in Progress
        {
            DbCommand deleteCommand = database.createCommand(SQL_DELETE_TOUR);
            database.DefineParameter(deleteCommand, "@tourid", DbType.Int32, itemId);
            IDataReader reader = database.ExecuteReader(deleteCommand);
            
        }

        public IEnumerable<TourItem> GetItems(TourFolder folder)
        {
            DbCommand itemsCommand = database.createCommand(SQL_GET_ALL_ITEMS);
            
            return QueryTourItemsFromDb(itemsCommand);
        }

        private IEnumerable<TourItem> QueryTourItemsFromDb(DbCommand command)
        {
            List<TourItem> tourItemList = new List<TourItem>();
            using (IDataReader reader= database.ExecuteReader(command))
            {
                while (reader.Read())
                {
                    tourItemList.Add(new TourItem(
                        (int)reader["tourid"],
                        (string)reader["name"],
                        (string)reader["url"],
                        DateTime.Parse(reader["creationtime"].ToString()),
                        (int)reader["tourlength"],
                        (int)reader["duration"]
                      
                        ));
                }
            }
            return tourItemList;
        }
    }
}
