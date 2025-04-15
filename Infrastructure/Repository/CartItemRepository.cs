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
    public class CartItemRepository : GenericRepository<CartItem, string>, ICartItemRepository
    {
        public CartItemRepository(ApplicationDbContext Context) : base(Context) { }

        public async Task<CartItem> GetByCartItemIdAsync(string cartItemId)
        {
            CartItem rtn = default;

            var query = "SELECT * FROM CartItems WHERE IsDeleted = 0 AND Id = @CartItemId";

            try
            {
                rtn = await _connection.QueryFirstOrDefaultAsync<CartItem>(query, new { CartItemId = cartItemId });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return rtn;
        }

        public async Task<IEnumerable<CartItem>> GetCartItemsByCartIdAsync(string cartId)
        {
            string query = "SELECT * FROM CartItems WHERE CartId = @CartId AND  IsDeleted = 0";
            return await _connection.QueryAsync<CartItem>(query, new { CartId = cartId });
        }

        public async Task<CartItem?> GetCartItemByCartIdAndVariantIdAsync(string cartId, string productVariantId)
        {
            var query = "SELECT * FROM CartItems WHERE CartId = @CartId AND ProductVariantId = @ProductVariantId AND IsDeleted = 0";
            return await _connection.QueryFirstOrDefaultAsync<CartItem>(query, new { CartId = cartId, ProductVariantId = productVariantId });
        }

        public async Task<int> UpdateItemQuantityAsync(string cartItemId, int quantity, int price, int subTotal)
        {
            string query = "UPDATE CartItems SET Quantity = @Quantity , Price = @Price , SubTotal = @SubTotal WHERE Id = @Id";
            return await _connection.ExecuteAsync(query, new { Id = cartItemId, Quantity = quantity, Price = price, SubTotal = subTotal });
        }
    }
}
