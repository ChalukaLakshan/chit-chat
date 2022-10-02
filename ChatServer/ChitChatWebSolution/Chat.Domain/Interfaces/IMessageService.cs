using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chat.Data.EfCore;

namespace Chat.Domain.Interfaces
{
    public interface IMessageService
    {
        Task CreateMessageAsync(Message message);
        Task<IEnumerable<Message>> GetAllMessages();
    }
}
