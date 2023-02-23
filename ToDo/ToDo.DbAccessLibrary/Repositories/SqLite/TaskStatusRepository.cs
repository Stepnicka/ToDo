using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ToDo.DbAccessLibrary.Repositories.SqLite
{
    /// <inheritdoc cref="ITaskStatusRepository"/>
    internal class TaskStatusRepository : Db.Repository, ITaskStatusRepository
    {
        public TaskStatusRepository(Func<IDbConnection> GetConnection, Func<IDbTransaction> GetTransaction) : base(GetConnection, GetTransaction) { }

        /// <inheritdoc/>
        public async Task<List<Models.Db.TaskStatus>> GetAll()
        {
            var results = await LoadData<Models.Db.TaskStatus, dynamic>(sql: @" SELECT [TaskStatusId],[DisplayName] FROM [TaskStatus]; ", commandType: CommandType.Text, parameters: new { });

            return results.Distinct().ToList();
        }
    }
}
