using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chat.Data.EfCore;

namespace Chat.Data.Repositories
{
    public class AbstractRepositories
    {
        protected readonly ChitChatDbContext chitChatDbContext;

        public AbstractRepositories(ChitChatDbContext chitChatDbContext)
        {
            this.chitChatDbContext = chitChatDbContext;
        }
    }
}
