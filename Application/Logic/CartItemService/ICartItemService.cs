using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Application.Dto.CartItemDTO;
using Application.Dto.SubCategoryDTO;
using Domain.Contracts;

namespace Application.Logic.CartItemService
{
    public interface ICartItemService
    {

        Task<int> Create(CartItemDto dto);

        Task<CartItem> GetById(string id);

        Task<IEnumerable<CartItem>> GetCartItemsByCartIdAsync(string cartId);

        Task<int> Update(string id, int quantity);

        Task<int> SoftDelete(string id);    

        Task<int> CountAll();
    }
}
