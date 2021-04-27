using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TourPlaner_andreas.DAL.DAO;

namespace TourPlaner_andreas.DAL.common
{
    public class DALFactory
    {
        private static string assamblyName;
        private static Assembly dalAssambly;
        private static IDatabase database;

        static DALFactory()
        {
            assamblyName = ConfigurationManager.AppSettings["DALSqlAssambly"];
            dalAssambly = Assembly.Load(assamblyName);
        }

        public static IDatabase GetDatabase()
        {
            if (database == null)
            {
                database = CreateDatabase();
            }

            return database;
        }

        private static IDatabase CreateDatabase()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["PostGresSqlConnectionString"].ConnectionString;
            return CreateDatabase(connectionString);
        }

        private static IDatabase CreateDatabase(string connectionString)
        {
            string databaseClassName = assamblyName + ".Database";
            Type dbClass = dalAssambly.GetType(databaseClassName);
            return Activator.CreateInstance(dbClass, new object[] {connectionString}) as IDatabase;
        }


        public static ITourItemDAO createTourItemDAO()
        {
            string className = assamblyName + ".TourItemPostgresDAO";
            Type touritemType = dalAssambly.GetType(className);
            return Activator.CreateInstance(touritemType) as ITourItemDAO;
        }

        public static ITourLogDAO createTourLogDAO()
        {
            string className = assamblyName + ".TourLogPostgresDAO";
            Type tourLogType = dalAssambly.GetType(className);
            return Activator.CreateInstance(tourLogType) as ITourLogDAO;
        }

    }
}
