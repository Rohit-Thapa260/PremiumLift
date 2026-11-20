using System;
using System.Data;
using System.Data.SQLite;
using System.IO;

namespace PremiumLift.DataAccess
{
    /// <summary>
    /// All SQLite access for the operation log (Task 3).
    ///
    /// - Relative path: the database lives in a "Data" folder next to the
    ///   program (AppDomain.BaseDirectory), never a hard-coded C:\ path.
    /// - Disconnected model: rows are added / deleted in a DataTable and
    ///   written with SQLiteDataAdapter.Update(), not ExecuteNonQuery().
    /// - No duplication: every operation goes through UseAdapter(), which owns
    ///   the connection, the adapter commands and the exception handling.
    /// </summary>
    public sealed class ElevatorLogRepository
    {
        public const string TableName = "ElevatorLog";
        public const string RelativeFolder = "Data";
        public const string FileName = "ElevatorLog.db";

        private const string SelectSql =
            "SELECT LogID, LogTime, Lift, Event, Floor FROM ElevatorLog ORDER BY LogID DESC";
        private const string InsertSql =
            "INSERT INTO ElevatorLog (LogTime, Lift, Event, Floor) VALUES (@LogTime, @Lift, @Event, @Floor)";
        private const string DeleteSql =
            "DELETE FROM ElevatorLog WHERE LogID = @LogID";
        private const string CreateTableSql =
            "CREATE TABLE IF NOT EXISTS ElevatorLog (" +
            " LogID   INTEGER PRIMARY KEY AUTOINCREMENT," +
            " LogTime TEXT    NOT NULL," +
            " Lift    TEXT    NOT NULL," +
            " Event   TEXT    NOT NULL," +
            " Floor   INTEGER NULL)";

        private readonly string _connectionString;

        public ElevatorLogRepository()
        {
            // Relative to wherever the program is run from - portable.
            DatabasePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, RelativeFolder, FileName);
            _connectionString = "Data Source=" + DatabasePath + ";Version=3;";
        }

        public string DatabasePath { get; }

        /// <summary>Empty table with the log schema. Used for new rows and for loading.</summary>
        public static DataTable CreateLogTable()
        {
            var table = new DataTable(TableName);
            var id = table.Columns.Add("LogID", typeof(long));
            id.AutoIncrement = true;      // temporary negative IDs for unsaved rows
            id.AutoIncrementSeed = -1;
            id.AutoIncrementStep = -1;
            table.Columns.Add("LogTime", typeof(string));
            table.Columns.Add("Lift", typeof(string));
            table.Columns.Add("Event", typeof(string));
            table.Columns.Add("Floor", typeof(long)).AllowDBNull = true;
            table.PrimaryKey = new[] { id };
            return table;
        }

        /// <summary>Creates the Data folder, the database file and the table if missing.</summary>
        public void EnsureDatabase()
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(DatabasePath));
                using (var connection = new SQLiteConnection(_connectionString))
                using (var command = new SQLiteCommand(CreateTableSql, connection))
                {
                    connection.Open();
                    command.ExecuteNonQuery(); // schema (DDL) only - log data uses Update()
                }
            }
            catch (Exception ex) when (ex is SQLiteException || ex is IOException || ex is UnauthorizedAccessException)
            {
                throw new DataAccessException("Could not open or create the log database at:\n" + DatabasePath, ex);
            }
        }

        /// <summary>Reads every log entry, newest first.</summary>
        public DataTable LoadAll()
        {
            return UseAdapter(adapter =>
            {
                var table = CreateLogTable();
                adapter.Fill(table);
                table.AcceptChanges();
                return table;
            });
        }

        /// <summary>Writes the Added rows of <paramref name="changes"/> to the database.</summary>
        public int Save(DataTable changes)
        {
            if (changes == null) throw new ArgumentNullException(nameof(changes));
            return UseAdapter(adapter => adapter.Update(changes));
        }

        /// <summary>Deletes every log entry (rows marked Deleted, then Update()).</summary>
        public int ClearAll()
        {
            return UseAdapter(adapter =>
            {
                var table = CreateLogTable();
                adapter.Fill(table);
                table.AcceptChanges();
                foreach (DataRow row in table.Rows)
                    row.Delete();
                return adapter.Update(table);
            });
        }

        /// <summary>
        /// The ONE place that builds the connection + adapter and handles errors,
        /// so no database code is repeated anywhere else.
        /// </summary>
        private T UseAdapter<T>(Func<SQLiteDataAdapter, T> work)
        {
            try
            {
                using (var connection = new SQLiteConnection(_connectionString))
                using (var select = new SQLiteCommand(SelectSql, connection))
                using (var insert = new SQLiteCommand(InsertSql, connection))
                using (var delete = new SQLiteCommand(DeleteSql, connection))
                using (var adapter = new SQLiteDataAdapter(select))
                {
                    insert.Parameters.Add(Param("@LogTime", DbType.String, "LogTime"));
                    insert.Parameters.Add(Param("@Lift", DbType.String, "Lift"));
                    insert.Parameters.Add(Param("@Event", DbType.String, "Event"));
                    insert.Parameters.Add(Param("@Floor", DbType.Int64, "Floor"));

                    var key = Param("@LogID", DbType.Int64, "LogID");
                    key.SourceVersion = DataRowVersion.Original;
                    delete.Parameters.Add(key);

                    adapter.InsertCommand = insert;
                    adapter.DeleteCommand = delete;

                    // Fill/Update open and close the connection themselves.
                    return work(adapter);
                }
            }
            catch (SQLiteException ex)
            {
                throw new DataAccessException("Database error: " + ex.Message, ex);
            }
            catch (DBConcurrencyException ex)
            {
                throw new DataAccessException("The log changed while it was being saved. Please refresh.", ex);
            }
        }

        private static SQLiteParameter Param(string name, DbType type, string sourceColumn)
        {
            return new SQLiteParameter(name, type) { SourceColumn = sourceColumn };
        }
    }

    /// <summary>Friendly wrapper for any database failure.</summary>
    public sealed class DataAccessException : Exception
    {
        public DataAccessException(string message, Exception inner) : base(message, inner) { }
    }
}
