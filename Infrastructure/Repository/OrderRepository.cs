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

namespace Infrastructure.Repository
{
    public class OrderRepository : GenericRepository<Order, string>, IOrderRepository
    {
        public OrderRepository(ApplicationDbContext Context) : base(Context) { }

        public async Task<IEnumerable<Order>> GetAllIsPendingAsync()
        {
            var query = "SELECT * FROM Orders WHERE OrderStatus = 'Pending' ORDER BY CreatedOn DESC";

            return await _connection.QueryAsync<Order>(query);
        }

        public async Task<IEnumerable<Order>> GetAllIsConfirmAsync()
        {
            var query = "SELECT * FROM Orders WHERE OrderStatus = 'Confirmed' ORDER BY CreatedOn DESC";

            return await _connection.QueryAsync<Order>(query);
        }
    }
}
