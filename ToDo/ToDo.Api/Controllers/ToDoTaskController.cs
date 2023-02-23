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
    public class ToDoTaskController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly ILogger logger;

        public ToDoTaskController(IMediator mediator, ILoggerFactory loggerFactory)
        {
            this.mediator = mediator;
            this.logger = loggerFactory.CreateLogger<ToDoTaskController>();
        }

        /// <summary>
        ///     Get all task states
        /// </summary>
        [HttpGet(nameof(GetAll))]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(Models.Api.Response.ToDoTask.GetAllResponse))]
        [ProducesResponseType(statusCode: StatusCodes.Status500InternalServerError, type: typeof(Models.Api.SimpleError))]
        public async Task<IActionResult> GetAll(bool includeCompletedTasks = true)
        {
            try
            {
                var results = await mediator.Send(new Services.ToDoTask.GetAllQuery() { IncludeCompletedTasks = includeCompletedTasks });

                return Ok(new Models.Api.Response.ToDoTask.GetAllResponse { Tasks = results.Tasks });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "HttpRequest {methodName} failed.", nameof(GetAll));

                return StatusCode((int)StatusCodes.Status500InternalServerError, Models.Api.SimpleError.InternalError(ex));
            }
        }

        /// <summary>
        ///     Create new task
        /// </summary>
        [HttpPost(nameof(Create))]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(Models.Api.Response.ToDoTask.CreateResponse))]
        [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(Models.Api.SimpleError))]
        [ProducesResponseType(statusCode: StatusCodes.Status500InternalServerError, type: typeof(Models.Api.SimpleError))]
        public async Task<IActionResult> Create([FromBody] Models.Api.Request.ToDoTask.CreateRequest apiRequest)
        {
            try
            {
                var result = await mediator.Send(new Services.ToDoTask.CreateCommand() { Name = apiRequest.Name, Priority = apiRequest.Priority });

                if (result.IsSucces == false)
                    return BadRequest(Models.Api.SimpleError.BadRequest(result.Reason));

                return Ok(new Models.Api.Response.ToDoTask.CreateResponse { Task = result.Task });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "HttpRequest {methodName} failed.", nameof(Create));

                return StatusCode((int)StatusCodes.Status500InternalServerError, Models.Api.SimpleError.InternalError(ex));
            }
        }

        /// <summary>
        ///     Update existing task
        /// </summary>
        [HttpPut(nameof(Update))]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(void))]
        [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(Models.Api.SimpleError))]
        [ProducesResponseType(statusCode: StatusCodes.Status500InternalServerError, type: typeof(Models.Api.SimpleError))]
        public async Task<IActionResult> Update([FromBody] Models.Api.Request.ToDoTask.UpdateRequest apiRequest)
        {
            try
            {
                var result = await mediator.Send(new Services.ToDoTask.UpdateCommand() { TaskToUpdate = apiRequest.Task });

                if (result.IsSucces == false)
                    return BadRequest(Models.Api.SimpleError.BadRequest(result.Reason));

                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "HttpRequest {methodName} failed.", nameof(Update));

                return StatusCode((int)StatusCodes.Status500InternalServerError, Models.Api.SimpleError.InternalError(ex));
            }
        }

        /// <summary>
        ///     Delete single request
        /// </summary>
        [HttpDelete(nameof(Delete))]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(void))]
        [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(Models.Api.SimpleError))]
        [ProducesResponseType(statusCode: StatusCodes.Status500InternalServerError, type: typeof(Models.Api.SimpleError))]
        public async Task<IActionResult> Delete([FromBody] Models.Api.Request.ToDoTask.DeleteRequest apiRequest)
        {
            try
            {
                var result = await mediator.Send(new Services.ToDoTask.DeleteCommand() { ToDoTaskId = apiRequest.ToDoTaskId });

                if (result.IsSucces == false)
                    return BadRequest(Models.Api.SimpleError.BadRequest(result.Reason));

                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "HttpRequest {methodName} failed.", nameof(Delete));

                return StatusCode((int)StatusCodes.Status500InternalServerError, Models.Api.SimpleError.InternalError(ex));
            }
        }
    }
}
