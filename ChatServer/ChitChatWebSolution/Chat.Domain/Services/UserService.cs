using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Chat.Data.EfCore;
using Chat.Data.Repositories;
using Chat.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;


namespace Chat.Domain.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepositories _repositories;
        private readonly IConfiguration _configuration;

        public UserService(IUserRepositories repositories, IConfiguration configuration)
        {
            this._repositories = repositories;
            this._configuration = configuration;
        }

        public async Task<User> FindUserByUsernameAsync(string username)
        {
            var dbUser = await _repositories.FindUserByUsernameAsync(username.Trim()).ConfigureAwait(false);

            if (string.IsNullOrEmpty(dbUser.Username))
            {
                throw new Exception("User does not exists.");
            }

            return dbUser;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _repositories.GetAllUsersAsync();
        }

        public async Task CreateUserAsync(User user)
        {
            var dbUser = await _repositories.FindUserByUsernameAsync(user.Username.Trim()).ConfigureAwait(false);
            if (!string.IsNullOrEmpty(dbUser.Username))
            {
                throw new Exception("Username already exists. Please try with different one.");
            }
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
            user.CreatedOn = DateTime.Now;

            await _repositories.CreateUserAsync(user).ConfigureAwait(false);
        }

        public async Task<string> LoginAsync(User user)
        {
            var dbUser = await _repositories.FindUserByUsernameAsync(user.Username.Trim()).ConfigureAwait(false);

            if (string.IsNullOrEmpty(dbUser.Username))
            {
                throw new Exception("Username or Password invalid");
            }

            var userHasValidPassword = BCrypt.Net.BCrypt.Verify(user.PasswordHash, dbUser.PasswordHash);

            if (!userHasValidPassword)
            {
                throw new Exception("Username or Password invalid");
            }

            var token = CreateJwtToken(dbUser.Id);
            return token;
        }

        private string CreateJwtToken(Guid userId)
        {
            var claims = new Claim[]
            {
                new("Id", userId.ToString()),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Token:SecurityKey"]));
            var creeds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var duration = Convert.ToInt32(_configuration["Token:AccessTokenExpiresMin"]);

            var jwt = new JwtSecurityToken(
                audience: _configuration["Token:Audience"],
                issuer: _configuration["Token:Issuer"],
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(duration),
                signingCredentials: creeds);

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
