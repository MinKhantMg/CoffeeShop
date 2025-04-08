using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts;
using Infrastructure.GenericRepository;

namespace Infrastructure.Repository.Interface
{
    public interface IOrderItemRepository : IGenericRepository<OrderItem, string>
    {
        Task<OrderItem> CreateOrderItem(OrderItem dto);
    }
}
