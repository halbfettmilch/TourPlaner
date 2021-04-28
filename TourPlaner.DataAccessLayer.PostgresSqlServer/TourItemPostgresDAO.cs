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
        private const string SQL_FIND_BY_ID = "SELECT * from  public.\"tours\" WHERE \"TourID\"=@TourID;";
        private const string SQL_GET_ALL_ITEMS = "SELECT * from  public.\"tours\";";
        private const string SQL_INSERT_NEW_ITEM = "INSERT INTO public.\"tours\" (\"Name\", \"url\", \"CreationTime\") VALUES (@Name,@url,@CreationTime) RETURNING \"TourID\";" ; // need further work

        public TourItemPostgresDAO()
        {
            this.database = DALFactory.GetDatabase();
           
        }

        //needs further work

        public TourItem AddNewItem(string name, string url,string annotation, DateTime creationTime)
        {
            DbCommand insertCommand = database.createCommand(SQL_INSERT_NEW_ITEM);
            database.DefineParameter(insertCommand, "@Name",DbType.String, name);
            database.DefineParameter(insertCommand, "@Url", DbType.String, name);
            database.DefineParameter(insertCommand, "@Annotation", DbType.String, annotation);
            database.DefineParameter(insertCommand, "@CreationTime", DbType.String, creationTime.ToString());
            return FindById(database.ExecuteScalar(insertCommand));
        }

        public TourItem FindById(int itemId)
        {
            DbCommand findCommand = database.createCommand(SQL_FIND_BY_ID);
            database.DefineParameter(findCommand, "@Id",DbType.Int32, itemId);
            IEnumerable<TourItem> tourItems = QueryTourItemsFromDb(findCommand);
            return tourItems.FirstOrDefault();

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
                        (int)reader["TourID"],
                        (string)reader["Name"],
                        (string)reader["Url"],
                        DateTime.Parse(reader["CreationTime"].ToString()),
                        (int)reader["TourLength"],
                        (int)reader["Duration"]
                      
                        ));
                }
            }
            return tourItemList;
        }
    }
}
