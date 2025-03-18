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
    public class ProductRepository : GenericRepository<Product, string> , IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context) { }
    }
}
