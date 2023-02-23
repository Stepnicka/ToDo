using System;
using System.Threading.Tasks;
using Xunit;

namespace ToDo.Test.Services.ToDoTask
{
    public class TST_DeleteHandler : IClassFixture<Fixture>
    {
        private readonly Random rnd;

        public TST_DeleteHandler()
        {
            rnd = new Random();
        }

        [Fact(DisplayName = "Services/DeleteHandler - succesfully delete task")]
        public async Task Handle_Ok()
        {
            var mockedDb = new Mock.MockingDatabase();
            var mockedFactory = new Mock.SqlFactory(mockedDb);

            var handler = new ToDo.Services.ToDoTask.DeleteHandler(mockedFactory);

            var TaskToDelete = new Models.Db.ToDoTask();
            TaskToDelete.ToDoTaskId = 1;
            TaskToDelete.TaskStatusId = Models.Db.TaskStatus.Completed.TaskStatusId;
            TaskToDelete.Priority = rnd.Next(0, 50);
            TaskToDelete.Name = $"Testing task num. {rnd.Next(1, 10)}";
            mockedDb.Tasks.Add(TaskToDelete.ToDoTaskId, TaskToDelete);

            var result = await handler.Handle(new ToDo.Services.ToDoTask.DeleteCommand { ToDoTaskId = TaskToDelete.ToDoTaskId }, new System.Threading.CancellationToken());

            var deleted = !mockedDb.Tasks.ContainsKey(TaskToDelete.ToDoTaskId);

            Assert.True(result.IsSucces);
            Assert.True(deleted);
        }

        [Fact(DisplayName = "Services/DeleteHandler - fail delete task - only completed task can be deleted")]
        public async Task Handle_NotCompleted()
        {
            var mockedDb = new Mock.MockingDatabase();
            var mockedFactory = new Mock.SqlFactory(mockedDb);

            var handler = new ToDo.Services.ToDoTask.DeleteHandler(mockedFactory);

            var TaskToDelete = new Models.Db.ToDoTask();
            TaskToDelete.ToDoTaskId = 1;
            TaskToDelete.TaskStatusId = Models.Db.TaskStatus.InProgress.TaskStatusId; /// <---- NOT COMPLETED
            TaskToDelete.Priority = rnd.Next(0, 50);
            TaskToDelete.Name = $"Testing task num. {rnd.Next(1, 10)}";
            mockedDb.Tasks.Add(TaskToDelete.ToDoTaskId, TaskToDelete);

            var result = await handler.Handle(new ToDo.Services.ToDoTask.DeleteCommand { ToDoTaskId = TaskToDelete.ToDoTaskId }, new System.Threading.CancellationToken());

            var deleted = !mockedDb.Tasks.ContainsKey(TaskToDelete.ToDoTaskId);

            Assert.False(result.IsSucces);
            Assert.False(deleted);
        }

        [Fact(DisplayName = "Services/DeleteHandler - fail delete task - task does not exist")]
        public async Task Handle_DoesNotExists()
        {
            var mockedDb = new Mock.MockingDatabase();
            var mockedFactory = new Mock.SqlFactory(mockedDb);

            var handler = new ToDo.Services.ToDoTask.DeleteHandler(mockedFactory);

            var randomId = rnd.Next(0, int.MaxValue); /// <---- does not matter, mocked db is empty 

            var result = await handler.Handle(new ToDo.Services.ToDoTask.DeleteCommand { ToDoTaskId = randomId }, new System.Threading.CancellationToken());

            Assert.False(result.IsSucces);
        }
    }
}
