using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Base;
using Infrastructure.GenericRepository;
using Infrastructure.Repository;

namespace Infrastructure.UnitOfWork
{
    public interface IUnit
    {
        GenericRepository<T, TId> GetRepository<T, TId>() where T : class, IEntity<TId>;

        UserRepository GetUserRepository();
    }
}
