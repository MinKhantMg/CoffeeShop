using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts;

namespace Application.Dto.CartItemDTO
{
    public class CartItemSummaryDto
    {
        public List<CartItemDisplayDto> CartItems { get; set; }
        public int TotalPrice { get; set; }
    }
}
