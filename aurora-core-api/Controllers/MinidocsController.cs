using AuroraCore.Application.DTOs.Minidoc;
using AuroraCore.Application.Interfaces;
using AuroraCore.Domain.Model;
using AuroraCore.Domain.Shared;
using AuroraCore.Web.DTOs;
using AuroraCore.Web.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AuroraCore.Web.Controllers
{
    [Authorize]
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
        public async Task<Response<MinidocResource>> Create([FromForm] MinidocCreation creationParams)
        {
            try
            {
                var videoStream = new MemoryStream();
                creationParams.File.CopyTo(videoStream);

                var minidoc = await _minidocService.Create(GetCurrentUser().Id, new MinidocCreationParams
                {
                    Categories = creationParams.Categories,
                    ChannelId = creationParams.ChannelId,
                    Description = creationParams.Description,
                    Title = creationParams.Title,
                    Topics = creationParams.Topics,
                    Video = videoStream,
                });

                return Created(minidoc);
            }
            catch (ValidationException ex)
            {
                return BadRequest<MinidocResource>(ex.Message);
            }
        }

        [HttpPut]
        [Route("minidocs/{id}")]
        public async Task<Response<MinidocResource>> Edit(Guid id, [FromBody] MinidocEdition editionParams)
        {
            try
            {
                var videoStream = new MemoryStream();
                editionParams.File.CopyTo(videoStream);

                var editedMinidoc = await _minidocService.Edit(GetCurrentUser().Id, id, new MinidocEditionParams
                {
                    Categories = editionParams.Categories,
                    Description = editionParams.Description,
                    Title = editionParams.Title,
                    Topics = editionParams.Topics,
                    Video = videoStream,
                });

                return Ok(editedMinidoc);
            }
            catch (ValidationException ex)
            {
                return BadRequest<MinidocResource>(ex.Message);
            }
        }

        [HttpDelete]
        [Route("minidocs/{id}")]
        public async Task<Response<object>> Delete(Guid id)
        {
            try
            {
                await _minidocService.Delete(GetCurrentUser().Id, id);
                return Ok("Minidoc deleted successfully");
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("minidocs/{id}")]
        public async Task<Response<MinidocResource>> GetById(Guid id)
        {
            try
            {
                MinidocResource minidoc = await _minidocService.Get(id);
                return Ok(minidoc);
            }
            catch (ValidationException ex)
            {
                return BadRequest<MinidocResource>(ex.Message);
            }
        }

        [HttpGet]
        [Route("channels/{channelId}/minidocs")]
        public async Task<Response<IEnumerable<MinidocCompact>>> GetByChannelId(Guid channelId)
        {
            try
            {
                return Ok(await _minidocService.GetByChannel(channelId));
            }
            catch (ValidationException ex)
            {
                return BadRequest<IEnumerable<MinidocCompact>>(ex.Message);
            }
        }

        [HttpGet]
        [Route("categories")]
        public async Task<Response<IEnumerable<MinidocCategory>>> GetAllTopics()
        {
            return Ok(await _minidocService.GetAvailableCategories());
        }
    }
}