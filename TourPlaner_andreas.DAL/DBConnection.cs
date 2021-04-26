using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlaner_andreas.Models;
using Npgsql;
using System.IO;
using System.Diagnostics;

namespace TourPlaner_andreas.DAL
{
    class DbConnection : DataAccess
    {
        //private string connectionString;
        
        
        public DbConnection()
        {   //C:\Users\Andre\source\repos\TourPlaner_andreas\TourPlaner_andreas\bin\Debug\net5.0-windows\config.txt'
            string path = "config.txt";
            // get connection string from config file: @"Server=localhost;Port=5432;User Id=postgres;Password=postgres;Database=tourplaner"
            string connectionString = File.ReadAllText(path);
            Debug.WriteLine(connectionString);
            // establish connection to DB
            using (NpgsqlConnection con = GetConnection(connectionString))
            {
                con.Open();
                if (con.State == ConnectionState.Open)
                {   Debug.WriteLine("connected to DB");
                   
                }
                else Debug.WriteLine("Not connected to DB");
                con.Close();
            }
        }

        public void AddTourToList(TourItem item)
        {
            // add insert/update SQL statement/query here
        }

        public void AddLogToItem(TourItem item, TourLog logs)
        {
            // add insert/update SQL statement/query here
        }

        public List<TourItem> GetItems()
        {
            // add select SQL query here
            return new List<TourItem>() {
                new TourItem() { Name = "Item1" },
                new TourItem() { Name = "Item2" },
                new TourItem() { Name = "Another" },
                new TourItem() { Name = "SWEI" },
                new TourItem() { Name = "FHTW" }
            };
        }
        public static NpgsqlConnection GetConnection(string connectionString)
        {
            return new NpgsqlConnection(connectionString);
        }
    }
}
