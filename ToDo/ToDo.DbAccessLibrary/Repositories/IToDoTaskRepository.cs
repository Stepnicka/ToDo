using ToDo.Models.Db;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ToDo.DbAccessLibrary.Repositories
{
    /// <summary>
    ///     Repository managing operations on [dbo].[ToDoTask]
    /// </summary>
    public interface IToDoTaskRepository
    {
        /// <summary>
        ///     Create new task
        /// </summary>
        Task<ToDoTask> Create(string name, int priority, Models.Db.TaskStatus status);

        /// <summary>
        ///     Return All tasks
        /// </summary>
        Task<List<ToDoTask>> GetAll(bool includeFinished = false);

        /// <summary>
        ///     Check if name is in use
        /// </summary>
        Task<bool> CheckExists(string name, int toDoTaskId = 0);

        /// <summary>
        ///     Update task
        /// </summary>
        Task Update(ToDoTask taskToUpdate);

        /// <summary>
        ///     Get single task by Id
        /// </summary>
        Task<ToDoTask> Get(int toDoTaskId);

        /// <summary>
        ///     Delete single task by Id
        /// </summary>
        Task Delete(int toDoTaskId);
    }
}
