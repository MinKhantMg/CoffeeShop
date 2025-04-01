using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Application.Dto.CategoryDTO;
using Application.Dto.TableDTO;
using AutoMapper;
using Domain.Contracts;
using Infrastructure.GenericRepository;
using Infrastructure.Repository.Interface;
using Infrastructure.UnitOfWork;

namespace Application.Logic.TableService
{
    public class TableService : ITableService
    {
        private readonly IUnit _unit;
        private readonly IGenericRepository<Table, string> _genericRepository;
        private readonly ITableRepository _tableRepository;
        private readonly IMapper _mapper;

        public TableService(IUnit unit, IMapper mapper, ITableRepository tableRepository)
        {
            _unit = unit;
            _genericRepository = _unit.GetRepository<Table, string>();
            _tableRepository = tableRepository;
            _mapper = mapper;
        }

        public async Task<int> Create(TableDto dto, ClaimsPrincipal user)
        {
            var adminUserId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "admin123";

            var table = _mapper.Map<Table>(dto);
            table.Id = Guid.NewGuid().ToString().ToUpper();
            table.CreatedBy = adminUserId;
            table.CreatedOn = DateTime.Now;

            int result = await _genericRepository.Add(table);
            return result;
        }

        public async Task<IEnumerable<Table>> GetAll()
        {
            return await _tableRepository.GetAllIsDeletetedAsync();
        }

        public async Task<Table> GetById(string id)
        {
            if (id == null)
                throw new Exception("Table record does not exist.");

            return await _tableRepository.GetByTableIdAsync(id);

        }

        public async Task<int> Update(string id, TableDto dto, ClaimsPrincipal user)
        {
            var adminUserId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "admin123";

            var table = await GetById(id);
            table.Name = dto.Name;
            table.Status = dto.Status;
            table.LastModifiedBy = adminUserId;
            table.LastModifiedOn = DateTime.UtcNow;
            int tableUpdated = await _genericRepository.Update(table);
            return tableUpdated;
        }

        public async Task<int> SoftDelete(string id)
        {
           

            var table = await GetById(id);
          
            int result = await _genericRepository.SoftDelete(table);
            return result;
        }

        public async Task<int> CountAll()
        {
            int count = await _genericRepository.Count();
            return count;
        }

    }
}
