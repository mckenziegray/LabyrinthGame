using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;

namespace Labyrinth
{
    /// <summary>
    /// Provides access to the database
    /// </summary>
    public static class Dao : object
    {
        public const string DB = "Labyrinth";
        public const string SCHEMA = "dbo";
        public const string CONN_STRING = "Server=(localdb)\\mssqllocaldb;Database=master;Trusted_Connection=True;";

        /// <summary>
        /// Runs the InitializeDatabase script, which creates the DB and tables and initializes the data.
        /// </summary>
        public static void InitializeDatabase()
        {
            using (SqlConnection connection = new SqlConnection(CONN_STRING))
            {
                Server server = new Server(new ServerConnection(connection));
                server.ConnectionContext.ExecuteNonQuery(File.ReadAllText($"DataAccess/InitializeDatabase.sql"));
            }
        }

        /// <summary>
        /// Retrieves a table from the database
        /// </summary>
        /// <param name="tableName">The name of the table to retrieve</param>
        /// <param name="orderByColName">The name of the column to order by</param>
        /// <returns>A <see cref="DataTable"/> containing the data in the table</returns>
        public static DataTable GetTable(string tableName, string orderByColName = null)
        {
            DataTable table = new DataTable();
            string query = $"SELECT * FROM {DB}.{SCHEMA}.{tableName}";

            if (orderByColName != null)
            {
                query += $" ORDER BY {orderByColName}";
            }

            using (SqlDataAdapter da = new SqlDataAdapter(query, CONN_STRING))
            {
                da.Fill(table);
            }

            return table;
        }

        /// <summary>
        /// Extracts data from a <see cref="DataTable"/> as a <see cref="Dictionary{TKey, TValue}"/>. Poor man's Entity Framework replacement.
        /// </summary>
        /// <typeparam name="KeyType">The data type of the primary key.</typeparam>
        /// <typeparam name="DataEntryType">The data type of the object to map the data to.</typeparam>
        /// <param name="table">The table containing the data to be extracted.</param>
        /// <returns></returns>
        public static Dictionary<KeyType, DataEntryType> ExtractData<KeyType, DataEntryType>(DataTable table)
        {
            Dictionary<KeyType, DataEntryType> data = new Dictionary<KeyType, DataEntryType>();

            foreach (DataRow row in table.Rows)
            {
                DataEntryType dataEntry = (DataEntryType)typeof(DataEntryType).GetConstructor(new Type[0]).Invoke(new object[0]);

                foreach (PropertyInfo prop in typeof(DataEntryType).GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance))
                {
                    if (table.Columns.Contains(prop.Name))
                    {
                        prop.SetValue(dataEntry, prop.PropertyType.IsEnum ? Enum.Parse(prop.PropertyType, row[prop.Name].ToString()) : Convert.ChangeType(row[prop.Name], prop.PropertyType));

                        if (prop.PropertyType == typeof(KeyType))
                        {
                            data[(KeyType)prop.GetValue(dataEntry)] = dataEntry;
                        }
                    }
                }
            }

            return data;
        }
    }
}
