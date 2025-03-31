using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Application.Dto.CategoryDTO;
using Domain.Contracts;

namespace Application.Logic.CategoryService
{
    public interface ICategoryService
    {
        Task<int> Create(CategoryAddDto dto, ClaimsPrincipal user);

        Task<IEnumerable<Category>> GetAll();

        Task<Category> GetById(string id);

        Task<int> Update(string id, CategoryAddDto dto, ClaimsPrincipal user);

        Task<int> SoftDelete(string id);

        Task<int> CountAll();

    }
}
