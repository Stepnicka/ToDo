using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Mime;
using System.Threading.Tasks;
using MediatR;

namespace ToDo.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskStatusController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly ILogger logger;

        public TaskStatusController(IMediator mediator, ILoggerFactory loggerFactory)
        {
            this.mediator = mediator;
            this.logger = loggerFactory.CreateLogger<TaskStatusController>();
        }

        /// <summary>
        ///     Get all task states
        /// </summary>
        [HttpGet(nameof(GetAll))]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(Models.Api.Response.TaskStatus.GetAllResponse))]
        [ProducesResponseType(statusCode: StatusCodes.Status500InternalServerError, type: typeof(Models.Api.SimpleError))]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var results = await mediator.Send(new Services.TaskStatus.GetAllQuery());

                return Ok(new Models.Api.Response.TaskStatus.GetAllResponse { States = results.States });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "HttpRequest {methodName} failed.", nameof(GetAll));

                return StatusCode((int)StatusCodes.Status500InternalServerError, Models.Api.SimpleError.InternalError(ex));
            }
        }
    }
}
