using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chat.Data.EfCore;
using Microsoft.EntityFrameworkCore;

namespace Chat.Data.Repositories
{
    public class MessageRepository : AbstractRepositories, IMessageRepository
    {
        public MessageRepository(ChitChatDbContext chitChatDbContext) : base(chitChatDbContext)
        {
        }

        public async Task CreateMessageAsync(Message message)
        {
            await chitChatDbContext.Messages.AddAsync(message);
            await chitChatDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Message>> GetAllMessages()
        {
            return await chitChatDbContext.Messages
                .Include(x => x.FromUser)
                .Include(x => x.ToUser).AsNoTracking().ToListAsync();
        }
    }
}
