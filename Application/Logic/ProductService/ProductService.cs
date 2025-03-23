using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Application.Dto.ProductDTO;
using Application.Dto.SubCategoryDTO;
using AutoMapper;
using Domain.Contracts;
using Infrastructure.GenericRepository;
using Infrastructure.Repository;
using Infrastructure.Repository.Interface;
using Infrastructure.UnitOfWork;

namespace Application.Logic.ProductService
{
    public class ProductService : IProductService
    {
        private readonly IUnit _unit;
        private readonly IGenericRepository<Product, string> _genericRepository;
        private readonly IProductRepository _productRepository;
        private IMapper _mapper;

        public ProductService(IUnit unit, IProductRepository productRepository, IMapper mapper)
        {
            _unit = unit;
            _genericRepository = _unit.GetRepository<Product, string>();
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<int> Create(ProductAddDto dto, ClaimsPrincipal user)
        {
            var adminUserId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "admin123";
            var product = _mapper.Map<Product>(dto);
            product.Id = Guid.NewGuid().ToString().ToUpper();
            product.CreatedBy = adminUserId;
            product.CreatedOn = DateTime.UtcNow;
            int result = await _genericRepository.Add(product);
            return result;
        }

        public async Task<IEnumerable<Product>> GetAll()
        {
            return await _productRepository.GetAllIsDeletetedAsync();
        }

        public async Task<Product> GetById(string id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
                throw new Exception("Product record does not exist.");
            return product;
        }

        public async Task<int> Update(string id, ProductAddDto dto, ClaimsPrincipal user)
        {
            var adminUserId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "admin123";

            var product = await GetById(id);
            product.Name = dto.Name;
            product.SubCategoryId = dto.SubcategoryId;
            product.LastModifiedBy = adminUserId;
            product.LastModifiedOn = DateTime.UtcNow;
            int productUpdated = await _genericRepository.Update(product);
            return productUpdated;
        }

        public async Task<bool> SoftDelete(string id, ClaimsPrincipal user)
        {
            var adminUserId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "admin123";

            var product = await GetById(id);
            product.DeletedBy = adminUserId;
            product.DeletedOn = DateTime.UtcNow;
            int result = await _genericRepository.SoftDelete(product);
            return (result > 0);

        }

        public async Task<int> CountAll()
        {
            int count = await _genericRepository.Count();
            return count;
        }
    }
}
