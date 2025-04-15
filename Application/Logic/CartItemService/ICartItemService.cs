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
        Task<CartItemSummaryDto> Create(CartItemDto dto);

        Task<CartItem> GetById(string id);

        Task<CartItemSummaryDto> GetCartItemsByCartIdAsync(string id);

        Task<CartItemSummaryDto> AdjustQuantityAsync(string id, int adjustment);

        Task<int> SoftDelete(string id);    

        Task<int> CountAll();
    }
}
