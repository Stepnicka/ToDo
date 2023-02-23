using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ToDo.Models.Api
{
    /// <summary>
    ///     Simple Error object returned by the API
    /// </summary>
    public class SimpleError
    {
        /// <summary>
        ///     Satus code for simple error object
        /// </summary>
        [JsonProperty(PropertyName = "code")]
        public int Code { get; set; }

        /// <summary>
        ///     List of string error messages
        /// </summary>
        [JsonProperty(PropertyName = "errors")]
        public string[] Errors { get; set; }

        public static SimpleError InternalError(Exception ex) => new SimpleError { Code = StatusCodes.Status500InternalServerError, Errors = HandleException(ex, new List<string>() { basicErrorWithId }).ToArray() };

        public static SimpleError Conflict(params string[] errors) => new SimpleError() { Code = StatusCodes.Status409Conflict, Errors = errors };

        public static SimpleError NotFound(params string[] errors) => new SimpleError() { Code = StatusCodes.Status404NotFound, Errors = errors };

        public static SimpleError BadRequest(params string[] errors) => new SimpleError() { Code = StatusCodes.Status400BadRequest, Errors = errors };

        private static string basicErrorWithId => $"Api request failed with internal unexpected error.";

        private static List<string> HandleException(Exception ex, List<string> messages)
        {
            if (ex == null)
                return messages;

            messages.Add(ex.Message);

            return HandleException(ex.InnerException, messages);
        }
    }
}
