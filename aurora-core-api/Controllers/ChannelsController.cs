using aurora_core_api.Responses;
using AuroraCore.Application.DTOs;
using AuroraCore.Application.Interfaces;
using AuroraCore.Domain.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace aurora_core_api.Controllers
{
    [Authorize]
    [ApiController]
    public class ChannelsController : ApiControllerBase
    {
        private readonly IChannelService _channelService;

        public ChannelsController(IChannelService channelService)
        {
            _channelService = channelService;
        }

        [HttpPost]
        [Route("me/channels")]
        public Response<object> CreateChannel(ChannelCreationParams creation)
        {
            try
            {
                _channelService.Create(GetCurrentUser().Id, creation);

                return Ok("Channel created successfully");
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("me/channels")]
        public Response<object> EditChannel(ChannelEditionParams channel)
        {
            try
            {
                _channelService.Edit(channel);
                return Ok("Channel edited successfully");
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("channels/{id}")]
        public Response<ChannelResource> GetOne(Guid id)
        {
            try
            {
                ChannelResource channel = _channelService.GetOne(id);
                return Ok(channel);
            }
            catch (ValidationException ex)
            {
                return BadRequest<ChannelResource>(ex.Message);
            }
        }

        [HttpGet]
        [Route("me/channels")]
        public Response<IEnumerable<ChannelResource>> GetMyChannels()
        {
            try
            {
                IEnumerable<ChannelResource> channels = _channelService.GetAllOwnedBy(GetCurrentUser().Id);
                return Ok(channels);
            }
            catch (ValidationException ex)
            {
                return BadRequest<IEnumerable<ChannelResource>>(ex.Message);
            }
        }

        [HttpPatch]
        [Route("me/channels/{id}/image")]
        public Response<object> ChangeImage(Guid id, [FromBody] string imageBase64)
        {
            try
            {
                _channelService.ChangeImage(id, imageBase64);
                return Ok("Image changed successfully");
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
