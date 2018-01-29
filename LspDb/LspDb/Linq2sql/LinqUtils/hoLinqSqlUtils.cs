using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using LinqToDB.DataProvider;
using LinqToDB.DataProvider.SQLite;
using Microsoft.Win32;


namespace LspDb.Linq2sql.LinqUtils
{
    public static class LinqUtil
    {

        /// <summary>
        /// Make a DataTable from a LINQ query
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        // ReSharper disable once MemberCanBePrivate.Global
        public static DataTable ToDataTable<T>(this IEnumerable<T> source)
        {
            PropertyInfo[] properties = typeof(T).GetProperties();

            DataTable output = new DataTable();

            foreach (var prop in properties)
            {
                output.Columns.Add(prop.Name, prop.PropertyType);
            }

            foreach (var item in source)
            {
                DataRow row = output.NewRow();

                foreach (var prop in properties)
                {
                    row[prop.Name] = prop.GetValue(item, null);
                }

                output.Rows.Add(row);
            }

            return output;
        }
       
        /// <summary>
        /// Get data source from connection string
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static string  GetDataSourceFromConnectionString(string connectionString)
        {
            Regex rx = new Regex("DataSource=([^;]*)");
            Match match =  rx.Match(connectionString);
            return match.Success ? match.Groups[1].Value : "";
        }
        
        /// <summary>
        /// Get connection string and data provider for SQLite database from path
        /// </summary>
        /// <param name="sqliteDbPath"></param>
        /// <param name="provider"></param>
        /// <returns></returns>
        /// string dsnName = "DSN=MySqlEa;Trusted_Connection=Yes;";
        //  dsnName = "DSN=MySqlEa;";
        public static string GetConnectionString(string sqliteDbPath, out IDataProvider provider)
        {
            provider = new SQLiteDataProvider();
            return $"Data Source={sqliteDbPath};";
            
        }

        
        /// <summary>
        /// Get connectionString if a DSN is part of the connectionString or "" if no DSN available.
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        // ReSharper disable once UnusedMember.Local
        private static string GetConnectionStringForDsn(string connectionString)
        {
            Regex rgx = new Regex("Data Source=([^;]*);");
            Match match = rgx.Match(connectionString);
            if (match.Success)
            {

                return GetConnectionStringFromDsn(match.Groups[1].Value);

            }
            else
            {
                return "";
            }
        }
        /// <summary>
        /// Get ConnectionString from ODBC DSN (System, User, no file DSN)
        /// - Supports ODBC System and User DSN
        /// - Concatenates all of the registry entries of the odbc dsn definition 
        /// - Ignores the entries for: Driver, Lastuser
        /// Tested with: Access, SqlServer, MySql
        /// </summary>
        /// <param name="dsn"></param>
        /// <returns></returns>
        static string GetConnectionStringFromDsn(string dsn)
        {
            string con = GetConnectionString(Registry.CurrentUser, dsn);
            if (con == "") con = GetConnectionString(Registry.LocalMachine, dsn);
            return con;

        }
        static string GetConnectionString(RegistryKey rootKey, string dsn)
        {
            string registryKey = $@"Software\ODBC\ODBC.INI\{dsn}";

            RegistryKey key =
                Registry.CurrentUser.OpenSubKey(registryKey);
            if (key == null) return "";

            var l = from k in key.GetValueNames()
                where k.ToLower() != "driver" && k.ToLower() != "lastuser"
                select new
                {
                    Value = k + "=" + key.GetValue(k) + ";"
                };
            return String.Join("", l.Select(i => i.Value).ToArray());
        }
    }
}
