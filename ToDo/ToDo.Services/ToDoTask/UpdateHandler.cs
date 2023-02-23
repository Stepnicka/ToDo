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
    /// <remarks> Handled by <see cref="UpdateHandler"/> </remarks>
    public class UpdateCommand : IRequest<UpdateResponse>
    {
        /// <summary>
        ///     Task to update
        /// </summary>
        public Models.Db.ToDoTask TaskToUpdate { get; set; }
    }

    /// <remarks> Returned by <see cref="UpdateHandler"/> </remarks>
    public class UpdateResponse
    {
        public bool IsSucces { get; set; }

        public string Reason { get; set; }
    }

    public class UpdateHandler : IRequestHandler<UpdateCommand, UpdateResponse>
    {
        private readonly ISqlFactory sqlFactory;

        public UpdateHandler(ISqlFactory sqlFactory)
        {
            this.sqlFactory = sqlFactory;
        }

        public async Task<UpdateResponse> Handle(UpdateCommand request, CancellationToken cancellationToken)
        {
            var response = new UpdateResponse();

            using(var sql = sqlFactory.ToDo)
            {
                if(await sql.ToDoTask.CheckExists(request.TaskToUpdate.Name, request.TaskToUpdate.ToDoTaskId) == true)
                {
                    response.IsSucces = false;
                    response.Reason = "Task name is already in use";
                }
                else
                {
                    await sql.ToDoTask.Update(request.TaskToUpdate);

                    response.IsSucces = true;
                    response.Reason = null;
                }
            }
            
            return response;
        }
    }
}
