using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Domain.Contracts;
using Domain.Database;
using Infrastructure.GenericRepository;
using Infrastructure.Repository.Interface;
using Infrastructure.UnitOfWork;

namespace Infrastructure.Repository
{
    public class TableRepository : GenericRepository<Table, string>, ITableRepository
    {
        public TableRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<Table>> GetAllIsDeletetedAsync()
        {
            var query = "SELECT * FROM Tables WHERE IsDeleted = 0";

            return await _connection.QueryAsync<Table>(query);
        }

        public async Task<Table> GetByTableIdAsync(string tableId)
        {
            var query = "SELECT * FROM Tables WHERE IsDeleted = 0 AND Id = @TableId";

            return await _connection.QueryFirstOrDefaultAsync<Table>(query, new { TableId = tableId });
        }

    }
}
