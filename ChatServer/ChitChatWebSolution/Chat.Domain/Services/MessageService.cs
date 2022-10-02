using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chat.Data.EfCore;
using Chat.Data.Repositories;
using Chat.Domain.Interfaces;

namespace Chat.Domain.Services
{
    public class MessageService: IMessageService
    {
        private readonly IMessageRepository _messageRepository;

        public MessageService(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public async Task CreateMessageAsync(Message message)
        {
            await _messageRepository.CreateMessageAsync(message);
        }

        public async Task<IEnumerable<Message>> GetAllMessages()
        {
            var messages = (await _messageRepository.GetAllMessages()).ToList();

            foreach (var message in messages)
            {
                message.ToUserName = message.ToUser.Username;
                message.FromUserName = message.FromUser.Username;
            }

            return messages;
        }
    }
}
