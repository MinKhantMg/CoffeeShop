using System.Net.Http.Json;
using Application.Dto.OrderDTO;
using AutoMapper;
using Domain.Contracts;
using Infrastructure.GenericRepository;
using Infrastructure.Repository;
using Infrastructure.Repository.Interface;
using Infrastructure.UnitOfWork;

namespace Application.Logic.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly IUnit _unit;
        private readonly IGenericRepository<Order, string> _genericRepository;
        private readonly ICartItemRepository _cartItemRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IPaymentRepository _paymentRepository;

        public OrderService(IUnit unit, ICartItemRepository cartItemRepository,
                        IOrderItemRepository orderItemRepository, IPaymentRepository paymentRepository)
        {
            _unit = unit;
            _genericRepository = _unit.GetRepository<Order, string>();
            _cartItemRepository = cartItemRepository;
            _orderItemRepository = orderItemRepository;
            _paymentRepository = paymentRepository;
        }

        public async Task<OrderSummaryDto> GetOrderSummary(string cartId)
        {
            // Fetch cart data from local storage or database
            var cartItems = await _cartItemRepository.GetCartItemsByCartIdAsync(cartId);

            // Calculate total amount for the order
            decimal totalAmount = cartItems.Sum(item => item.Price * item.Quantity);

            return new OrderSummaryDto
            {
                CartItems = cartItems,
                TotalAmount = totalAmount
            };
        }

        public async Task<OrderDto> CreateOrder(string cartId, string paymentType)
        {
            var cartItems = await _cartItemRepository.GetCartItemsByCartIdAsync(cartId);
            var totalAmount = cartItems.Sum(item => item.Price * item.Quantity);

            // Create Order
            var order = new Order
            {
                Id = Guid.NewGuid().ToString(),
                TotalAmount = totalAmount,
                OrderStatus = "Pending",
                CreatedOn = DateTime.UtcNow,
                PaymentType = paymentType,
                IsDeleted = false
            };


            await _genericRepository.Add(order);

            foreach (var item in cartItems)
            {
                var orderItem = new OrderItem
                {
                    Id = Guid.NewGuid().ToString().ToUpper(),
                    Quantity = item.Quantity,
                    Price = item.Price,
                    TotalPrice = item.Price * item.Quantity,
                    OrderId = order.Id,
                    ProductVariantId = item.ProductVariantId
                };

                await _orderItemRepository.CreateOrderItem(orderItem);

            }

            var paymentDto = new Payment
            {
                Id = Guid.NewGuid().ToString().ToUpper(),
                Amount = totalAmount,
                PaymentType = paymentType,
                CreatedOn = DateTime.UtcNow,
                OrderId = order.Id
            };

            await _paymentRepository.CreatePaymentAsync(paymentDto);

            return new OrderDto
            {
                Id = order.Id,
                TotalAmount = totalAmount,
                OrderStatus = "Pending",
                PaymentType = paymentType
            };

        }
    }
}
