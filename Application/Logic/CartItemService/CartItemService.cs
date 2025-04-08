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

        public async Task<int> Create(CartItemDto dto)
        {
            var productVariant = await _productVariantRepository.GetByIdAsync(dto.ProductVariantId);

            if (productVariant == null)
                throw new Exception("Invalid ProductVariant selected");

            var cartItem = _mapper.Map<CartItem>(dto);
            cartItem.Id = Guid.NewGuid().ToString().ToUpper();
            cartItem.CreatedOn = DateTime.UtcNow;
            cartItem.Price = productVariant.Price;

            int result = await _genericRepository.Add(cartItem);
            return result;
        }

        public async Task<CartItem> GetById(string id)
        {
            if (id == null)
                throw new Exception("CartItem record does not exist.");

            var rtn = await _genericRepository.Get(id);
            return rtn;

        }

        public async Task<IEnumerable<CartItem>> GetCartItemsByCartIdAsync(string id)
        {
            if (id == null)
                throw new Exception("CartItem record does not exist.");

            var rtn = await _cartItemRepository.GetCartItemsByCartIdAsync(id);
            return rtn;

        }

        public async Task<int> Update(string id, int quantity)
        {
            var cartItem = await GetById(id);
            if (cartItem == null)
                throw new Exception("CartItem not found");

            if (quantity < 1)
                throw new ArgumentException("Quantity must be at least 1");

            var productVariant = await _productVariantRepository.GetByIdAsync(cartItem.ProductVariantId);
            if (productVariant == null)
                throw new Exception("Invalid ProductVariant selected");

            cartItem.Quantity = quantity;
            cartItem.Price = productVariant.Price;

            return await _cartItemRepository.UpdateItemQuantityAsync(cartItem.Id, cartItem.Quantity, cartItem.Price);
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
