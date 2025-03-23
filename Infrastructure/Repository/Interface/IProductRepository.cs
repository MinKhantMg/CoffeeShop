using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts;
using Infrastructure.GenericRepository;

namespace Infrastructure.Repository.Interface
{
    public interface IProductRepository : IGenericRepository<Product, string>
    {
        Task<IEnumerable<Product>> GetAllIsDeletetedAsync();

        Task<Product> GetByIdAsync(string productId);
    }
}
