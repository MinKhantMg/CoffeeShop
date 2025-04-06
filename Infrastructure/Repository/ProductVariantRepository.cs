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
    public class ProductVariantRepository : GenericRepository<ProductVariant, string>, IProductVariantRepository
    {
        public ProductVariantRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<ProductVariant>> GetAllIsDeletetedAsync()
        {
            var query = "SELECT * FROM ProductVariants WHERE IsDeleted = 0";

            return await _connection.QueryAsync<ProductVariant>(query);
        }

        public async Task<ProductVariant> GetByIdAsync(string productVariantId)
        {
            var query = "SELECT * FROM ProductVariants WHERE IsDeleted = 0  AND Id = @ProductVariantId";

            return await _connection.QueryFirstOrDefaultAsync<ProductVariant>(query, new { ProductVariantId = productVariantId });
        }

        public async Task<IEnumerable<ProductVariant>> GetByProductIdAsync(string productId)
        {
            var query = "SELECT * FROM ProductVariants WHERE IsDeleted = 0  AND ProductId = @ProductId";

            return await _connection.QueryAsync<ProductVariant>(query, new { ProductId = productId });
        }
    }
}
