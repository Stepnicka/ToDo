using Microsoft.Data.Sqlite;

namespace ToDo.DbAccessLibrary.SqLite
{
    internal static class Initializer
    {
        private static bool initialized = false;
        private static object lockingObject = new object();

        public static SqliteConnection Connect()
        {
            lock (lockingObject)
            {
                if (initialized == false)
                    return Initialize();
            }

            return new SqliteConnection(GetConnectionString());
        }

        private static SqliteConnection Initialize()
        {
            var connection = new SqliteConnection(GetConnectionString());

            try
            {
                connection.Open();

                SetTables(connection);

                initialized = true;
            }
            finally
            {
                connection.Close();
            }

            return connection;
        }

        private static string GetConnectionString()
        {
            var builder = new SqliteConnectionStringBuilder();
            builder.DataSource = "memory";

            return builder.ConnectionString;
        }

        private static void SetTables(SqliteConnection connection)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"
                    
                    CREATE TABLE IF NOT EXISTS [TaskStatus]
                    (
	                    [TaskStatusId]				INTEGER					NOT NULL    PRIMARY KEY    AUTOINCREMENT,
	                    [DisplayName]				TEXT		            NOT NULL    UNIQUE
                    );

                    CREATE TABLE IF NOT EXISTS [ToDoTask]
                    (
	                    [ToDoTaskId]				INTEGER					NOT NULL    PRIMARY KEY    AUTOINCREMENT,
	                    [TaskStatusId]				INTEGER					NOT NULL,
	                    [Priority]					INTEGER					NOT NULL,
	                    [Name]						TEXT		            NOT NULL    UNIQUE,
                        FOREIGN KEY([TaskStatusId]) REFERENCES [TaskStatus]([TaskStatusId])
                    );

                    INSERT OR IGNORE INTO [TaskStatus]([DisplayName])
                    VALUES('Not started'),('In progress'),('Completed');
                ";

                command.ExecuteNonQuery();
            }
        }
    }
}
