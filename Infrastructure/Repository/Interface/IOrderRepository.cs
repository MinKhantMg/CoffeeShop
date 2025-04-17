using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts;
using Infrastructure.GenericRepository;

namespace Infrastructure.Repository.Interface
{
    public interface IOrderRepository : IGenericRepository<Order, string>
    {
        Task<IEnumerable<Order>> GetAllIsPendingAsync();

        Task<IEnumerable<Order>> GetAllIsConfirmAsync();
    }
}
