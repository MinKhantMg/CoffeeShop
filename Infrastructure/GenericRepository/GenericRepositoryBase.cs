using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Domain.Base;
using Domain.Database;

namespace Infrastructure.GenericRepository;

public class GenericRepositoryBase<T> where T : class
{
    protected readonly IDbConnection _connection;

    protected string TABLE_NAME;
    protected string? KEY_COLUMN;
    protected string? KEY_PROPERTY;
    protected string COLUMNS;
    protected string COLUMNS_NO_ID;
    protected string ALL_COLUMNS_PROPERTY;
    protected string PROPERTIES;
    protected string PROPERTIES_NO_ID;

    public GenericRepositoryBase(ApplicationDbContext context)
    {
        _connection = context.CreateConnection();
        TABLE_NAME = GetTableName();

        KEY_COLUMN = GetKeyColumnName();
        KEY_PROPERTY = GetKeyPropertyName();

        COLUMNS = GetColumns();
        COLUMNS_NO_ID = GetColumns(excludeKey: true);

        PROPERTIES = GetPropertyNames();
        PROPERTIES_NO_ID = GetPropertyNames(excludeKey: true);
        ALL_COLUMNS_PROPERTY = GetColumnsAsProperties();
    }

    #region PrivateMethods

    protected string GetTableName()
    {
        var type = typeof(T);
        var tableAttribute = type.GetCustomAttribute<TableAttribute>();
        if (tableAttribute != null)
            return tableAttribute.Name;

        return type.Name;
    }

    protected string? GetKeyColumnName()
    {
        PropertyInfo[] properties = typeof(T).GetProperties();

        foreach (PropertyInfo property in properties)
        {
            object[] keyAttributes = property.GetCustomAttributes(typeof(KeyAttribute), true);

            if (keyAttributes != null && keyAttributes.Length > 0)
            {
                object[] columnAttributes = property.GetCustomAttributes(typeof(ColumnAttribute), true);

                if (columnAttributes != null && columnAttributes.Length > 0)
                {
                    ColumnAttribute columnAttribute = (ColumnAttribute)columnAttributes[0];
                    return columnAttribute?.Name ?? "";
                }
                else
                {
                    return property.Name;
                }
            }
        }

        return null;
    }


    protected string GetColumns(bool excludeKey = false)
    {
        var type = typeof(T);
        var columns = string.Join(", ", type.GetProperties()
            .Where(p => !excludeKey || !p.IsDefined(typeof(KeyAttribute)))
            .Select(p =>
            {
                var columnAttribute = p.GetCustomAttribute<ColumnAttribute>();
                return columnAttribute != null ? columnAttribute.Name : p.Name;
            }));

        return columns;
    }

    protected string GetColumnsAsProperties(bool excludeKey = false)
    {
        var type = typeof(T);
        var columnsAsProperties = string.Join(", ", type.GetProperties()
            .Where(p => !excludeKey || !p.IsDefined(typeof(KeyAttribute)))
            .Select(p =>
            {
                var columnAttribute = p.GetCustomAttribute<ColumnAttribute>();
                return columnAttribute != null ? $"{columnAttribute.Name} AS {p.Name}" : p.Name;
            }));

        return columnsAsProperties;
    }

    protected string GetPropertyNames(bool excludeKey = false)
    {
        var properties = typeof(T).GetProperties()
            .Where(p => !excludeKey || p.GetCustomAttribute<KeyAttribute>() == null);

        var values = string.Join(", ", properties.Select(p => $"@{p.Name}"));

        return values;
    }

    protected string? GetKeyPropertyName()
    {
        var properties = typeof(T).GetProperties()
            .Where(p => p.GetCustomAttribute<KeyAttribute>() != null).ToList();

        if (properties.Any())
            return properties?.FirstOrDefault()?.Name ?? null;

        return null;
    }

    protected IEnumerable<PropertyInfo> GetProperties(bool excludeKey = false)
    {
        var properties = typeof(T).GetProperties()
            .Where(p => !excludeKey || p.GetCustomAttribute<KeyAttribute>() == null);

        return properties;
    }

    //protected IDictionary<string, object?> ToDictionary(object obj)
    //{
    //    return obj.GetType().GetProperties().ToDictionary
    //    (
    //        propInfo => propInfo.Name,
    //        propInfo => propInfo.GetValue(obj, null)
    //    );
    //}

    protected string EncodeForSQLLike(string keyword)
    {
        return keyword.Replace("[", "[[]").Replace("%", "[%]");
    }
    protected bool IsSoftDelete()
    {
        bool result = false;

        Type t = typeof(T);

        if (t is ISoftDelete) result = true;

        return result;
    }
    //protected DynamicParameters ToParams(object obj)
    //{
    //    var parameters = ToDictionary(obj);

    //    DynamicParameters dynamicParameters = new DynamicParameters();
    //    foreach (var key in parameters.Keys)
    //    {
    //        dynamicParameters.Add(key, parameters[key]);
    //    }

    //    return dynamicParameters;
    //}


    #endregion

}

public class ExtendedQuery
{
    string _query = string.Empty;
    DynamicParameters _parameters = new DynamicParameters();

    public string Query
    {
        get { return _query; }
        set { _query = value; }
    }

    public DynamicParameters Parameters
    {
        get { return _parameters; }
        set { _parameters = value; }
    }

    public void SetParameters(object obj)
    {

        if (obj.GetType() != typeof(DynamicParameters))
        {
            var dictionaryParams = ToDictionary(obj);

            foreach (var key in dictionaryParams.Keys)
            {
                if (!_parameters.ParameterNames.Contains(key))
                    _parameters.Add(key, dictionaryParams[key]);
            }
        }
    }

    public void AddParameter(string key, object val)
    {

        if (!string.IsNullOrEmpty(key) && val != null)
            _parameters.Add(key, val);

    }

    public void AddPaging(int pageSize, int pageNumber)
    {
        _query = _query + $" Limit @Limit Offset @Offset";

        AddParameter("Limit", pageSize);
        AddParameter("Offset", (pageNumber - 1) * pageSize);

    }

    public void AddOrderBy(string orderby)
    {
        if (!string.IsNullOrEmpty(orderby))
        {
            _query += " ORDERY BY " + orderby;
        }
    }

    public void AddSoftDelete()
    {
        if (_query.FindWord("WHERE"))
            _query += " AND IsDeleted=false";
        else
            _query += " WHERE IsDeleted=false";

    }

    protected IDictionary<string, object?> ToDictionary(object obj)
    {
        return obj.GetType().GetProperties().ToDictionary
        (
            propInfo => propInfo.Name,
            propInfo => propInfo.GetValue(obj, null)
        );
    }
}