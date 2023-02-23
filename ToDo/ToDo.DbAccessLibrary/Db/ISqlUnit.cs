using System;
using System.Data;

namespace ToDo.DbAccessLibrary.Db
{
    public interface ISqlUnit : IDisposable
    {
        /// <summary>
        ///     Commit active transaction and close it
        /// </summary>
        void Commit();

        /// <summary>
        ///     Rollback active transaction and close it
        /// </summary>
        void RollBack();

        /// <summary>
        ///     Create new transaction on connection
        /// </summary>
        void StartTransaction(IsolationLevel isolationLevel);
    }
}
