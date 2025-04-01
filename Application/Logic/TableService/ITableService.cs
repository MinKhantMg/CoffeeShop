using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Application.Dto.CategoryDTO;
using Application.Dto.TableDTO;
using Domain.Contracts;

namespace Application.Logic.TableService
{
    public interface ITableService
    {
        Task<int> Create(TableDto dto, ClaimsPrincipal user);

        Task<IEnumerable<Table>> GetAll();

        Task<Table> GetById(string id);

        Task<int> Update(string id, TableDto dto, ClaimsPrincipal user);

        Task<int> SoftDelete(string id);

        Task<int> CountAll();
    }
}
