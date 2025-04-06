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

        public async Task<IEnumerable<CartItem>> GetCartItemsByCartIdAsync(string cartId)
        {
            string query = "SELECT * FROM CartItems WHERE CartId = @CartId";
            return await _connection.QueryAsync<CartItem>(query, new { CartId = cartId });
        }

        public async Task<int> UpdateItemQuantityAsync(string cartItemId, int quantity, decimal price)
        {
            string query = "UPDATE CartItems SET Quantity = @Quantity , Price = @Price WHERE Id = @Id";
            return await _connection.ExecuteAsync(query, new { Id = cartItemId, Quantity = quantity, Price = price });
        }
    }
}
