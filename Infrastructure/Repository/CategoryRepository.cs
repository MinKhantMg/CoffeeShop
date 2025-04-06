using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Domain.Contracts;
using Domain.Database;
using Infrastructure.GenericRepository;
using Infrastructure.Repository.Interface;

namespace Infrastructure.Repository
{
    public class CategoryRepository : GenericRepository<Category, string>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<Category>> GetAllIsDeletetedAsync()
        {
            var query = "SELECT * FROM Categories WHERE IsDeleted = 0";

            return await _connection.QueryAsync<Category>(query);
        }

        public async Task<Category> GetByCategoryIdAsync(string categoryId)
        {
            Category rtn = default;

            var query = "SELECT * FROM Categories WHERE IsDeleted = 0 AND Id = @CategoryId";

            try
            {
                rtn = await _connection.QueryFirstOrDefaultAsync<Category>(query, new { CategoryId = categoryId });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return rtn;
        }
    }
}
