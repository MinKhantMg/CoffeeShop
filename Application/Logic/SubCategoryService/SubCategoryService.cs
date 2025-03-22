using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Application.Dto.SubCategoryDTO;
using Application.Dto.UserDTO;
using AutoMapper;
using Dapper;
using Domain.Contracts;
using Infrastructure.GenericRepository;
using Infrastructure.Repository;
using Infrastructure.Repository.Interface;
using Infrastructure.UnitOfWork;

namespace Application.Logic.SubCategoryService
{
    public class SubCategoryService : ISubCategoryService
    {
        private readonly IUnit _unit;
        private readonly IGenericRepository<SubCategory, string> _genericRepository;
        private readonly ISubCategoryRepository _subCategoryRepository;
        private readonly IMapper _mapper;

        public SubCategoryService(IUnit unit, ISubCategoryRepository subCategoryRepository, IMapper mapper)
        {
            _unit = unit;
            _genericRepository = _unit.GetRepository<SubCategory, string>();
            _subCategoryRepository = subCategoryRepository;
            _mapper = mapper;
        }

        public async Task<int> Create(SubCategoryDto dto, ClaimsPrincipal user)
        {
            var adminUserId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "admin123";
            var subCategory = _mapper.Map<SubCategory>(dto);
            subCategory.Id = Guid.NewGuid().ToString().ToUpper();
            subCategory.CreatedBy = adminUserId;
            subCategory.CreatedOn = DateTime.UtcNow;
            int result = await _genericRepository.Add(subCategory);
            return result;
        }

        public async Task<IEnumerable<SubCategory>> GetAll()
        {
            return await _subCategoryRepository.GetAllIsDeletetedAsync();
        }

        public async Task<SubCategory> GetById(string id)
        {
            var subCategory = await _subCategoryRepository.GetByIdAsync(id);
            if (subCategory == null)
                throw new Exception("Category record does not exist.");
            return subCategory;
        }

        public async Task<int> Update(string id, SubCategoryDto dto, ClaimsPrincipal user)
        {
            var adminUserId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "admin123";

            var subCategory = await GetById(id);
            subCategory.Name = dto.Name;
            subCategory.CategoryId = dto.CategoryId;
            subCategory.LastModifiedBy = adminUserId;
            subCategory.LastModifiedOn = DateTime.UtcNow;
            int subCategoryUpdated = await _genericRepository.Update(subCategory);
            return subCategoryUpdated;
        }

        public async Task<bool> SoftDelete(string id, ClaimsPrincipal user)
        {
            var adminUserId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "admin123";

            var subCategory = await GetById(id);
            subCategory.DeletedBy = adminUserId;
            subCategory.DeletedOn = DateTime.UtcNow;
            int result = await _genericRepository.SoftDelete(subCategory);
            return (result > 0);

        }

        public async Task<int> CountAll()
        {
            int count = await _genericRepository.Count();
            return count;
        }

    }
}
