using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using ToDo.Models.Api;

namespace ToDo.Api.Middleware
{
    internal sealed class ReformatValidationProblemAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.Result is BadRequestObjectResult badRequestObjectResult)
            {
                if (badRequestObjectResult.Value is ValidationProblemDetails)
                {
                    if (context.HttpContext.Items.TryGetValue("ValidationResult", out var value))
                    {
                        var validationResult = value as FluentValidation.Results.ValidationResult;
                        var errorMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToArray();

                        context.Result = new BadRequestObjectResult(new SimpleError() { Code = StatusCodes.Status400BadRequest, Errors = errorMessages });
                    }
                }
            }

            base.OnResultExecuting(context);
        }
    }
}
