using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts;
using Domain.Database;
using Infrastructure.GenericRepository;
using Infrastructure.Repository.Interface;
using Dapper;

namespace Infrastructure.Repository
{
    public class UserRepository : GenericRepository<User, string>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<User> FindByEmailAsync(string email)
        {

            string query = $"SELECT * FROM {TABLE_NAME} WHERE LOWER(Email) = @Email LIMIT 1";

            return await _connection.QueryFirstOrDefaultAsync<User>(query, new { Email = email.ToLower() });
        }

        public async Task<User> GetUserByRefreshToken(string refreshToken)
        {
            string query = $"SELECT * FROM {TABLE_NAME} WHERE refresh_token = @RefreshToken";

            return await _connection.QueryFirstOrDefaultAsync<User>(query, new { RefreshToken = refreshToken });
        }

        public async Task<bool> UpdateRefreshToken(string userId, string refreshToken, DateTime expiryDate)
        {
            string query = $"UPDATE {TABLE_NAME} SET RefreshToken = @RefreshToken, RefreshTokenExpiry = @ExpiryDate WHERE id = @UserId";

            int rowAffected = await _connection.ExecuteAsync(query, new { RefreshToken = refreshToken, ExpiryDate = expiryDate, UserId = userId });

            return rowAffected > 0;
        }

    }
}
