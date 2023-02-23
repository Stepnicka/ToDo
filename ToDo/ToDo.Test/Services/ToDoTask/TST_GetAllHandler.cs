using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ToDo.Test.Services.ToDoTask
{
    public class TST_GetAllHandler : IClassFixture<Fixture>
    {
        private readonly Random rnd;

        public TST_GetAllHandler(Fixture fixture)
        {
            rnd = new Random();
        }

        [Fact(DisplayName = "Services/GetAllHandler - succesfully fetch tasks, olny completed")]
        public async Task Handle_Ok_OnlyNotCompleted()
        {
            var mockedDb = new Mock.MockingDatabase();
            var mockedFactory = new Mock.SqlFactory(mockedDb);

            var handler = new ToDo.Services.ToDoTask.GetAllHandler(mockedFactory);

            var firstTask = new Models.Db.ToDoTask();
            firstTask.ToDoTaskId = 1;
            firstTask.TaskStatusId = Models.Db.TaskStatus.NotStarted.TaskStatusId;
            firstTask.Priority = rnd.Next(0, 50);
            firstTask.Name = $"Testing task num. {rnd.Next(1, 10)}";

            var secondTask = new Models.Db.ToDoTask();
            secondTask.ToDoTaskId = 2;
            secondTask.TaskStatusId = Models.Db.TaskStatus.Completed.TaskStatusId; /* <- THIS TASK SHOULD NOT RETURN */
            secondTask.Priority = rnd.Next(0, 50);
            secondTask.Name = $"Testing task num. {rnd.Next(1, 10)}";

            mockedDb.Tasks.Add(firstTask.ToDoTaskId, firstTask);
            mockedDb.Tasks.Add(secondTask.ToDoTaskId, secondTask);

            var result = await handler.Handle(new ToDo.Services.ToDoTask.GetAllQuery { IncludeCompletedTasks = false }, new System.Threading.CancellationToken());

            Assert.NotNull(result.Tasks);
            Assert.NotEmpty(result.Tasks);
            Assert.Single(result.Tasks);

            var actualItem = result.Tasks.Single();

            Assert.Equal(firstTask.ToDoTaskId, actualItem.ToDoTaskId);
            Assert.Equal(firstTask.TaskStatusId, actualItem.TaskStatusId);
            Assert.Equal(firstTask.Priority, actualItem.Priority);
            Assert.Equal(firstTask.Name, actualItem.Name);
        }

        [Fact(DisplayName = "Services/GetAllHandler - succesfully fetch tasks, ALL")]
        public async Task Handle_Ok_All()
        {
            var mockedDb = new Mock.MockingDatabase();
            var mockedFactory = new Mock.SqlFactory(mockedDb);

            var handler = new ToDo.Services.ToDoTask.GetAllHandler(mockedFactory);

            var firstTask = new Models.Db.ToDoTask();
            firstTask.ToDoTaskId = 1;
            firstTask.TaskStatusId = Models.Db.TaskStatus.NotStarted.TaskStatusId;
            firstTask.Priority = rnd.Next(0, 50);
            firstTask.Name = $"Testing task num. {rnd.Next(1, 10)}";

            var secondTask = new Models.Db.ToDoTask();
            secondTask.ToDoTaskId = 2;
            secondTask.TaskStatusId = Models.Db.TaskStatus.Completed.TaskStatusId;
            secondTask.Priority = rnd.Next(0, 50);
            secondTask.Name = $"Testing task num. {rnd.Next(1, 10)}";

            mockedDb.Tasks.Add(firstTask.ToDoTaskId, firstTask);
            mockedDb.Tasks.Add(secondTask.ToDoTaskId, secondTask);

            var result = await handler.Handle(new ToDo.Services.ToDoTask.GetAllQuery { IncludeCompletedTasks = true }, new System.Threading.CancellationToken());

            Assert.NotNull(result.Tasks);
            Assert.NotEmpty(result.Tasks);

            Action<Models.Db.ToDoTask> filter = item => 
            {
                switch (item.ToDoTaskId)
                {
                    case 1:
                        Assert.Equal(firstTask.ToDoTaskId, item.ToDoTaskId);
                        Assert.Equal(firstTask.TaskStatusId, item.TaskStatusId);
                        Assert.Equal(firstTask.Priority, item.Priority);
                        Assert.Equal(firstTask.Name, item.Name);
                        break;
                    case 2:
                        Assert.Equal(secondTask.ToDoTaskId, item.ToDoTaskId);
                        Assert.Equal(secondTask.TaskStatusId, item.TaskStatusId);
                        Assert.Equal(secondTask.Priority, item.Priority);
                        Assert.Equal(secondTask.Name, item.Name);
                        break;
                    default: throw new Exception();
                }
            };

            Assert.Collection(result.Tasks, filter, filter);
        }
    }
}
