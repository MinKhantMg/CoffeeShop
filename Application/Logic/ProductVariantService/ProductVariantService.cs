using System.Security.Claims;
using Application.Dto.ProductVariantDTO;
using AutoMapper;
using Domain.Contracts;
using Infrastructure.GenericRepository;
using Infrastructure.Repository.Interface;
using Infrastructure.UnitOfWork;

namespace Application.Logic.ProductVariantService
{
    public class ProductVariantService : IProductVariantService
    {
        private readonly IUnit _unit;
        private readonly IGenericRepository<ProductVariant, string> _genericRepository;
        private readonly IProductVariantRepository _productVariantRepository;
        private IMapper _mapper;

        public ProductVariantService(IUnit unit, IProductVariantRepository productVariantRepository, IMapper mapper)
        {
            _unit = unit;
            _genericRepository = _unit.GetRepository<ProductVariant, string>();
            _productVariantRepository = productVariantRepository;
            _mapper = mapper;
        }

        public async Task<int> Create(ProductVariantDto dto, ClaimsPrincipal user)
        {
            var adminUserId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "admin123";
            var productVariant = _mapper.Map<ProductVariant>(dto);
            productVariant.Id = Guid.NewGuid().ToString().ToUpper();
            productVariant.CreatedBy = adminUserId;
            productVariant.CreatedOn = DateTime.UtcNow;
            int result = await _genericRepository.Add(productVariant);
            return result;
        }

        public async Task<IEnumerable<ProductVariant>> GetAll()
        {
            return await _productVariantRepository.GetAllIsDeletetedAsync();
        }

        public async Task<ProductVariant> GetById(string id)
        {
            var productVariant = await _productVariantRepository.GetByIdAsync(id);
            if (productVariant == null)
                throw new Exception("ProductVariant record does not exist.");
            return productVariant;
        }

        public async Task<IEnumerable<ProductVariant>> GetByProductId(string productId)
        {
            var productVariant = await _productVariantRepository.GetByProductIdAsync(productId);
            if (productVariant == null)
                throw new Exception("No productvariants found for this product.");
            return productVariant;
        }

        public async Task<int> Update(string id, ProductVariantDto dto, ClaimsPrincipal user)
        {
            var adminUserId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "admin123";

            var productVariant = await GetById(id);
            productVariant.Name = dto.Name;
            productVariant.ProductId = dto.ProductId;
            productVariant.LastModifiedBy = adminUserId;
            productVariant.LastModifiedOn = DateTime.UtcNow;
            int productVariantUpdated = await _genericRepository.Update(productVariant);
            return productVariantUpdated;
        }

        public async Task<int> SoftDelete(string id)
        {
            var productVariant = await GetById(id);
            int result = await _genericRepository.SoftDelete(productVariant);
            return result;

        }

        public async Task<int> CountAll()
        {
            int count = await _genericRepository.Count();
            return count;
        }
    }
}
