using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Chat.Data.EfCore;


namespace Chat.Domain.Interfaces
{
    public interface IUserService
    {
        Task CreateUserAsync(User user);
        Task<string> LoginAsync(User user);
        Task<User> FindUserByUsernameAsync(string username);
        Task<IEnumerable<User>> GetAllUsersAsync();
    }
}
