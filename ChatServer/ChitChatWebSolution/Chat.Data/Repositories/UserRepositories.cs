using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Chat.Data.EfCore;
using Microsoft.EntityFrameworkCore;

namespace Chat.Data.Repositories
{
    public class UserRepositories : AbstractRepositories, IUserRepositories
    {
        public UserRepositories(ChitChatDbContext chitChatDbContext) : base(chitChatDbContext)
        {
        }

        public async Task CreateUserAsync(User user)
        {
            await chitChatDbContext.Users.AddAsync(user);
            await chitChatDbContext.SaveChangesAsync();
        }

        public async Task<User> FindUserByUsernameAsync(string userName)
        {
            var user = await chitChatDbContext.Users.FirstOrDefaultAsync(u => u.Username.ToUpper().Equals(userName.ToUpper()));

            return user ?? new User { Username = string.Empty };
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await chitChatDbContext.Users.ToListAsync();
        }
    }
}
