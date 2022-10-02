using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Chat.Data.EfCore;
using Chat.Domain.Interfaces;
using ChitChat.Api.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ChitChat.Api.Hubs
{
    //[Authorize]
    public class ChitChatHub: Hub
    {
        private readonly IMessageService _messageService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public ChitChatHub(IMessageService messageService, IMapper mapper, IUserService userService)
        {
            _messageService = messageService;
            _mapper = mapper;
            _userService = userService;
        }

        public async Task AddMessage(MessageDto messageDto)
        {
            var message = _mapper.Map<Message>(messageDto);
            message.FromUserId = (await _userService.FindUserByUsernameAsync(messageDto.FromUsername).ConfigureAwait(false)).Id;
            message.ToUserId = (await _userService.FindUserByUsernameAsync(messageDto.ToUsername).ConfigureAwait(false)).Id;

            await _messageService.CreateMessageAsync(message);

            await Clients.All.SendAsync("MessageBroadcaster", messageDto);
        }
    }
}
