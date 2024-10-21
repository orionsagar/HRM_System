using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repository
{
    public interface ICustomRepository
    {
        //Task<int> Add(string sqlQuery, string extraQuery);
        //Task<int> Delete(string sqlQuery, string extraQuery);
        //Task<string> Get(string sqlQuery, string extraQuery);
        //Task<int> GetCountOne(string sqlQuery, string extraQuery);       
        //Task<List<string>> GetList(string sqlQuery, string extraQuery);
        //Task<IEnumerable<T>> GetListT<T>(string sqlQuery, string extraQuery) where T : class;

        ////Task<List<SelectListItem>> GetSelectList(string sqlQuery, string extraQuery);
        //Task<T> GetT<T>(string sqlQuery, string extraQuery) where T : class;
        //T InsertT<T>(string query, T entity) where T : class;
        //Task<int> Update(string sqlQuery, string extraQuery);
        //T UpdateT<T>(string query, T entity) where T : class;
    }

    public class CustomRepository : ICustomRepository
    {
        //public async Task<int> Add(string sqlQuery, string extraQuery)
        //{
        //    try
        //    {
        //        var sql = sqlQuery;
        //        using (var connection = CreateConnection())
        //        {
        //            connection.Open();
        //            var affectedRows = await connection.ExecuteScalarAsync<int>(sql + extraQuery);
        //            return affectedRows;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}


        //public async Task<int> Delete(string sqlQuery, string extraQuery)
        //{
        //    var sql = sqlQuery;
        //    using (var connection = new SqlConnection(AppConfig.DefaultConnection))
        //    {
        //        connection.Open();
        //        var affectedRows = await connection.ExecuteAsync(sql + extraQuery);
        //        return affectedRows;
        //    }
        //}

        //public async Task<string> Get(string sqlQuery, string extraQuery)
        //{
        //    var sql = sqlQuery;
        //    using (var connection = new SqlConnection(AppConfig.DefaultConnection))
        //    {
        //        connection.Open();
        //        var affectedRows = await connection.ExecuteScalarAsync<string>(sql + extraQuery);
        //        return affectedRows;
        //    }
        //}
        //public async Task<T> GetT<T>(string sqlQuery, string extraQuery) where T : class
        //{
        //    var sql = sqlQuery;
        //    using var connection = new SqlConnection(AppConfig.DefaultConnection);
        //    connection.Open();
        //    var result = await connection.QueryFirstOrDefaultAsync<T>(sql + extraQuery);
        //    return result;
        //}

        //public async Task<int> GetCountOne(string sqlQuery, string extraQuery)
        //{
        //    var sql = sqlQuery;
        //    using (var connection = new SqlConnection(AppConfig.DefaultConnection))
        //    {
        //        connection.Open();
        //        var affectedRows = await connection.ExecuteScalarAsync<int>(sql + extraQuery);
        //        return affectedRows;
        //    }
        //}



        //public async Task<List<string>> GetList(string sqlQuery, string extraQuery)
        //{
        //    var sql = sqlQuery;
        //    using (var connection = new SqlConnection(AppConfig.DefaultConnection))
        //    {
        //        connection.Open();
        //        var result = await connection.QueryAsync<string>(sql + extraQuery);
        //        return result.ToList();
        //    }
        //}


        //public async Task<IEnumerable<T>> GetListT<T>(string sqlQuery, string extraQuery) where T : class
        //{
        //    var sql = sqlQuery;
        //    using (var connection = new SqlConnection(AppConfig.DefaultConnection))
        //    {
        //        connection.Open();
        //        var result = await connection.QueryAsync<T>(sql + extraQuery);
        //        return result;
        //    }
        //}





        //public async Task<List<SelectListItem>> GetSelectList(string sqlQuery, string extraQuery)
        //{
        //    var sql = sqlQuery;
        //    using (var connection = new SqlConnection(AppConfig.DefaultConnection))
        //    {
        //        connection.Open();
        //        var result = await connection.QueryAsync<SelectListItem>(sql + extraQuery);
        //        return result.ToList();
        //    }
        //}


        //public async Task<int> Update(string sqlQuery, string extraQuery)
        //{
        //    var sql = sqlQuery;
        //    using (var connection = new SqlConnection(AppConfig.DefaultConnection))
        //    {
        //        connection.Open();
        //        var affectedRows = await connection.ExecuteAsync(sql + extraQuery);
        //        return affectedRows;
        //    }
        //}

        ////Task<string> ICustomRepository.Get(string sqlQuery, string extraQuery)
        ////{
        ////    throw new NotImplementedException();
        ////}

        //public T InsertT<T>(string query, T entity) where T : class
        //{
        //    T result;
        //    using IDbConnection db = new SqlConnection(AppConfig.DefaultConnection);
        //    try
        //    {
        //        if (db.State == ConnectionState.Closed)
        //            db.Open();

        //        using var tran = db.BeginTransaction();
        //        try
        //        {
        //            result = db.Query<T>(query, entity).FirstOrDefault();
        //            tran.Commit();
        //        }
        //        catch (Exception ex)
        //        {
        //            tran.Rollback();
        //            throw ex;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        if (db.State == ConnectionState.Open)
        //            db.Close();
        //    }

        //    return result;
        //}

        //public T UpdateT<T>(string query, T entity) where T : class
        //{
        //    T result;
        //    using IDbConnection db = new SqlConnection(AppConfig.DefaultConnection);
        //    try
        //    {
        //        if (db.State == ConnectionState.Closed)
        //            db.Open();

        //        using var tran = db.BeginTransaction();
        //        try
        //        {
        //            result = db.Query<T>(query, entity).FirstOrDefault();
        //            tran.Commit();
        //        }
        //        catch (Exception ex)
        //        {
        //            tran.Rollback();
        //            throw ex;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        if (db.State == ConnectionState.Open)
        //            db.Close();
        //    }
        //    return result;
        //}
    }
}
