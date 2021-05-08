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
        private ITourItemDAO tourItemDAO;
        private const string SQL_FIND_BY_LOGITEMID = "SELECT * from  public.\"logs\" WHERE \"logid\"=@logid;";
        private const string SQL_GET_ALL_ITEMS = "SELECT * from  public.\"logs\" WHERE \"touritemid\" =@touritemid;";
        private const string SQL_INSERT_NEW_ITEMLOG = "INSERT INTO public.\"logs\" (\"logid\",\"date\",\"maxvelocity\",\"minvelocity\",\"avvelocity\",\"caloriesburnt\",\"duration\",\"touritemid\") VALUES (@logid,@date,@maxvelocity,@minvelocity,@avvelocity,@caloriesburnt,@duration,@touritemid) RETURNING \"logid\";";
        public TourLogPostgresDAO()
        {
            this.database = DALFactory.GetDatabase();
            this.tourItemDAO = DALFactory.CreateTourItemDAO();
        }

       

        public TourLog AddNewItemLog(int logId, DateTime date, int maxVelocity, int minVelocity, int avVelocity, int caloriesBurnt, int duration, TourItem loggedItem)

        {
            DbCommand insertCommand = database.createCommand(SQL_INSERT_NEW_ITEMLOG);
            database.DefineParameter(insertCommand, "@logid", DbType.Int32, logId);
            database.DefineParameter(insertCommand, "@date", DbType.Date, date);
            database.DefineParameter(insertCommand, "@maxvelocity", DbType.Int32, maxVelocity);
            database.DefineParameter(insertCommand, "@minvelocity", DbType.Int32, minVelocity);
            database.DefineParameter(insertCommand, "@avvelocity", DbType.Int32, avVelocity);
            database.DefineParameter(insertCommand, "@caloriesburnt", DbType.Int32, caloriesBurnt);
            database.DefineParameter(insertCommand, "@duration", DbType.Int32, duration);
            database.DefineParameter(insertCommand, "@touritemid", DbType.Int32, loggedItem.TourID);
            return FindById(database.ExecuteScalar(insertCommand));


        }

        public TourLog FindById(int LogId)
        {
            DbCommand findCommand = database.createCommand(SQL_FIND_BY_LOGITEMID);
            database.DefineParameter(findCommand, "@logid", DbType.Int32, LogId);
            IEnumerable<TourLog> tourLogList = QueryTourLogsFromDb(findCommand);
            return tourLogList.FirstOrDefault();
        }

        public IEnumerable<TourLog> GetLogsForTourItem(TourItem item)
        {
            DbCommand getLogsCommand = database.createCommand(SQL_GET_ALL_ITEMS);
            database.DefineParameter(getLogsCommand, "@touritemid", DbType.Int32, item.TourID);
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
                        (int)reader["logid"],
                        DateTime.Parse(reader["date"].ToString()),
                        (int)reader["maxvelocity"],
                        (int)reader["minvelocity"],
                        (int)reader["avvelocity"],
                        (int)reader["caloriesburnt"],
                        (int)reader["duration"],
                       tourItemDAO.FindById((int)reader["touritemid"])
                    ));
                }
            }
            return tourLogList;
        }
    }
}
