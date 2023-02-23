using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ToDo.DbAccessLibrary;

namespace ToDo.Services.ToDoTask
{
    /// <remarks> Handled by <see cref="GetAllHandler"/> </remarks>
    public class GetAllQuery : IRequest<GetAllResponse> 
    {
        /// <summary>
        ///     Should completed tasks be included?
        /// </summary>
        public bool IncludeCompletedTasks { get; set; }
    }

    /// <remarks> Returned by <see cref="GetAllHandler"/> </remarks>
    public class GetAllResponse
    {
        /// <summary>
        ///     All tasks
        /// </summary>
        public List<Models.Db.ToDoTask> Tasks { get; set; }
    }

    public class GetAllHandler : IRequestHandler<GetAllQuery, GetAllResponse>
    {
        private readonly ISqlFactory sqlFactory;

        public GetAllHandler(ISqlFactory sqlFactory)
        {
            this.sqlFactory = sqlFactory;
        }

        public async Task<GetAllResponse> Handle(GetAllQuery request, CancellationToken cancellationToken)
        {
            var respone = new GetAllResponse();

            using (var sql = sqlFactory.ToDo)
                respone.Tasks = await sql.ToDoTask.GetAll(request.IncludeCompletedTasks);

            return respone;
        }
    }
}
