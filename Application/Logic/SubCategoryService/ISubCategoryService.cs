using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Application.Dto.CategoryDTO;
using Application.Dto.SubCategoryDTO;
using Domain.Contracts;

namespace Application.Logic.SubCategoryService
{
    public interface ISubCategoryService
    {
        Task<int> Create(SubCategoryDto dto, ClaimsPrincipal user);

        Task<IEnumerable<SubCategory>> GetAll();

        Task<SubCategory> GetById(string id);

        Task<IEnumerable<SubCategory>> GetByCategoryId(string categoryId);

        Task<int> Update(string id, SubCategoryDto dto, ClaimsPrincipal user);

        Task<int> SoftDelete(string id);

        Task<int> CountAll();
    }
}
