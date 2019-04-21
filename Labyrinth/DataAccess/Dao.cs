using System.Data;
using System.Data.SqlClient;

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
    }
}
