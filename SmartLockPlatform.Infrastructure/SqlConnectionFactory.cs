using System.Data;
using Npgsql;
using SmartLockPlatform.Application.Base;

namespace SmartLockPlatform.Infrastructure;

internal class SqlConnectionFactory : ISqlConnectionFactory, IDisposable
{
    private readonly string _connectionString;
    private IDbConnection? _connection;

    public SqlConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IDbConnection GetOpenConnection()
    {
        if (_connection is { State: ConnectionState.Open }) return _connection;

        _connection = new NpgsqlConnection(_connectionString);
        _connection.Open();

        return _connection;
    }
    
    public IDbConnection CreateConnection()
    {
        var connection = new NpgsqlConnection(_connectionString);
        connection.Open();
    
        return connection;
    }
    

    public void Dispose()
    {
        if (_connection is { State: ConnectionState.Open })
        {
            _connection.Dispose();
        }
    }
}