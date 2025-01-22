using ContactManagement.Domain.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManagement.Infrastructure.ActionFilters
{

    public class ApiResultFilter : IAsyncResultFilter
    {
        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            if (context.Result is ObjectResult objectResult)
            {
                var statusCode = objectResult.StatusCode ?? StatusCodes.Status200OK;

                if (objectResult.Value?.GetType().Name.StartsWith("Result`1") == true)
                {
                    dynamic result = objectResult.Value;
                    if (result.IsSuccess)
                    {
                        context.Result = new ObjectResult(new ApiResponse
                        {
                            Success = true,
                            Data = result.Value,
                            StatusCode = statusCode
                        })
                        {
                            StatusCode = statusCode
                        };
                    }
                    else
                    {
                        context.Result = new ObjectResult(new ApiResponse
                        {
                            Success = false,
                            Error = result.Error,
                            StatusCode = StatusCodes.Status400BadRequest
                        })
                        {
                            StatusCode = StatusCodes.Status400BadRequest
                        };
                    }
                }
                // Handle regular responses
                else
                {
                    context.Result = new ObjectResult(new ApiResponse
                    {
                        Success = true,
                        Data = objectResult.Value,
                        StatusCode = statusCode
                    })
                    {
                        StatusCode = statusCode
                    };
                }
            }
            else if (context.Result is StatusCodeResult statusCodeResult)
            {
                context.Result = new ObjectResult(new ApiResponse
                {
                    Success = statusCodeResult.StatusCode < 400,
                    StatusCode = statusCodeResult.StatusCode
                })
                {
                    StatusCode = statusCodeResult.StatusCode
                };
            }

            await next();
        }
    }

    public class ApiResponse
    {
        public bool Success { get; set; }
        public object Data { get; set; }
        public string Error { get; set; }
        public int StatusCode { get; set; }
    }

}
