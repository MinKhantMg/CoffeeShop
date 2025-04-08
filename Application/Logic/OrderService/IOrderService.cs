using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dto.OrderDTO;

namespace Application.Logic.OrderService
{
    public interface IOrderService
    {
        Task<OrderSummaryDto> GetOrderSummary(string cartId);
        Task<OrderDto> CreateOrder(string cartId, string paymentType);
    }
}
