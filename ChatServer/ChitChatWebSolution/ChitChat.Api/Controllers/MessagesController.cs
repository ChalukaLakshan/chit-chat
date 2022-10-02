using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Chat.Data.EfCore;
using Chat.Domain.Interfaces;
using ChitChat.Api.Dto;
using Microsoft.AspNetCore.Authorization;

namespace ChitChat.Api.Controllers
{
    [Route("api/messages")]
    [ApiController]
    [Authorize]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageService _messageService;
        private readonly IMapper _mapper;

        public MessagesController(IMessageService messageService, IMapper mapper)
        {
            _messageService = messageService;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMessages()
        {
            var messageList = await _messageService.GetAllMessages();
            var messageDtoList = _mapper.Map<List<Message>, List<MessageDto>>(messageList.ToList());

            return Ok(messageDtoList);
        }
    }
}
