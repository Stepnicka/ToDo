namespace ToDo.DbAccessLibrary
{
    public interface IToDoDatabase : Db.ISqlUnit
    {
        /// <summary>
        ///     Task states
        /// </summary>
        Repositories.ITaskStatusRepository TaskStatus { get; }

        /// <summary>
        ///     To Do tasks
        /// </summary>
        Repositories.IToDoTaskRepository ToDoTask { get; }
    }
}
