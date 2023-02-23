using System.Data;

namespace ToDo.DbAccessLibrary.SqLite
{
    /// <inheritdoc cref="IToDoDatabase"/>
    public class ToDoDatabase : IToDoDatabase
    {
        /// <summary> Flag if unit and underlaying connection is disposed </summary>
        private bool disposed = false;

        /// <summary> Underlaying connection passed down to repositories </summary>
        private IDbConnection connection;

        /// <summary> Underlaying transaction used by repositories </summary>
        private IDbTransaction transaction;

        #region REPOSITORIES

        /// <inheritdoc/>
        public Repositories.ITaskStatusRepository TaskStatus { get; }

        /// <inheritdoc/>
        public Repositories.IToDoTaskRepository ToDoTask { get; }

        #endregion

        private IDbConnection GetConnection() => connection;
        private IDbTransaction GetTransaction() => transaction;

        public ToDoDatabase()
        {
            this.connection = Initializer.Connect();

            this.TaskStatus = new Repositories.SqLite.TaskStatusRepository(GetConnection, GetTransaction);
            this.ToDoTask = new Repositories.SqLite.ToDoTaskRepository(GetConnection, GetTransaction);
        }

        /// <inheritdoc/>
        public void StartTransaction(IsolationLevel isolationLevel)
        {
            if (transaction != null)
                return;

            if (connection.State != ConnectionState.Open)
                connection.Open();

            transaction = connection.BeginTransaction(isolationLevel);
        }

        /// <inheritdoc/>
        public void Commit()
        {
            if (transaction == null)
                return;

            try
            {
                transaction.Commit();

                if (connection.State != ConnectionState.Closed)
                    connection.Close();
            }
            finally
            {
                transaction.Dispose();
                transaction = null;
            }
        }

        /// <inheritdoc/>
        public void RollBack()
        {
            if (transaction == null)
                return;

            try
            {
                transaction.Rollback();

                if (connection.State != ConnectionState.Closed)
                    connection.Close();
            }
            finally
            {
                transaction.Dispose();
                transaction = null;
            }
        }

        public void Dispose() => Dispose(disposed != true);
        protected virtual void Dispose(bool disposing)
        {
            if (transaction != null)
            {
                transaction.Dispose();
                transaction = null;
            }

            if (connection != null)
            {
                connection.Dispose();
                connection = null;
            }

            disposed = true;
        }
    }
}
