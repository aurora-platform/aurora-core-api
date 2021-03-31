using aurora_core_api.Responses;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace aurora_core_api.Factories
{
    public class ResponseFactory
    {
        public static Response<T> Create<T>(HttpResponse response, HttpStatusCode statusCode, string message, T content)
        {
            response.StatusCode = (int)statusCode;
            return new Response<T>(message, content);
        }

        public static Response<object> Create(HttpResponse response, HttpStatusCode statusCode, string message)
        {
            response.StatusCode = (int)statusCode;
            return new Response<object>(message, null);
        }

        public static Response<T> Ok<T>(HttpResponse response, string message, T content)
        {
            response.StatusCode = (int)HttpStatusCode.OK;
            return new Response<T>(message, content);
        }

        public static Response<object> Ok(HttpResponse response, string message)
        {
            response.StatusCode = (int)HttpStatusCode.OK;
            return new Response<object>(message, null);
        }
    }
}
