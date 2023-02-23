using FluentValidation;

namespace ToDo.Models.Api.Request.ToDoTask
{
    /// <summary>
    ///     Api Request
    /// </summary>
    public class CreateRequest
    {
        /// <summary>
        ///     Task priority
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        ///     Task name (names are unique per task)
        /// </summary>
        public string Name { get; set; }

        public class Validator : AbstractValidator<CreateRequest>
        {
            public Validator()
            {
                this.RuleFor(i => i)
                    .NotNull().WithMessage("Invalid request");

                this.RuleFor(i => i.Priority)
                    .GreaterThan(0).WithMessage("Priority must be greater then 0");

                this.RuleFor(i => i.Name)
                    .NotEmpty().WithMessage("Name must be specified")
                    .MinimumLength(5).WithMessage("Minimal name lenght is 5 characters")
                    .MaximumLength(250).WithMessage("Maximal name lenght is 250 characters");
            }
        }
    }
}
