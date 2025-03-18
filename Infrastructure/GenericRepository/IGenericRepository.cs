using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts;

namespace Infrastructure.GenericRepository
{
    public interface IGenericRepository<T, TId>
    {
        Task<T> Get(TId id);
        Task<T> Get(string fieldName, string fieldValue);


        Task<IEnumerable<T>> GetAll(string orderby);
        Task<IEnumerable<T>> GetList(string orderby);
        Task<IEnumerable<T>> GetList(string[] ids, string orderby);
        Task<IEnumerable<T>> Search(string fieldName, string keyword, string opr, string orderby);
        Task<IEnumerable<T>> Search(string fieldName, string keyword, int pageSize, int pageNumber, string opr, string orderby);
        Task<IEnumerable<T>> GetPagedList(int pageSize, int pageNumber, string orderby);
        Task<IEnumerable<T>> GetPagedList(string query, object parameters, int pageSize, int pageNumber);

        Task<int> Count();
        Task<int> Count(string fieldName, string keyword, string opr);
        Task<int> QueryCount(string query, object parameters);

        Task<bool> IsExist(TId id);
        Task<bool> IsExist(string fieldName, string fieldValue);

        Task<int> Add(T entity);
        Task<int> Insert(T entity);
        Task<int> Update(T entity);
        Task<int> Delete(T entity);
        Task<int> SoftDelete(T entity);

    }
}
