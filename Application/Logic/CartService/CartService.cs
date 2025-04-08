using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Application.Dto.CartDTO;
using AutoMapper;
using Domain.Contracts;
using Infrastructure.GenericRepository;
using Infrastructure.Repository.Interface;
using Infrastructure.UnitOfWork;

namespace Application.Logic.CartService
{
    public class CartService : ICartService
    {
        private readonly IUnit _unit;
        private readonly IGenericRepository<Cart, string> _genericRepository;
        private readonly IMapper _mapper;

        public CartService(IUnit unit, IMapper mapper)
        {
            _unit = unit;
            _genericRepository = _unit.GetRepository<Cart, string>();
            _mapper = mapper;
        }

        public async Task<int> Create(CartDto dto)
        {
            var cart = _mapper.Map<Cart>(dto);
            cart.Id = Guid.NewGuid().ToString().ToUpper();
            cart.CreatedOn = DateTime.UtcNow;
            //cart.Status = true;
            int result = await _genericRepository.Add(cart);
            return result;
        }

        public async Task<Cart> GetById(string id)
        {
            if (id == null)
                throw new Exception("Cart record does not exist.");

            var rtn = await _genericRepository.Get(id);
            return rtn;

        }
        public async Task<int> SoftDelete(string id)
        {
            var cart = await GetById(id);
            int result = await _genericRepository.SoftDelete(cart);
            return result;
        }

        public async Task<int> CountAll()
        {
            int count = await _genericRepository.Count();
            return count;
        }
    }
}
