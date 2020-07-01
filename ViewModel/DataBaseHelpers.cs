using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using LibraryApp.Model;
using SQLite;


namespace LibraryApp.ViewModel
{
    public class DatabaseHelpers
    {
        public static string dbFile = Path.Combine(Environment.CurrentDirectory, "MyDatabase.db");

    
        public static bool Insert<T>(T item)
        {
          
            bool result = false;

            using (SQLiteConnection conn = new SQLiteConnection(dbFile))
            { 
                conn.CreateTable<T>();
                int numberOfRows = conn.Insert(item);
                if (numberOfRows > 0)
                    result = true;
            }

            return result;
        }

        public static bool Update<T>(T item)
        {
            bool result = false;

            using (SQLiteConnection conn = new SQLiteConnection(dbFile))
            {
                conn.CreateTable<T>();
                int numberOfRows = conn.Update(item);
                if (numberOfRows > 0)
                    result = true;
            }

            return result;
        }

        public static bool Delete<T>(T item)
        {
            bool result = false;

            using (SQLiteConnection conn = new SQLiteConnection(dbFile))
            {
                conn.CreateTable<T>();
                int numberOfRows = conn.Delete(item);
                if (numberOfRows > 0)
                    result = true;
            }

            return result;
        }

        public static List<T> Select<T>(string sql) where T : new()
        { 
            List<T> importedFiles;
            using (SQLiteConnection conn = new SQLiteConnection(dbFile))
            {
                conn.CreateTable<T>();
                importedFiles = conn.Query<T>(sql);

            }  
            return importedFiles;
        }
    }
}
