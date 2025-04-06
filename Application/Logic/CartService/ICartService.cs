using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Application.Dto.CartDTO;
using Domain.Contracts;

namespace Application.Logic.CartService
{
    public interface ICartService
    {
        Task<int> Create(CartDto dto);

        Task<Cart> GetById(string id);

        //Task<int> Update(string id, Cart dto);

        Task<int> SoftDelete(string id);

        Task<int> CountAll();
    }
}
