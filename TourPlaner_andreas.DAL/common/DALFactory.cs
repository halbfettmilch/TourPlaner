using System;
using System.Configuration;
using System.Reflection;
using TourPlaner_andreas.DAL.DAO;

namespace TourPlaner_andreas.DAL.common
{
    public class DALFactory
    {
        private static readonly string AssemblyName;
        private static readonly Assembly dalAssembly;
        private static IDatabase database;
        private static IFileAccess fileAccess;
        private static readonly bool useFileSystem;

        static DALFactory()
        {
            useFileSystem = bool.Parse(ConfigurationManager.AppSettings["useFileSystem"]);
            AssemblyName = ConfigurationManager.AppSettings["DALSqlAssembly"];
            if (useFileSystem) AssemblyName = ConfigurationManager.AppSettings["DALFileAssembly"];
            dalAssembly = Assembly.Load(AssemblyName);
        }

        public static IDatabase GetDatabase()
        {
            if (database == null) database = CreateDatabase();

            return database;
        }

        private static IDatabase CreateDatabase()
        {
            var connectionString =
                ConfigurationManager.ConnectionStrings["PostGresSqlConnectionString"].ConnectionString;
            return CreateDatabase(connectionString);
        }

        private static IDatabase CreateDatabase(string connectionString)
        {
            var databaseClassName = AssemblyName + ".Database";
            var dbClass = dalAssembly.GetType(databaseClassName);
            return Activator.CreateInstance(dbClass, connectionString) as IDatabase;
        }

        public static IFileAccess GetFileAccess()
        {
            if (fileAccess == null) fileAccess = CreateFileAccess();

            return fileAccess;
        }

        private static IFileAccess CreateFileAccess()
        {
            var startFolder = ConfigurationManager.ConnectionStrings["StartFolderFilePath"].ConnectionString;
            return CreateFileAccess(startFolder);
        }

        private static IFileAccess CreateFileAccess(string startFolder)
        {
            var databaseClassName = AssemblyName + ".FileAccess";
            var dbClass = dalAssembly.GetType(databaseClassName);

            return Activator.CreateInstance(dbClass, startFolder) as IFileAccess;
        }

        // create tour item sql/file dao object
        public static ITourItemDAO CreateTourItemDAO()
        {
            var className = AssemblyName + ".TourItemPostgresDAO";
            if (useFileSystem) className = AssemblyName + ".TourItemFileDAO";

            var zoneType = dalAssembly.GetType(className);
            return Activator.CreateInstance(zoneType) as ITourItemDAO;
        }

        // create tour log sql/file dao object
        public static ITourLogDAO CreateTourLogDAO()
        {
            var className = AssemblyName + ".TourLogPostgresDAO";
            if (useFileSystem) className = AssemblyName + ".TourLogFileDAO";

            var zoneType = dalAssembly.GetType(className);
            return Activator.CreateInstance(zoneType) as ITourLogDAO;
        }
    }
}