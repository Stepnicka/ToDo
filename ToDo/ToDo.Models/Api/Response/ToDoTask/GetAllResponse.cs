using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDo.Models.Api.Response.ToDoTask
{
    /// <summary>
    ///     API response
    /// </summary>
    public class GetAllResponse
    {
        /// <summary>
        ///     All tasks
        /// </summary>
        public List<Db.ToDoTask> Tasks { get; set; }
    }
}
