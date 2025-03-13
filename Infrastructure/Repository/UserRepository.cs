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
           // string query = $"SELECT {ALL_COLUMNS_PROPERTY} FROM {TABLE_NAME} WHERE {KEY_COLUMN} = @{KEY_PROPERTY} LIMIT 1";

            string query = $"SELECT * FROM {TABLE_NAME} WHERE LOWER(Email) = @Email LIMIT 1";

            return await _connection.QueryFirstOrDefaultAsync<User>(query, new { Email = email.ToLower() });
        }

    }
}
