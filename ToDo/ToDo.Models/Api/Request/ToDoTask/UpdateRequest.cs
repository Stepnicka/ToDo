using FluentValidation;

namespace ToDo.Models.Api.Request.ToDoTask
{
    /// <summary>
    ///     API request
    /// </summary>
    public class UpdateRequest 
    {
        /// <summary>
        ///     Task to be updated
        /// </summary>
        public Db.ToDoTask Task { get; set; }

        public class Validator : AbstractValidator<UpdateRequest>
        {
            public Validator(IValidator<Db.ToDoTask> taskValidator)
            {
                this.RuleFor(i => i)
                    .NotNull().WithMessage("Invalid request");

                this.RuleFor(i => i.Task)
                    .NotNull().WithMessage("Invalid request");

                this.RuleFor(i => i.Task)
                    .SetValidator(taskValidator);
            }
        }
    }
}
