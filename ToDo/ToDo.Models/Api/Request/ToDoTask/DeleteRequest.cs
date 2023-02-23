using FluentValidation;

namespace ToDo.Models.Api.Request.ToDoTask
{
    /// <summary>
    ///     Api Request
    /// </summary>
    public class DeleteRequest
    {
        /// <summary>
        ///     Task unique identifier
        /// </summary>
        public int ToDoTaskId { get; set; }

        public class Validator : AbstractValidator<DeleteRequest>
        {
            public Validator()
            {
                this.RuleFor(i => i.ToDoTaskId)
                    .GreaterThan(0).WithMessage("Invalid request");
            }
        }
    }
}
