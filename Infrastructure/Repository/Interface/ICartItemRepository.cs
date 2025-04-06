using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts;

namespace Infrastructure.Repository.Interface
{
    public interface ICartItemRepository
    {
        Task<IEnumerable<CartItem>> GetCartItemsByCartIdAsync(string cartId);

        Task<int> UpdateItemQuantityAsync(string cartItemId, int quantity, decimal price);
    }
}
