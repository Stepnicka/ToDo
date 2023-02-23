using MediatR;
using System.Threading;
using System.Threading.Tasks;
using ToDo.DbAccessLibrary;

namespace ToDo.Services.ToDoTask
{
    /// <remarks> Handled by <see cref="DeleteHandler"/> </remarks>
    public class DeleteCommand : IRequest<DeleteResponse>
    {
        /// <summary>
        ///     Task unique identifier
        /// </summary>
        public int ToDoTaskId { get; set; }
    }

    /// <remarks> Returned by <see cref="DeleteHandler"/> </remarks>
    public class DeleteResponse
    {
        public bool IsSucces { get; set; }

        public string Reason { get; set; }
    }

    public class DeleteHandler : IRequestHandler<DeleteCommand, DeleteResponse>
    {
        private readonly ISqlFactory sqlFactory;

        public DeleteHandler(ISqlFactory sqlFactory)
        {
            this.sqlFactory = sqlFactory;
        }

        public async Task<DeleteResponse> Handle(DeleteCommand request, CancellationToken cancellationToken)
        {
            var response = new DeleteResponse();

            using(var sql = sqlFactory.ToDo)
            {
                var task = await sql.ToDoTask.Get(request.ToDoTaskId);

                if(task is null)
                {
                    response.IsSucces = false;
                    response.Reason = "No task found";
                }
                else if(task.TaskStatusId != Models.Db.TaskStatus.Completed.TaskStatusId)
                {
                    response.IsSucces = false;
                    response.Reason = "Only completed tasks can be removed";
                }
                else
                {
                    await sql.ToDoTask.Delete(request.ToDoTaskId);

                    response.IsSucces = true;
                    response.Reason = null;
                }
            }

            return response;
        }
    }
}
