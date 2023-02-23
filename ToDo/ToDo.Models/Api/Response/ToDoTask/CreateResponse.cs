namespace ToDo.Models.Api.Response.ToDoTask
{
    /// <summary>
    ///     Api Response
    /// </summary>
    public class CreateResponse
    {
        /// <summary>
        ///     Newly created Task
        /// </summary>
        public Models.Db.ToDoTask Task { get; set; }
    }
}
