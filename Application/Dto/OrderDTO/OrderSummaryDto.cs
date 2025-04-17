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
        public string OrderType { get; set; }
        public string PaymentType { get; set; }
        public int TotalAmount { get; set; }
        public List<OrderItemDto> Items { get; set; }
    }
}
