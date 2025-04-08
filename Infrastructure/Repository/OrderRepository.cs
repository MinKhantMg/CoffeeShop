using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts;
using Domain.Database;
using Infrastructure.GenericRepository;
using Infrastructure.Repository.Interface;

namespace Infrastructure.Repository
{
    public class OrderRepository : GenericRepository<Order, string>, IOrderRepository
    {
        public OrderRepository(ApplicationDbContext Context) : base(Context) { }
    }
}
