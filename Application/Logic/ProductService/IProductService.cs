
using System.Security.Claims;
using Application.Dto.ProductDTO;
using Domain.Contracts;

namespace Application.Logic.ProductService
{
    public interface IProductService
    {
        Task<int> Create(ProductAddDto dto, ClaimsPrincipal user);

        Task<IEnumerable<Product>> GetAll();

        Task<Product> GetById(string id);

        Task<IEnumerable<Product>> GetBySubCategoryId(string subCategoryId);

        Task<int> Update(string id, ProductAddDto dto, ClaimsPrincipal user);

        Task<bool> SoftDelete(string id, ClaimsPrincipal user);

        Task<int> CountAll();
    }

   
}
