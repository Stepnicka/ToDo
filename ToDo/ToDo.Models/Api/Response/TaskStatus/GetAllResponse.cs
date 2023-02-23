using System.Collections.Generic;

namespace ToDo.Models.Api.Response.TaskStatus
{
    /// <summary>
    ///     API response
    /// </summary>
    public class GetAllResponse
    {
        /// <summary>
        ///     All possible task states
        /// </summary>
        public List<Db.TaskStatus> States { get; set; }
    }
}
