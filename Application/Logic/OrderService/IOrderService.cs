using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dto.CartItemDTO;
using Application.Dto.OrderDTO;
using Domain.Contracts;

namespace Application.Logic.OrderService
{
    public interface IOrderService
    {
        Task<OrderReceipt> GetOrderSummaryByOrderId(string orderId);

        Task<Order> GetById(string id);

        Task<List<CartItemDisplayDto>> GetCartItems(string cartId);

        Task<bool> ConfirmOrder(string orderId);

        Task<IEnumerable<Order>> GetPendingOrdersAsync();

        Task<IEnumerable<Order>> GetConfirmOrdersAsync();

        Task<OrderDto> CreateOrder(string cartId, string paymentType, string orderType);

        //Task<OrderSummaryDto> GetPendingOrderByIdAsync(string orderId);
    }
}
