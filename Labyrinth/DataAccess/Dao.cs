using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace Labyrinth
{
    /// <summary>
    /// Provides access to the database
    /// </summary>
    static class Dao : object
    {
        public const string DB = "Labyrinth";
        public const string SCHEMA = "dbo";
        public const string CONN_STRING = "Server=localhost;Database=master;Trusted_Connection=True;";

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
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
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
