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
    public class ProductRepository : GenericRepository<Product, string> , IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<Product>> GetAllIsDeletetedAsync()
        {
            var query = "SELECT * FROM Products WHERE IsDeleted = 0 ORDER BY CreatedOn DESC";

            return await _connection.QueryAsync<Product>(query);
        }

        public async Task<Product> GetByIdAsync(string productId)
        {
            var query = "SELECT * FROM Products WHERE IsDeleted = 0  AND Id = @ProductId";

            return await _connection.QueryFirstOrDefaultAsync<Product>(query, new { ProductId = productId });
        }

        public async Task<IEnumerable<Product>> GetBySubCategoryIdAsync(string subCategoryId)
        {
            var query = "SELECT * FROM Products WHERE IsDeleted = 0  AND SubCategoryId = @SubCategoryId";

            return await _connection.QueryAsync<Product>(query, new { SubCategoryId = subCategoryId });
        }
    }
}
