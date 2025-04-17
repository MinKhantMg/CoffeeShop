using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dto.CartItemDTO;
using Domain.Contracts;
using Domain.Enums;

namespace Application.Dto.OrderDTO
{
    public class OrderReceipt
    {
        public string Id { get; set; }
        public int TotalAmount { get; set; }
        public string OrderStatus { get; set; }

        public DateTime? CreatedOn {  get; set; }
        public string OrderType { get; set; } = Enum.GetName(typeof(OrderType), Domain.Enums.OrderType.DineIn);

        public string PaymentType { get; set; } = Enum.GetName(typeof(PaymentType), Domain.Enums.PaymentType.Cash);
        public IEnumerable<CartItemDisplayDto> CartItems { get; set; }
    }
}
