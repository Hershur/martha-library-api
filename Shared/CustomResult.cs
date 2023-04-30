using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace Shared 
{
    public static class SharedUtils
    {
        public static ObjectResult CustomResult(int statusCode, string responseMessage, object? data = null)
        {
            return new ObjectResult(
                new { responseCode = statusCode.ToString(), responseMessage = responseMessage, data = data }
            ) {
                StatusCode = statusCode
            };
        }
    }
}