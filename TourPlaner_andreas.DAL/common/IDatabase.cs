
using System.Data;
using System.Data.Common;


namespace TourPlaner_andreas.DAL.common
{
    public interface IDatabase
    {
        DbCommand createCommand(string genericCommandText);
        int DeclareParameter(DbCommand command, string name, DbType type);
        void DefineParameter(DbCommand command, string name, DbType type, object value);
        void SetParameter(DbCommand command, string name, object value);
        IDataReader ExecuteReader(DbCommand command);
        int ExecuteScalar(DbCommand command);
    }
}
