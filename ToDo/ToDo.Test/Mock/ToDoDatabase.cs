using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ToDo.DbAccessLibrary.Repositories;
using ToDo.Models.Db;

namespace ToDo.Test.Mock
{
    internal class ToDoDatabase : ToDo.DbAccessLibrary.IToDoDatabase
    {
        public ITaskStatusRepository TaskStatus { get; }

        public IToDoTaskRepository ToDoTask { get; }

        public ToDoDatabase(MockingDatabase db)
        {
            TaskStatus = new TaskStatusRepository(db);
            ToDoTask = new ToDoTaskRepository(db);
        }

        public void Commit() { }
        public void Dispose() { }
        public void RollBack() { }
        public void StartTransaction(IsolationLevel isolationLevel) { }
    }

    internal class TaskStatusRepository : ITaskStatusRepository
    {
        private readonly MockingDatabase db;

        public TaskStatusRepository(MockingDatabase db)
        {
            this.db = db;
        }

        public Task<List<Models.Db.TaskStatus>> GetAll()
        {
            return Task.FromResult(db.States.Values.ToList());
        }
    }

    internal class ToDoTaskRepository : IToDoTaskRepository
    {
        private readonly MockingDatabase db;

        public ToDoTaskRepository(MockingDatabase db)
        {
            this.db = db;
        }

        public Task<bool> CheckExists(string name, int toDoTaskId = 0)
        {
            var exits = db.Tasks.Values.Where(i => i.Name == name && i.ToDoTaskId != toDoTaskId).Any();

            return Task.FromResult(exits);
        }

        public Task<ToDoTask> Create(string name, int priority, Models.Db.TaskStatus status)
        {
            var task = new ToDoTask()
            {
                Name = name,
                Priority = priority,
                TaskStatusId = status.TaskStatusId,
                ToDoTaskId = db.Tasks.Values.Count + 1
            };

            db.Tasks.Add(task.ToDoTaskId, task);

            return Task.FromResult(task);
        }

        public Task Delete(int toDoTaskId)
        {
            db.Tasks.Remove(toDoTaskId);

            return Task.CompletedTask;  
        }

        public Task<ToDoTask> Get(int toDoTaskId)
        {
            db.Tasks.TryGetValue(toDoTaskId, out ToDoTask task);

            return Task.FromResult(task);
        }

        public Task<List<ToDoTask>> GetAll(bool includeFinished = false)
        {
            var tasks = db.Tasks.Values.Where(i => i.TaskStatusId != 3 || (includeFinished == true) ).ToList();

            return Task.FromResult(tasks);
        }

        public Task Update(ToDoTask taskToUpdate)
        {
            db.Tasks.TryGetValue(taskToUpdate.ToDoTaskId, out ToDoTask task);

            if(task is not null)
            {
                task.Name = taskToUpdate.Name;
                task.Priority = taskToUpdate.Priority;
                task.TaskStatusId = taskToUpdate.TaskStatusId;
            }

            return Task.CompletedTask;
        }
    }

    internal class MockingDatabase
    {
        public Dictionary<int, Models.Db.ToDoTask> Tasks { get; } = new Dictionary<int, Models.Db.ToDoTask>();
        public Dictionary<int, Models.Db.TaskStatus> States { get; } = new Dictionary<int, Models.Db.TaskStatus>();
    }
}
