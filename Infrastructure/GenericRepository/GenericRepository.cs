using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Domain.Database;

namespace Infrastructure.GenericRepository;

public class GenericRepository<T, TId> : GenericRepositoryBase<T>, IGenericRepository<T, TId> where T : class
{
    public GenericRepository(ApplicationDbContext context) : base(context)
    {
    }

    #region Get

    public async Task<T> Get(TId id)
    {
        T result;
        try
        {
            string query = $"SELECT {ALL_COLUMNS_PROPERTY} FROM {TABLE_NAME} WHERE {KEY_COLUMN} =@Id";

            result = await _connection.QueryFirstOrDefaultAsync<T>(query, new { Id = id });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching a record from db: ${ex.Message}");
            throw new Exception($"Unable to fetch data. Please contact the administrator. ${ex.Message}");
        }
        finally
        {
            _connection.Close();
        }

        return result;
    }

    public async Task<T> Get(string fieldName, string fieldValue)
    {
        T result;

        try
        {
            var query = $"SELECT {ALL_COLUMNS_PROPERTY} FROM {TABLE_NAME} WHERE {fieldName} = @FieldName";
            result = await _connection.QueryFirstOrDefaultAsync<T>(query, new { FieldName = fieldValue });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching a record from db: ${ex.Message}");
            throw new Exception("Unable to fetch data. Please contact the administrator.");
        }
        finally
        {
            _connection.Close();
        }


        return result;
    }
    #endregion

    #region GetList

    public async Task<IEnumerable<T>> GetAll(string orderby = "")
    {
        IEnumerable<T> result;
        try
        {
            ExtendedQuery extendedQuery = new ExtendedQuery();
            extendedQuery.Query = $"SELECT {ALL_COLUMNS_PROPERTY} FROM {TABLE_NAME}";
            extendedQuery.AddOrderBy(orderby);

            result = await _connection.QueryAsync<T>(extendedQuery.Query, extendedQuery.Parameters);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching records from db: ${ex.Message}");
            throw new Exception("Unable to fetch data. Please contact the administrator.");
        }
        finally
        {
            _connection.Close();
        }
        return result;
    }

    public async Task<IEnumerable<T>> GetList(string orderby = "")
    {
        IEnumerable<T> result;
        try
        {
            ExtendedQuery extendedQuery = new ExtendedQuery();
            extendedQuery.Query = $"SELECT {ALL_COLUMNS_PROPERTY} FROM {TABLE_NAME}";
            extendedQuery.AddOrderBy(orderby);

            result = await _connection.QueryAsync<T>(extendedQuery.Query, extendedQuery.Parameters);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching records from db: ${ex.Message}");
            throw new Exception("Unable to fetch data. Please contact the administrator.");
        }
        finally
        {
            _connection.Close();
        }
        return result;
    }

    public async Task<IEnumerable<T>> GetList(string[] ids, string orderby = "")
    {
        IEnumerable<T> result;
        try
        {
            ExtendedQuery extendedQuery = new ExtendedQuery();
            extendedQuery.Query = $"SELECT {ALL_COLUMNS_PROPERTY} FROM {TABLE_NAME} WHERE Id IN @Ids";
            extendedQuery.AddSoftDelete();
            extendedQuery.AddOrderBy(orderby);
            extendedQuery.AddParameter("Ids", ids);

            result = await _connection.QueryAsync<T>(extendedQuery.Query, extendedQuery.Parameters);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching records from db: ${ex.Message}");
            throw new Exception("Unable to fetch data. Please contact the administrator.");
        }
        finally
        {
            _connection.Close();
        }
        return result;
    }

    //Limit --> PageSize  ,  Offset --> PageNumber

    public async Task<IEnumerable<T>> Search(string fieldName, string keyword, string opr = "=", string orderby = "")
    {
        IEnumerable<T> result;
        try
        {
            ExtendedQuery extendedQuery = new ExtendedQuery();
            extendedQuery.Query = $"SELECT {ALL_COLUMNS_PROPERTY} FROM {TABLE_NAME} WHERE {fieldName} {opr} @Keyword";
            extendedQuery.AddSoftDelete();
            extendedQuery.AddOrderBy(orderby);
            extendedQuery.AddParameter("Keyword", keyword);


            result = await _connection.QueryAsync<T>(extendedQuery.Query, extendedQuery.Parameters);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching records from db: ${ex.Message}");
            throw new Exception("Unable to fetch data. Please contact the administrator.");
        }
        finally
        {
            _connection.Close();
        }
        return result;
    }
    #endregion

    #region GetPagedList

    public async Task<IEnumerable<T>> Search(string fieldName, string keyword, int pageSize, int pageNumber,
                                            string opr = "=", string orderby = "")
    {
        IEnumerable<T> result;
        try
        {

            ExtendedQuery extendedQuery = new ExtendedQuery();
            extendedQuery.Query = $"SELECT {ALL_COLUMNS_PROPERTY} FROM {TABLE_NAME} WHERE {fieldName} {opr} @Keyword";
            extendedQuery.AddSoftDelete();
            extendedQuery.AddOrderBy(orderby);
            extendedQuery.AddPaging(pageSize, pageNumber);
            extendedQuery.AddParameter("Keyword", keyword);

            result = await _connection.QueryAsync<T>(extendedQuery.Query, extendedQuery.Parameters);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching records from db: ${ex.Message}");
            throw new Exception("Unable to fetch data. Please contact the administrator.");
        }
        finally
        {
            _connection.Close();
        }
        return result;
    }

    public async Task<IEnumerable<T>> GetPagedList(int pageSize, int pageNumber, string orderby = "")
    {
        IEnumerable<T> result;
        try
        {
            ExtendedQuery extendedQuery = new ExtendedQuery();
            extendedQuery.Query = $"SELECT {ALL_COLUMNS_PROPERTY} FROM {TABLE_NAME}"; ;
            extendedQuery.AddSoftDelete();
            extendedQuery.AddOrderBy(orderby);
            extendedQuery.AddPaging(pageSize, pageNumber);

            result = await _connection.QueryAsync<T>(extendedQuery.Query, extendedQuery.Parameters);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching records from db: ${ex.Message}");
            throw new Exception("Unable to fetch data. Please contact the administrator.");
        }
        finally
        {
            _connection.Close();
        }
        return result;

    }

    public async Task<IEnumerable<T>> GetPagedList(string query, object parameters, int pageSize, int pageNumber)
    {
        IEnumerable<T> result;
        try
        {
            ExtendedQuery extendedQuery = new ExtendedQuery();
            extendedQuery.Query = query;
            extendedQuery.SetParameters(parameters);
            extendedQuery.AddPaging(pageSize, pageNumber);

            result = await _connection.QueryAsync<T>(extendedQuery.Query, extendedQuery.Parameters);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching records from db: ${ex.Message}");
            throw new Exception("Unable to fetch data. Please contact the administrator.");
        }
        finally
        {
            _connection.Close();
        }
        return result;

    }
    #endregion

    #region Count

    public async Task<int> Count()
    {
        int result = -1;

        try
        {
            ExtendedQuery extendedQuery = new ExtendedQuery();
            extendedQuery.Query = $"SELECT COUNT(*) FROM {TABLE_NAME}";
            extendedQuery.AddSoftDelete();

            result = await QueryCount(extendedQuery.Query, null);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching records from db: ${ex.Message}");
            throw new Exception("Unable to fetch data. Please contact the administrator.");
        }
        finally
        {
            _connection.Close();
        }

        return result;
    }
    public async Task<int> Count(string fieldName, string keyword, string opr = "=")
    {
        int result = -1;
        try
        {
            ExtendedQuery extendedQuery = new ExtendedQuery();
            extendedQuery.Query = $"SELECT {ALL_COLUMNS_PROPERTY} FROM {TABLE_NAME} WHERE {fieldName} {opr} @Keyword";
            extendedQuery.AddSoftDelete();
            extendedQuery.AddParameter("Keyword", keyword);


            result = await _connection.QueryFirstOrDefaultAsync<int>(extendedQuery.Query, extendedQuery.Parameters);

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error counting records in db: ${ex.Message}");
            throw new Exception("Unable to count data. Please contact the administrator.");
        }
        finally
        {
            _connection.Close();
        }

        return result;
    }

    public async Task<int> QueryCount(string query, object parameters)
    {
        int result = -1;
        try
        {
            if (parameters != null)
            {
                ExtendedQuery extendedQuery = new ExtendedQuery();
                extendedQuery.Query = query;
                extendedQuery.SetParameters(parameters);

                result = await _connection.QueryFirstOrDefaultAsync<int>(extendedQuery.Query, extendedQuery.Parameters);
            }
            else
            {
                result = await _connection.QueryFirstOrDefaultAsync<int>(query);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error counting records in db: ${ex.Message}");
            throw new Exception("Unable to count data. Please contact the administrator.");
        }
        finally
        {
            _connection.Close();
        }

        return result;
    }



    #endregion

    #region IsExist

    public async Task<bool> IsExist(TId id)
    {
        bool result = false;
        try
        {

            string query = $"SELECT COUNT(*) FROM {TABLE_NAME} WHERE {KEY_COLUMN} =@Id";

            result = await QueryCount(query, new { Id = id }) > 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting a record in db: ${ex.Message}");
        }
        finally
        {
            _connection.Close();
        }


        return result;
    }

    public async Task<bool> IsExist(string fieldName, string fieldValue)
    {
        return await Get(fieldName, fieldValue) != null;
    }

    #endregion

    #region Add-Update-Delete

    public async Task<int> Add(T entity)
    {
        int rowsEffected = 0;
        try
        {
            string query = $"INSERT INTO {TABLE_NAME} ({COLUMNS}) VALUES ({PROPERTIES})";
            rowsEffected = await _connection.ExecuteAsync(query, entity);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding a record to db: ${ex.Message}");
            rowsEffected = -1;
        }
        finally
        {
            _connection.Close();
        }

        return rowsEffected;
    }

    public async Task<int> Insert(T entity)
    {
        return await Add(entity);
    }

    public async Task<int> Update(T entity)
    {
        int rowsEffected = 0;
        try
        {
            StringBuilder query = new StringBuilder();
            query.Append($"UPDATE {TABLE_NAME} SET ");

            foreach (var property in GetProperties(true))
            {
                var columnAttribute = property.GetCustomAttribute<ColumnAttribute>();

                string propertyName = property.Name;
                string columnAttributeName = columnAttribute?.Name ?? propertyName;


                query.Append($"{columnAttributeName} = @{propertyName},");
            }
            query.Remove(query.Length - 1, 1);

            query.Append($" WHERE {KEY_COLUMN} = @{KEY_PROPERTY}");

            rowsEffected = await _connection.ExecuteAsync(query.ToString(), entity);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating a record in db: ${ex.Message}");
            rowsEffected = -1;
        }
        finally
        {
            _connection.Close();
        }

        return rowsEffected;
    }

    public async Task<int> Delete(T entity)
    {
        int rowsEffected = 0;
        try
        {

            string query = $"DELETE FROM {TABLE_NAME} WHERE {KEY_COLUMN} = @{KEY_PROPERTY}";

            rowsEffected = await _connection.ExecuteAsync(query, entity);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting a record in db: ${ex.Message}");
            rowsEffected = -1;
        }
        finally
        {
            _connection.Close();
        }

        return rowsEffected;
    }

    public async Task<int> SoftDelete(T entity)
    {
        int rowsEffected = 0;
        try
        {
            string query = $"UPDATE {TABLE_NAME} SET  IsDeleted = true WHERE Id = @Id ";
            rowsEffected = await _connection.ExecuteAsync(query, entity);

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting a record in db: ${ex.Message}");
            rowsEffected = -1;
        }
        finally
        {
            _connection.Close();
        }

        return rowsEffected;
    }
    #endregion

}
