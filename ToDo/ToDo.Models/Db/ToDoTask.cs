using FluentValidation;

namespace ToDo.Models.Db
{
    /// <summary>
    ///     Task to do
    /// </summary>
    public class ToDoTask
    {
        /// <summary>
        ///     Task unique identifier
        /// </summary>
        public int ToDoTaskId { get; set; }

        /// <summary>
        ///     Task state identifier
        /// </summary>
        public int TaskStatusId { get; set; }

        /// <summary>
        ///     Task priority
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        ///     Task name (names are unique per task)
        /// </summary>
        public string Name { get; set; }

        public class Validator : AbstractValidator<ToDoTask>
        {
            public Validator()
            {
                this.RuleFor(i => i.Priority)
                    .GreaterThan(0).WithMessage("Priority must be greater then 0");

                this.RuleFor(i => i.Name)
                    .NotEmpty().WithMessage("Name must be specified")
                    .MinimumLength(5).WithMessage("Minimal name lenght is 5 characters")
                    .MaximumLength(250).WithMessage("Maximal name lenght is 250 characters");

                this.RuleFor(i => i.TaskStatusId)
                    .InclusiveBetween(1, 3).WithMessage("Invalid state");

                this.RuleFor(i => i.ToDoTaskId)
                    .GreaterThan(0).WithMessage("Invalid request");
            }
        }
    }
}
