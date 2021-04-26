using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TourPlaner_andreas.DAL.common;
using TourPlaner_andreas.DAL.DAO;
using TourPlaner_andreas.Models;

namespace TourPlaner.DataAccessLayer.PostgresSqlServer
{
    public class TourLogPostgresDAO : ITourLogDAO
    {

        private IDatabase database;
        private ITourItemDAO tourItemDao;
        private const string SQL_FIND_BY_MEDIAITEMID = "SELECT * from  public.\"tourLogs\" WHERE \"Id\"=@Id;";
        private const string SQL_GET_ALL_ITEMS = "SELECT * from  public.\"tourLogs\" WHERE \"TourLogId\" =@TourLogId;";
        private const string SQL_INSERT_NEW_ITEMLOG = "INSERT INTO public.\"tourLogs\" (\"LogText\", \"TourItemId\") VALUES (@LogText,@TourItemId) RETURNING \"Id\";";

        public TourLogPostgresDAO(IDatabase database, ITourItemDAO tourItemDao)
        {
            this.database = database;
            this.tourItemDao = tourItemDao;
        }

        public TourLog AddNewItemLog(string logText, TourItem item)

        {
            DbCommand insertCommand = database.createCommand(SQL_INSERT_NEW_ITEMLOG);
            database.DefineParameter(insertCommand, "@LogText", DbType.String, logText);
            database.DefineParameter(insertCommand, "@TourItemId", DbType.Int32, item.Id);
            return FindById(database.ExecuteScalar(insertCommand));


        }

        public TourLog FindById(int LogId)
        {
            DbCommand findCommand = database.createCommand(SQL_FIND_BY_MEDIAITEMID);
            database.DefineParameter(findCommand, "@Id", DbType.Int32, LogId);
            IEnumerable<TourLog> tourLogList = QueryTourLogsFromDb(findCommand);
            return tourLogList.FirstOrDefault();
        }

        public IEnumerable<TourLog> GetLogsForTourItem(TourItem item)
        {
            DbCommand getLogsCommand = database.createCommand(SQL_FIND_BY_MEDIAITEMID);
            database.DefineParameter(getLogsCommand, "@TourItemId", DbType.Int32, item.Id);
            return QueryTourLogsFromDb(getLogsCommand);
        }

        private IEnumerable<TourLog> QueryTourLogsFromDb(DbCommand command)
        {
            List<TourLog> tourLogList = new List<TourLog>();
            using (IDataReader reader = database.ExecuteReader(command))
            {
                while (reader.Read())
                {
                    tourLogList.Add(new TourLog(
                        (int)reader["Id"],
                        (string)reader["LogText"],
                       tourItemDao.FindById((int)reader["TourItemId"])
                    ));
                }
            }
            return tourLogList;
        }
    }
}
