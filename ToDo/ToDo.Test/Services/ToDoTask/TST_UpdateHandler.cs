using System;
using System.Threading.Tasks;
using Xunit;

namespace ToDo.Test.Services.ToDoTask
{
    public class TST_UpdateHandler : IClassFixture<Fixture>
    {
        private readonly Random rnd;

        public TST_UpdateHandler(Fixture fixture)
        {
            rnd = new Random();
        }

        [Fact(DisplayName = "Services/UpdateHandler - succesfully update task")]
        public async Task Handle_Ok()
        {
            var mockedDb = new Mock.MockingDatabase();
            var mockedFactory = new Mock.SqlFactory(mockedDb);

            var handler = new ToDo.Services.ToDoTask.UpdateHandler(mockedFactory);

            var expectedTask = new Models.Db.ToDoTask();
            expectedTask.ToDoTaskId = 1;
            expectedTask.TaskStatusId = Models.Db.TaskStatus.NotStarted.TaskStatusId;
            expectedTask.Priority = rnd.Next(0, 50);
            expectedTask.Name = "Testing task";

            mockedDb.Tasks.Add(expectedTask.ToDoTaskId, expectedTask);

            var updatedTask = new Models.Db.ToDoTask();
            updatedTask.ToDoTaskId = 1;
            updatedTask.TaskStatusId = Models.Db.TaskStatus.Completed.TaskStatusId;
            updatedTask.Priority = rnd.Next(0,50);
            updatedTask.Name = $"Testing task num. {rnd.Next(1,10)}";

            var result = await handler.Handle(new ToDo.Services.ToDoTask.UpdateCommand { TaskToUpdate = updatedTask }, new System.Threading.CancellationToken());

            Assert.True(result.IsSucces);

            Assert.Equal(expectedTask.ToDoTaskId, updatedTask.ToDoTaskId);
            Assert.Equal(expectedTask.TaskStatusId, updatedTask.TaskStatusId);
            Assert.Equal(expectedTask.Priority, updatedTask.Priority);
            Assert.Equal(expectedTask.Name, updatedTask.Name);
        }

        [Fact(DisplayName = "Services/UpdateHandler - Fail update task - task with new name already exists")]
        public async Task Handle_AlreadyUsed()
        {
            var mockedDb = new Mock.MockingDatabase();
            var mockedFactory = new Mock.SqlFactory(mockedDb);

            var handler = new ToDo.Services.ToDoTask.UpdateHandler(mockedFactory);

            var blockingTask = new Models.Db.ToDoTask();
            blockingTask.ToDoTaskId = 1;
            blockingTask.TaskStatusId = Models.Db.TaskStatus.NotStarted.TaskStatusId;
            blockingTask.Priority = rnd.Next(0, 50);
            blockingTask.Name = $"Testing task num. {rnd.Next(1, 10)}";

            var expectedTask = new Models.Db.ToDoTask();
            expectedTask.ToDoTaskId = 2;
            expectedTask.TaskStatusId = Models.Db.TaskStatus.NotStarted.TaskStatusId;
            expectedTask.Priority = rnd.Next(0, 50);
            expectedTask.Name = $"Testing task num. {rnd.Next(1, 10)}";

            mockedDb.Tasks.Add(blockingTask.ToDoTaskId, blockingTask);
            mockedDb.Tasks.Add(expectedTask.ToDoTaskId, expectedTask);

            var updatedTask = new Models.Db.ToDoTask();
            updatedTask.ToDoTaskId = 2;
            updatedTask.TaskStatusId = Models.Db.TaskStatus.Completed.TaskStatusId;
            updatedTask.Priority = rnd.Next(0, 50);
            updatedTask.Name = blockingTask.Name; /// <---- Only one task of same name can be in database

            var result = await handler.Handle(new ToDo.Services.ToDoTask.UpdateCommand { TaskToUpdate = updatedTask }, new System.Threading.CancellationToken());

            Assert.False(result.IsSucces);
        }
    }
}
