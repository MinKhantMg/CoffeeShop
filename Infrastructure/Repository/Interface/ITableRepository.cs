using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts;
using Domain.Database;
using Infrastructure.GenericRepository;

namespace Infrastructure.Repository.Interface
{
    public interface ITableRepository : IGenericRepository<Table,string>
    {
        Task<IEnumerable<Table>> GetAllIsDeletetedAsync();

        Task<Table> GetByTableIdAsync(string tableId);
    }
}
