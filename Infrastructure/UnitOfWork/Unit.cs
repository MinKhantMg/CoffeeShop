using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Base;
using Domain.Database;
using Infrastructure.GenericRepository;
using Infrastructure.Repository;

namespace Infrastructure.UnitOfWork
{
    public class Unit : IUnit
    {
        private readonly ApplicationDbContext _context;

        public Unit(ApplicationDbContext context)
        {
            _context = context;
        }

        public GenericRepository<T, TId> GetRepository<T, TId>() where T : class, IEntity<TId>
        {
            return new GenericRepository<T, TId>(_context);
        }

        public UserRepository GetUserRepository()
        {
            return new UserRepository(_context);
        }
    }
}
