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
        private static string AssemblyName;
        private static Assembly dalAssembly;
        private static IDatabase database;
        private static IFileAccess fileAccess;
        private static bool useFileSystem;

        static DALFactory()
        {   useFileSystem =bool.Parse(ConfigurationManager.AppSettings["useFileSystem"]);
            AssemblyName = ConfigurationManager.AppSettings["DALSqlAssembly"];
            if (useFileSystem)
            {
                AssemblyName = ConfigurationManager.AppSettings["DALFileAssembly"];
            }
            dalAssembly = Assembly.Load(AssemblyName);
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
            string databaseClassName = AssemblyName + ".Database";
            Type dbClass = dalAssembly.GetType(databaseClassName);
            return Activator.CreateInstance(dbClass, new object[] {connectionString}) as IDatabase;
        }

        public static IFileAccess GetFileAccess()
        {
            if (fileAccess == null)
            {
                fileAccess = CreateFileAccess();
            }

            return fileAccess;
        }

        private static IFileAccess CreateFileAccess()
        {
            string startFolder = ConfigurationManager.ConnectionStrings["StartFolderFilePath"].ConnectionString;
            return CreateFileAccess(startFolder);
        }

        private static IFileAccess CreateFileAccess(string startFolder)
        {
            string databaseClassName = AssemblyName + ".FileAccess";
            Type dbClass = dalAssembly.GetType(databaseClassName);

            return Activator.CreateInstance(dbClass,
                new object[] { startFolder }) as IFileAccess;
        }
        // create tour item sql/file dao object
        public static ITourItemDAO CreateTourItemDAO()
        {
            string className = AssemblyName + ".TourItemPostgresDAO";
            if (useFileSystem)
            {
                className = AssemblyName + ".TourItemFileDAO";
            }

            Type zoneType = dalAssembly.GetType(className);
            return Activator.CreateInstance(zoneType) as ITourItemDAO;
        }

        // create tour log sql/file dao object
        public static ITourLogDAO CreateTourLogDAO()
        {
            string className = AssemblyName + ".TourLogPostgresDAO";
            if (useFileSystem)
            {
                className = AssemblyName + ".TourLogFileDAO";
            }

            Type zoneType = dalAssembly.GetType(className);
            return Activator.CreateInstance(zoneType) as ITourLogDAO;
        }



    }
}
