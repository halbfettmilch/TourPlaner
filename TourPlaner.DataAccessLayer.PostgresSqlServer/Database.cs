using System;
using System.Data;
using System.Data.Common;
using Npgsql;
using TourPlaner_andreas.DAL.common;

namespace TourPlaner.DataAccessLayer.PostgresSqlServer
{
    public class Database : IDatabase
    {
        private string connectionString;

        public Database(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public DbCommand createCommand(string genericCommandText)
        {
            return new NpgsqlCommand(genericCommandText);
        }

        public int DeclareParameter(DbCommand command, string name, DbType type)
        {
            if (!command.Parameters.Contains(name))
            {
                int Index = command.Parameters.Add(new NpgsqlParameter(name, type));
                return Index;
            }

            throw new ArgumentException(string.Format("Parameter {0} already exists.", name)); // TIPP Units test ob dieese Exception wirklich geworfen wird!
        }


        public void DefineParameter(DbCommand command, string name, DbType type, object value)
        {
            int Index = DeclareParameter(command, name, type);
            command.Parameters[Index].Value = value;
        }

        public void SetParameter(DbCommand command, string name, object value)
        {
            if (command.Parameters.Contains(name))
            {
                command.Parameters[name].Value = value;
            }

            throw new ArgumentException(string.Format("Parameter {0} does not exist", name));
        }

        public IDataReader ExecuteReader(DbCommand command)
        {
            using (DbConnection connection = CreateConnection())
            {   
                command.Connection = connection;
                return command.ExecuteReader(CommandBehavior.CloseConnection);
            }
        }

        public int ExecuteScalar(DbCommand command)
        {
            using (DbConnection connection= CreateConnection())
            {
                command.Connection = connection;
                return  Convert.ToInt32(command.ExecuteScalar());
            }
        }

        private DbConnection CreateConnection()
        {
            DbConnection connection = new NpgsqlConnection(this.connectionString);
            connection.Open();
            return connection;
        }

       
    }
}
