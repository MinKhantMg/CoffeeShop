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
    public class CartRepository : GenericRepository<Cart,string>, ICartRepository
    {
        public CartRepository(ApplicationDbContext context): base(context) { }

    }
}
