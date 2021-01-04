using Microsoft.AspNetCore.Http;
using System;

namespace CarsIsland.API.Utils
{
    public class ApiExceptionOptions
    {
        public Action<HttpContext, Exception, ApiError> AddResponseDetails { get; set; }
    }
}
