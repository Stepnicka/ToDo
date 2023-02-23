using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ToDo.DbAccessLibrary;

namespace ToDo.Services.ToDoTask
{
    /// <remarks> Handled by <see cref="CreateHandler"/> </remarks>
    public class CreateCommand : IRequest<CreateResponse>
    {
        /// <summary>
        ///     Task priority
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        ///     Task name (names are unique per task)
        /// </summary>
        public string Name { get; set; }
    }

    /// <remarks> Returned by <see cref="CreateHandler"/> </remarks>
    public class CreateResponse : IRequest
    {
        public bool IsSucces { get; set; }

        public string Reason { get; set; }

        public Models.Db.ToDoTask Task { get; set; }
    }

    public class CreateHandler : IRequestHandler<CreateCommand, CreateResponse>
    {
        private readonly ISqlFactory sqlFactory;

        public CreateHandler(ISqlFactory sqlFactory)
        {
            this.sqlFactory = sqlFactory;
        }

        public async Task<CreateResponse> Handle(CreateCommand request, CancellationToken cancellationToken)
        {
            var response = new CreateResponse();

            using(var sql = sqlFactory.ToDo)
            {
                if(await sql.ToDoTask.CheckExists(request.Name) == true)
                {
                    response.IsSucces = false;
                    response.Reason = "Task name is already in use";

                    return response;
                }

                response.Task = await sql.ToDoTask.Create(request.Name, request.Priority, Models.Db.TaskStatus.NotStarted);
                response.IsSucces = true;
                response.Reason = null;
            }

            return response;
        }
    }
}
