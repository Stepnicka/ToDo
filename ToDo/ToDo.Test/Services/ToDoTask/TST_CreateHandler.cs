using System;
using System.Threading.Tasks;
using Xunit;

namespace ToDo.Test.Services.ToDoTask
{
    public class TST_CreateHandler : IClassFixture<Fixture>
    {
        private readonly Random rnd;

        public TST_CreateHandler()
        {
            rnd = new Random();
        }

        [Fact(DisplayName = "Services/CreateHandler - succesfully create new task")]
        public async Task Handle_Ok()
        {
            var mockedDb = new Mock.MockingDatabase();
            var mockedFactory = new Mock.SqlFactory(mockedDb);

            var handler = new ToDo.Services.ToDoTask.CreateHandler(mockedFactory);

            var expectedName = "test";
            var expectedPriority = rnd.Next(0, 10);

            var result = await handler.Handle(new ToDo.Services.ToDoTask.CreateCommand { Name = expectedName, Priority = expectedPriority }, new System.Threading.CancellationToken());

            Assert.True(result.IsSucces);

            Assert.Equal(expectedName, result.Task.Name);
            Assert.Equal(expectedPriority, result.Task.Priority);
        }

        [Fact(DisplayName = "Services/CreateHandler - Fail create task - task with name already exists")]
        public async Task Handle_AlreadyUsed()
        {
            var mockedDb = new Mock.MockingDatabase();
            var mockedFactory = new Mock.SqlFactory(mockedDb);

            var handler = new ToDo.Services.ToDoTask.CreateHandler(mockedFactory);

            var blockingTask = new Models.Db.ToDoTask();
            blockingTask.ToDoTaskId = 1;
            blockingTask.TaskStatusId = Models.Db.TaskStatus.NotStarted.TaskStatusId;
            blockingTask.Priority = rnd.Next(0, 50);
            blockingTask.Name = $"Testing task num. {rnd.Next(1, 10)}";
            mockedDb.Tasks.Add(blockingTask.ToDoTaskId, blockingTask);

            var result = await handler.Handle(new ToDo.Services.ToDoTask.CreateCommand { Name = blockingTask.Name, Priority = rnd.Next(0,10) }, new System.Threading.CancellationToken());

            Assert.False(result.IsSucces);
        }
    }
}
