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
    public class SubCategoryRepository : GenericRepository<SubCategory, string>, ISubCategoryRepository
    {
        public SubCategoryRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<SubCategory>> GetAllIsDeletetedAsync()
        {
            var query = "SELECT * FROM SubCategories WHERE IsDeleted = 0";
            
            return await _connection.QueryAsync<SubCategory>(query);
        }

        public async Task<SubCategory> GetByIdAsync(string subCategoryId)
        {
            var query = "SELECT * FROM SubCategories WHERE IsDeleted = 0  AND Id = @SubCategoryId";

            return await _connection.QueryFirstOrDefaultAsync<SubCategory>(query, new { SubCategoryId = subCategoryId });
        }

        public async Task<IEnumerable<SubCategory>> GetByCategoryIdAsync(string categoryId)
        {
            var query = "SELECT * FROM SubCategories WHERE IsDeleted = 0  AND CategoryId = @CategoryId";

            return await _connection.QueryAsync<SubCategory>(query, new { CategoryId = categoryId });
        }

    }
}
