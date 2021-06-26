using AuroraCore.Web.Responses;
using AuroraCore.Application.Interfaces;
using AuroraCore.Domain.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using AuroraCore.Application.DTOs.Minidoc;
using AuroraCore.Application.DTOs.Channel;
using AuroraCore.Domain.Model;
using Microsoft.AspNetCore.Http;
using AuroraCore.Web.DTOs;
using System.IO;
using System.Net;

namespace AuroraCore.Web.Controllers
{
    //[Authorize]
    [ApiController]
    public class MinidocsController : ApiControllerBase
    {
        private readonly IMinidocService _minidocService;

        public MinidocsController(IMinidocService minidocService)
        {
            _minidocService = minidocService;
        }

        [HttpPost]
        [Route("minidocs")]
        public Response<object> Create([FromForm] MinidocCreation creationParams)
        {
            try
            {
                var videoStream = new MemoryStream();
                creationParams.File.CopyTo(videoStream);

                _minidocService.Create(GetCurrentUser().Id, new MinidocCreationParams{
                    Categories = creationParams.Categories,
                    ChannelId = creationParams.ChannelId,
                    Description = creationParams.Description,
                    Title = creationParams.Title,
                    Topics = creationParams.Topics,
                    Video = videoStream,
                });

                return Ok("Minidoc created successfully");
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("minidocs/{id}")]
        public Response<object> Edit(Guid id, [FromBody] MinidocEdition editionParams)
        {
            try
            {
                _minidocService.Edit(GetCurrentUser().Id, id, new MinidocEditionParams {
                    Categories = editionParams.Categories,
                    Description = editionParams.Description,
                    Title = editionParams.Title,
                    Topics = editionParams.Topics,
                    Video = editionParams.File.OpenReadStream()
                });

                return Ok("Minidoc edited successfully");
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("minidocs/{id}")]
        public Response<object> Delete(Guid id)
        {
            try
            {
                _minidocService.Delete(GetCurrentUser().Id, id);
                return Ok("Minidoc deleted successfully");
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("minidocs/{id}")]
        public Response<MinidocResource> GetById(Guid id)
        {
            try
            {
                MinidocResource minidoc = _minidocService.Get(id);
                return Ok(minidoc);
            }
            catch (ValidationException ex)
            {
                return BadRequest<MinidocResource>(ex.Message);
            }
        }

        [HttpGet]
        [Route("channels/{channelId}/minidocs")]
        public Response<IEnumerable<MinidocCompact>> GetByChannelId(Guid channelId)
        {
            try
            {
                IEnumerable<MinidocCompact> minidocs = _minidocService.GetByChannel(channelId);
                return Ok(minidocs);
            }
            catch (ValidationException ex)
            {
                return BadRequest<IEnumerable<MinidocCompact>>(ex.Message);
            }
        }

        [HttpGet]
        [Route("categories")]
        public Response<IEnumerable<MinidocCategory>> GetAllTopics()
        {
            return Ok(_minidocService.GetAvailableCategories());
        }

        [HttpGet]
        [Route("/video")]
        public IActionResult StreamVideo()
        {
            var rangeHeader = HttpContext.Request.Headers["range"].ToString();

            // if (string.IsNullOrEmpty(rangeHeader))
            // {
            //     return BadRequest<IEnumerable<MinidocCompact>>("Requires Range header");
            // }

            //var range = long.Parse(rangeHeader);
            var videoPath = "C:/Users/User/Downloads/top.mp4";
            //FileInfo videoInfo = new FileInfo(videoPath);  
            //long size = videoInfo.Length;

            //var CHUNK_SIZE = Math.Pow(10, 6); // 1MB
            //var start = range;
            //var end = Math.Min(start + CHUNK_SIZE, size - 1);

            //// Create headers
            //var contentLength = end - start + 1;
            // Response.Headers.Add("Content-Range", $"bytes {start}-{end}/{size}");
            // Response.Headers.Add("Accept-Ranges", "bytes");
            // Response.Headers.Add("Content-Length", contentLength.ToString());
            // Response.Headers.Add("Content-Type", "video/mp4");

            // Response.StatusCode = (int)HttpStatusCode.PartialContent;

            return new FileStreamResult(new FileStream(videoPath, FileMode.Open, FileAccess.Read), "video/mp4");
        }
    }
}