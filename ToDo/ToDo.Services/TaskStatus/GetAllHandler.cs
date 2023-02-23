using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ToDo.DbAccessLibrary;

namespace ToDo.Services.TaskStatus
{
    /// <remarks> Handled by <see cref="GetAllHandler"/> </remarks>
    public class GetAllQuery : IRequest<GetAllResponse> { }

    /// <remarks> Returned by <see cref="GetAllHandler"/> </remarks>
    public class GetAllResponse
    {
        /// <summary>
        ///     All possible task states
        /// </summary>
        public List<Models.Db.TaskStatus> States { get; set; }
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
            var response = new GetAllResponse();

            using (var sql = sqlFactory.ToDo)
                response.States = await sql.TaskStatus.GetAll();

            return response;
        }
    }
}
