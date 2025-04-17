using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Application.Dto.CartItemDTO;
using Application.Dto.CategoryDTO;
using AutoMapper;
using Domain.Contracts;
using Infrastructure.GenericRepository;
using Infrastructure.Repository;
using Infrastructure.Repository.Interface;
using Infrastructure.UnitOfWork;

namespace Application.Logic.CartItemService
{
    public class CartItemService : ICartItemService
    {
        private readonly IUnit _unit;
        private readonly IGenericRepository<CartItem, string> _genericRepository;
        private readonly ICartItemRepository _cartItemRepository;
        private readonly IProductVariantRepository _productVariantRepository;
        private readonly IMapper _mapper;

        public CartItemService(IUnit unit, IMapper mapper, ICartItemRepository cartItemRepository, IProductVariantRepository productVariantRepository)
        {
            _unit = unit;
            _genericRepository = _unit.GetRepository<CartItem, string>();
            _cartItemRepository = cartItemRepository;
            _productVariantRepository = productVariantRepository;
            _mapper = mapper;
        }

        public async Task<CartItemSummaryDto> Create(CartItemDto dto)
        {
            var productVariant = await _productVariantRepository.GetByIdAsync(dto.ProductVariantId);
            if (productVariant == null)
                throw new Exception("Invalid ProductVariant selected");

            var existingItem = await _cartItemRepository.GetCartItemByCartIdAndVariantIdAsync(dto.CartId, dto.ProductVariantId);

            if (existingItem != null)
            {
                // Update quantity & subtotal
                existingItem.Quantity += dto.Quantity;
                existingItem.Price = productVariant.Price;
                existingItem.SubTotal = existingItem.Price * existingItem.Quantity;

                await _cartItemRepository.UpdateItemQuantityAsync(
                    existingItem.Id,
                    existingItem.Quantity,
                    existingItem.Price,
                    existingItem.SubTotal
                );
            }
            else
            {
                // New item
                var cartItem = _mapper.Map<CartItem>(dto);
                cartItem.Id = Guid.NewGuid().ToString().ToUpper();
                cartItem.CreatedOn = DateTime.UtcNow;
                cartItem.Price = productVariant.Price;
                cartItem.SubTotal = productVariant.Price * cartItem.Quantity;

                await _genericRepository.Add(cartItem);
            }

            var cartItems = await _cartItemRepository.GetCartItemsByCartIdAsync(dto.CartId);

            var displayItems = new List<CartItemDisplayDto>();
            int totalAmount = 0;

            foreach (var item in cartItems)
            {
                var variant = await _productVariantRepository.GetByIdAsync(item.ProductVariantId);

                var subTotal = item.Quantity * item.Price;
                totalAmount += subTotal;

                displayItems.Add(new CartItemDisplayDto
                {
                    Id = item.Id,
                    CartId = item.CartId,
                    ProductVariantId = item.ProductVariantId,
                    Quantity = item.Quantity,
                    Price = item.Price,
                    SubTotal = subTotal,
                    ProductVariantName = variant?.Name,
                    ImageUrl = variant?.ImageUrl
                });
            }

            return new CartItemSummaryDto
            {
                CartItems = displayItems,
                TotalPrice = totalAmount
            };
        }


        //public async Task<CartItemSummaryDto> Create(CartItemDto dto)
        //{
        //    var productVariant = await _productVariantRepository.GetByIdAsync(dto.ProductVariantId);
        //    if (productVariant == null)
        //        throw new Exception("Invalid ProductVariant selected");

        //    var existingItem = await _cartItemRepository.GetCartItemByCartIdAndVariantIdAsync(dto.CartId, dto.ProductVariantId);

        //    if (existingItem != null)
        //    {
        //        // Update quantity & subtotal
        //        existingItem.Quantity += dto.Quantity;
        //        existingItem.Price = productVariant.Price;
        //        existingItem.SubTotal = existingItem.Price * existingItem.Quantity;

        //        await _cartItemRepository.UpdateItemQuantityAsync(existingItem.Id, existingItem.Quantity, existingItem.Price, existingItem.SubTotal);
        //    }
        //    else
        //    {
        //        // New item
        //        var cartItem = _mapper.Map<CartItem>(dto);
        //        cartItem.Id = Guid.NewGuid().ToString().ToUpper();
        //        cartItem.CreatedOn = DateTime.UtcNow;
        //        cartItem.Price = productVariant.Price;
        //        cartItem.SubTotal = productVariant.Price * cartItem.Quantity;

        //        await _genericRepository.Add(cartItem);
        //    }

        //    var cartItems = await _cartItemRepository.GetCartItemsByCartIdAsync(dto.CartId);
        //    int totalAmount = cartItems.Sum(item => item.Price * item.Quantity);

        //    return new CartItemSummaryDto
        //    {
        //        CartItems = cartItems,
        //        TotalPrice = totalAmount
        //    };
        //}

        public async Task<CartItem> GetById(string id)
        {
            if (id == null)
                throw new Exception("CartItem record does not exist.");

            var rtn = await _cartItemRepository.GetByCartItemIdAsync(id);
            return rtn;

        }

        //public async Task<CartItemSummaryDto> GetCartItemsByCartIdAsync(string id)
        //{
        //    var cartItems = await _cartItemRepository.GetCartItemsByCartIdAsync(id);

        //    int totalAmount = cartItems.Sum(item => item.Price * item.Quantity);

        //    if (id == null)
        //        throw new Exception("CartItem record does not exist.");

        //    return new CartItemSummaryDto
        //    {
        //        CartItems = cartItems,
        //        TotalPrice = totalAmount
        //    };
        //}

        public async Task<CartItemSummaryDto> GetCartItemsByCartIdAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Cart ID must not be null or empty.");

            var cartItems = await _cartItemRepository.GetCartItemsByCartIdAsync(id);

            if (cartItems == null || !cartItems.Any())
                throw new Exception("CartItem record does not exist.");

            var result = new List<CartItemDisplayDto>();
            int totalAmount = 0;

            foreach (var item in cartItems)
            {
                var variant = await _productVariantRepository.GetByIdAsync(item.ProductVariantId);

                var subTotal = item.Quantity * item.Price;
                totalAmount += subTotal;

                result.Add(new CartItemDisplayDto
                {
                    Id = item.Id,
                    CartId = item.CartId,
                    ProductVariantId = item.ProductVariantId,
                    Quantity = item.Quantity,
                    Price = item.Price,
                    SubTotal = subTotal,
                    ProductVariantName = variant?.Name,
                    ImageUrl = variant?.ImageUrl
                });
            }

            return new CartItemSummaryDto
            {
                CartItems = result,
                TotalPrice = totalAmount
            };
        }


        //public async Task<CartItemSummaryDto> AdjustQuantityAsync(string id, int adjustment)
        //{
        //    var cartItem = await GetById(id);
        //    if (cartItem == null)
        //        throw new Exception("CartItem not found");

        //    var newQuantity = cartItem.Quantity + adjustment;

        //    if (newQuantity <= 0)
        //    {
        //        await _genericRepository.SoftDelete(cartItem);
        //    }
        //    else
        //    {
        //        var productVariant = await _productVariantRepository.GetByIdAsync(cartItem.ProductVariantId);
        //        if (productVariant != null)
        //        {
        //            cartItem.Price = productVariant.Price;
        //            cartItem.SubTotal = productVariant.Price * newQuantity;
        //        }

        //        cartItem.Quantity = newQuantity;
        //        await _cartItemRepository.UpdateItemQuantityAsync(cartItem.Id, cartItem.Quantity, cartItem.Price, cartItem.SubTotal);
        //    }

        //    var cartItems = await _cartItemRepository.GetCartItemsByCartIdAsync(cartItem.CartId);
        //    int totalAmount = cartItems.Sum(item => item.Price * item.Quantity);

        //    return new CartItemSummaryDto
        //    {
        //        CartItems = cartItems,
        //        TotalPrice = totalAmount
        //    };
        //}

        public async Task<CartItemSummaryDto> AdjustQuantityAsync(string id, int quantity)
        {
            var cartItem = await GetById(id);
            if (cartItem == null)
                throw new Exception("CartItem not found");

            //var newQuantity = cartItem.Quantity + adjustment;

            if (quantity <= 0)
            {
                await _genericRepository.SoftDelete(cartItem);
            }
            else
            {
                var productVariant = await _productVariantRepository.GetByIdAsync(cartItem.ProductVariantId);
                if (productVariant != null)
                {
                    cartItem.Price = productVariant.Price;
                    cartItem.SubTotal = productVariant.Price * quantity;
                }

                cartItem.Quantity = quantity;
                await _cartItemRepository.UpdateItemQuantityAsync(
                    cartItem.Id,
                    cartItem.Quantity,
                    cartItem.Price,
                    cartItem.SubTotal
                );
            }

            var cartItems = await _cartItemRepository.GetCartItemsByCartIdAsync(cartItem.CartId);

            var displayItems = new List<CartItemDisplayDto>();
            int totalAmount = 0;

            foreach (var item in cartItems)
            {
                var variant = await _productVariantRepository.GetByIdAsync(item.ProductVariantId);

                var subTotal = item.Quantity * item.Price;
                totalAmount += subTotal;

                displayItems.Add(new CartItemDisplayDto
                {
                    Id = item.Id,
                    CartId = item.CartId,
                    ProductVariantId = item.ProductVariantId,
                    Quantity = item.Quantity,
                    Price = item.Price,
                    SubTotal = subTotal,
                    ProductVariantName = variant?.Name,
                    ImageUrl = variant?.ImageUrl
                });
            }

            return new CartItemSummaryDto
            {
                CartItems = displayItems,
                TotalPrice = totalAmount
            };
        }


        public async Task<int> SoftDelete(string id)
        {
            var category = await GetById(id);
            int result = await _genericRepository.SoftDelete(category);
            return result;
        }

        public async Task<int> CountAll()
        {
            int count = await _genericRepository.Count();
            return count;
        }
    }
}
