using Application.Dto.CartItemDTO;
using Application.Dto.OrderDTO;
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
        private readonly IProductVariantRepository _productVariantRepository;
        private readonly IOrderRepository _orderRepository;

        public OrderService(IUnit unit, ICartItemRepository cartItemRepository, IOrderItemRepository orderItemRepository, IOrderRepository orderRepository,
                            IPaymentRepository paymentRepository, IProductVariantRepository productVariantRepository)
        {
            _unit = unit;
            _genericRepository = _unit.GetRepository<Order, string>();
            _cartItemRepository = cartItemRepository;
            _orderItemRepository = orderItemRepository;
            _paymentRepository = paymentRepository;
            _productVariantRepository = productVariantRepository;
            _orderRepository = orderRepository;
        }

        public async Task<List<CartItemDisplayDto>> GetCartItems(string cartId)
        {
            var cartItems = await _cartItemRepository.GetCartItemsByCartIdAsync(cartId);

            var displayItems = new List<CartItemDisplayDto>();
            foreach (var item in cartItems)
            {
                var variant = await _productVariantRepository.GetByIdAsync(item.ProductVariantId);
                displayItems.Add(new CartItemDisplayDto
                {
                    ProductVariantId = item.ProductVariantId,
                    Quantity = item.Quantity,
                    Price = item.Price,
                    SubTotal = item.Quantity * item.Price,
                    ProductVariantName = variant?.Name,
                    ImageUrl = variant?.ImageUrl
                });
            }

            return displayItems;
        }

        public async Task<OrderReceipt> GetOrderSummaryByOrderId(string orderId)
        {
            var order = await GetById(orderId);
            if (order == null) return null;

            var orderItems = await _orderItemRepository.GetItemsByOrderIdAsync(orderId);
            var displayItems = new List<CartItemDisplayDto>();

            foreach (var item in orderItems)
            {
                var variant = await _productVariantRepository.GetByIdAsync(item.ProductVariantId);

                displayItems.Add(new CartItemDisplayDto
                {
                    ProductVariantId = item.ProductVariantId,
                    Quantity = item.Quantity,
                    Price = item.Price,
                    SubTotal = item.Quantity * item.Price,
                    ProductVariantName = variant?.Name,
                    ImageUrl = variant?.ImageUrl
                });
            }

            int totalAmount = displayItems.Sum(x => x.SubTotal);

            return new OrderReceipt
            {
                Id = order.Id,
                OrderStatus = order.OrderStatus,
                CreatedOn = order.CreatedOn,
                OrderType = order.OrderType,
                PaymentType = order.PaymentType,
                CartItems = displayItems,
                TotalAmount = totalAmount
            };
        }

        public async Task<bool> ConfirmOrder(string orderId)
        {
            var order = await _genericRepository.Get(orderId);
            if (order == null) throw new Exception("Order not found.");

            if (order.OrderStatus != "Pending")
                throw new Exception("Only pending orders can be confirmed.");

            order.OrderStatus = "Confirmed";
            order.CreatedOn = order.CreatedOn;

            await _genericRepository.Update(order);

            return true;
        }

        public async Task<IEnumerable<Order>> GetConfirmOrdersAsync()
        {
            var result = await _orderRepository.GetAllIsConfirmAsync();

            return result;
        }

        public async Task<IEnumerable<Order>> GetPendingOrdersAsync()
        {
            var result = await _orderRepository.GetAllIsPendingAsync();

            return result;
        }

        public async Task<Order> GetById(string id)
        {
            if (id == null)
                throw new Exception("Category record does not exist.");

            var rtn = await _genericRepository.Get(id);
            return rtn;
        }

        public async Task<OrderDto> CreateOrder(string cartId, string paymentType, string orderType)
        {
            var cartItems = await _cartItemRepository.GetCartItemsByCartIdAsync(cartId);
            var totalAmount = cartItems.Sum(item => item.Price * item.Quantity);

            var order = new Order
            {
                Id = Guid.NewGuid().ToString().ToUpper(),
                TotalAmount = totalAmount,
                OrderStatus = "Pending",
                CreatedOn = DateTime.UtcNow,
                PaymentType = paymentType,
                OrderType = orderType,
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
                OrderType = orderType,
                PaymentType = paymentType
            };

        }
    }
}
