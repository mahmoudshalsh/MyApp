using Dapper;
using Microsoft.Data.SqlClient;

namespace MyApp.Infrastructure;

public class DapperHelper
{
    public DapperHelper()
    {
    }

    public static async Task<IEnumerable<dynamic>> QueryAsync(string connectionString, string sql, object? parameters = null)
    {
        try
        {
            using (var connection = new SqlConnection(connectionString))
            {
                IEnumerable<dynamic> results = await connection
                    .QueryAsync<dynamic>(sql, parameters, commandTimeout: 600)
                    .ConfigureAwait(false);
                return results;
            }
        }
        catch
        {
            throw;
        }
    }

    public static async Task<IEnumerable<T>> QueryAsync<T>(string connectionString, string sql, object? parameters = null)
    {
        using var sqlConnection = new SqlConnection(connectionString);
        IEnumerable<T> results = await sqlConnection
            .QueryAsync<T>(sql, parameters, commandTimeout: 600);
        return results;
    }

    public static async Task<IEnumerable<T>> GetQueryAsync<T>(string connectionString, string sql, object? parameters = null)
    {
        try
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                IEnumerable<T> results = await sqlConnection
                    .QueryAsync<T>(sql, parameters, commandTimeout: 600)
                    .ConfigureAwait(false);
                return results;
            }
        }
        catch
        {
            throw;
        }
    }


    public static async Task<T> QuerySingleOrDefaultAsync<T>(string connectionString, string sql, object? parameters = null)
    {
        try
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                return await sqlConnection
                    .QuerySingleOrDefaultAsync<T>(sql, parameters, commandTimeout: 600)
                    .ConfigureAwait(false);
            }
        }
        catch
        {
            throw;
        }
    }


    public static async Task<int> ExecuteAsync(string connectionString, string sql, object? parameters = null)
    {
        try
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                return await sqlConnection
                    .ExecuteAsync(sql, parameters, commandTimeout: 600)
                    .ConfigureAwait(false);
            }
        }
        catch
        {
            throw;
        }
    }

    public static async Task<T> ExecuteScalarAsync<T>(string connectionString, string sql, object? parameters = null)
    {
        try
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                return await sqlConnection
                    .ExecuteScalarAsync<T>(sql, parameters, commandTimeout: 600)
                    .ConfigureAwait(false);
            }
        }
        catch
        {
            throw;
        }
    }
}