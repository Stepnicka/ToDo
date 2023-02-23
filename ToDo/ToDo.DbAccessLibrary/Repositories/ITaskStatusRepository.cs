using System.Collections.Generic;
using System.Threading.Tasks;

namespace ToDo.DbAccessLibrary.Repositories
{
    /// <summary>
    ///     Repository managing operations on [dbo].[TaskStatus]
    /// </summary>
    public interface ITaskStatusRepository
    {
        /// <summary>
        ///     Return all possible task states
        /// </summary>
        Task<List<Models.Db.TaskStatus>> GetAll();
    }
}
