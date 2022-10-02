using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chat.Data.EfCore;

namespace Chat.Data.Repositories
{
    public interface IMessageRepository
    {
        Task CreateMessageAsync(Message message);
        Task<IEnumerable<Message>> GetAllMessages();
    }
}
