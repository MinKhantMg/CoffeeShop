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
        Task<CartItem> GetByCartItemIdAsync(string cartItemId);

        Task<IEnumerable<CartItem>> GetCartItemsByCartIdAsync(string cartId);

        Task<CartItem?> GetCartItemByCartIdAndVariantIdAsync(string cartId, string productVariantId);

        Task<int> UpdateItemQuantityAsync(string cartItemId, int quantity, int price, int subTotal);

    }
}
