using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Application.Dto.ProductDTO;
using Application.Dto.ProductVariantDTO;
using Domain.Contracts;

namespace Application.Logic.ProductVariantService
{
    public interface IProductVariantService
    {
        Task<int> Create(ProductVariantDto dto, ClaimsPrincipal user);

        Task<IEnumerable<ProductVariant>> GetAll();

        Task<ProductVariant> GetById(string id);

        Task<IEnumerable<ProductVariant>> GetByProductId(string productId);

        Task<int> Update(string id, ProductVariantDto dto, ClaimsPrincipal user);

        Task<int> SoftDelete(string id);

        Task<int> CountAll();
    }
}
