using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chat.Data.EfCore;

namespace Chat.Data.Repositories
{
    public interface IUserRepositories
    {
        Task CreateUserAsync(User user);
        Task<User> FindUserByUsernameAsync(string userName);
        Task<IEnumerable<User>> GetAllUsersAsync();
    }
}
