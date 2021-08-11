using AuroraCore.Web.Responses;
using AuroraCore.Web.Utils;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Security.Claims;

namespace AuroraCore.Web.Controllers
{
    public class ApiControllerBase : ControllerBase
    {
        protected CurrentUser GetCurrentUser()
        {
            return new CurrentUser
            {
                Id = new Guid(User.FindFirst(ClaimTypes.NameIdentifier)?.Value)
            };
        }

        /**
         *  Success
         */

        protected Response<T> Ok<T>(T content)
        {
            Response.StatusCode = (int)HttpStatusCode.OK;
            return new Response<T>("", content);
        }

        protected Response<object> Ok(string message)
        {
            Response.StatusCode = (int)HttpStatusCode.OK;
            return new Response<object>(message, null);
        }

        protected Response<object> Created(string message)
        {
            Response.StatusCode = (int)HttpStatusCode.Created;
            return new Response<object>(message, null);
        }

        protected Response<T> Created<T>(T content)
        {
            Response.StatusCode = (int)HttpStatusCode.Created;
            return new Response<T>("", content);
        }

        /**
         *  Client errors
         */

        protected Response<T> BadRequest<T>(string message, T content = default)
        {
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return new Response<T>(message, content);
        }

        protected Response<object> BadRequest(string message)
        {
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return new Response<object>(message, null);
        }
    }
}