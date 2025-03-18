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
        private readonly IMapper _mapper;

        public CategoryService(IUnit unit, IMapper mapper)
        {
            _unit = unit;
            _genericRepository = _unit.GetRepository<Category, string>();
            _mapper = mapper;
        }

        public async Task<int> Create(CategoryAddDto dto, ClaimsPrincipal user)
        {

            var adminUserId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "admin123";
            var category = _mapper.Map<Category>(dto);
            category.Id = Guid.NewGuid().ToString().ToUpper();
            category.CreatedBy = adminUserId;

            int result = await _genericRepository.Add(category);
            return result;
        }

        public async Task<IEnumerable<Category>> GetAll()
        {
            var category = await _genericRepository.GetAll("");
            return category;
        }

        public async Task<Category> GetById(string id)
        {
            var category = await _genericRepository.Get(id);
            if (category == null)
                throw new Exception("Category record does not exist.");
            return category;
        }

        public async Task<int> Update(string id, CategoryAddDto dto)
        {
            var category = await GetById(id);
            category.Name = dto.Name;
            int memberUpdated = await _genericRepository.Update(category);
            return memberUpdated;
        }

        public async Task<bool> SoftDelete(string id)
        {
            var category = await GetById(id);
            int result = await _genericRepository.SoftDelete(category); // Could also be done with "isDeleted = true;"
            return (result > 0);
        }

        public async Task<int> CountAll()
        {
            int count = await _genericRepository.Count();
            return count;
        }

    }
}
