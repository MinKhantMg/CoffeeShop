using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dto.CartItemDTO;
using Domain.Contracts;

namespace Application.Dto.OrderDTO
{
    public class OrderSummaryDto
    {
        public IEnumerable<CartItem> CartItems { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
