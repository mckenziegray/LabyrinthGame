using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Labyrinth
{
    /// <summary>
    /// Stores data for an entry in the Enemy table
    /// </summary>
    public class EnemyDataEntry
    {
        public EnemyType EnemyType { get; set; }
        public int MaxHP { get; set; }
        public int CurrentHP { get; set; }
        public int Power { get; set; }
        public int Defense { get; set; }
        public int XP { get; set; }
        public string Description { get; set; }
        public int Difficulty { get; set; }
    }

    /// <summary>
    /// Provides access to the Enemy table in the database
    /// </summary>
    public static class EnemyDao
    {
        private static DataTable m_table;
        private static Dictionary<EnemyType, EnemyDataEntry> m_dataEntries;

        /// <summary>
        /// Retrieves the Enemy table from the database
        /// </summary>
        /// <returns>A <see cref="DataTable"/> containing the data in the table</returns>
        public static DataTable GetTable()
        {
            if (m_table == null)
            {
                m_table = Dao.GetTable(nameof(Enemy), nameof(Enemy.Difficulty));
            }

            return m_table;
        }

        /// <summary>
        /// Returns data from the Enemy table as a collection of <see cref="EnemyDataEntry"/>
        /// </summary>
        /// <returns>A collection of <see cref="EnemyDataEntry"/> containing Enemy table data</returns>
        public static Dictionary<EnemyType, EnemyDataEntry> GetData()
        {
            if (m_dataEntries == null)
            {
                m_dataEntries = Dao.ExtractData<EnemyType, EnemyDataEntry>(GetTable());
            }

            return m_dataEntries;
        }

        /// <summary>
        /// Inserts an enemy's data into the Enemy table if it does not exist.
        /// If the enemy already exists in the table, its entry is updated.
        /// </summary>
        /// <param name="enemy">The enemy to insert</param>
        /// <returns>A <see cref="DataTable"/> containing the data in the updated table</returns>
        public static DataTable InsertOrUpdate(Enemy enemy)
        {
            string query = $@"
                INSERT INTO {Dao.SCHEMA}.{m_table.TableName} 
                ({nameof(enemy.EnemyType)}, {nameof(enemy.Description)}, {nameof(enemy.MaxHP)}, {nameof(enemy.Power)}, {nameof(enemy.XP)}, {nameof(enemy.Difficulty)}) 
                VALUES('{enemy.EnemyType}', '{enemy.Description}', {enemy.MaxHP}, {enemy.Power}, {enemy.XP}, {enemy.Difficulty})
                ON DUPLICATE KEY UPDATE 
                {nameof(enemy.Description)} = '{enemy.Description}'
                {nameof(enemy.MaxHP)} = {enemy.MaxHP}
                {nameof(enemy.Power)} = {enemy.Power}
                {nameof(enemy.XP)} = {enemy.XP}
                {nameof(enemy.Difficulty)} = {enemy.Difficulty}";

            using (SqlDataAdapter da = new SqlDataAdapter(query, Dao.CONN_STRING))
            {
                da.Fill(m_table);
            }

            return m_table;
        }
    }
}
