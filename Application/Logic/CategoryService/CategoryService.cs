using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Application.Dto.CategoryDTO;
using AutoMapper;
using AutoMapper.Execution;
using Domain.Contracts;
using Infrastructure.GenericRepository;
using Infrastructure.Repository.Interface;
using Infrastructure.UnitOfWork;

namespace Application.Logic.CategoryService
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnit _unit;
        private readonly IGenericRepository<Category, string> _genericRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(IUnit unit, IMapper mapper, ICategoryRepository categoryRepository)
        {
            _unit = unit;
            _genericRepository = _unit.GetRepository<Category, string>();
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<int> Create(CategoryAddDto dto, ClaimsPrincipal user)
        {
            var adminUserId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "admin123";
            var category = _mapper.Map<Category>(dto);
            category.Id = Guid.NewGuid().ToString().ToUpper();
            category.CreatedBy = adminUserId;
            category.CreatedOn = DateTime.UtcNow;
            int result = await _genericRepository.Add(category);
            return result;
        }

        public async Task<IEnumerable<Category>> GetAll()
        {
            return await _categoryRepository.GetAllIsDeletetedAsync();
        }

        public async Task<Category> GetById(string id)
        {
            if (id == null)
                throw new Exception("Category record does not exist.");

            var rtn = await _categoryRepository.GetByCategoryIdAsync(id);
            return rtn;

        }

        public async Task<int> Update(string id, CategoryAddDto dto, ClaimsPrincipal user)
        {
            var adminUserId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "admin123";

            var category = await GetById(id);
            category.Name = dto.Name;
            category.LastModifiedBy = adminUserId;
            category.LastModifiedOn = DateTime.UtcNow;
            int categoryUpdated = await _genericRepository.Update(category);
            return categoryUpdated;
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
