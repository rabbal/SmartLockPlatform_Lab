using System.Data;

namespace SmartLockPlatform.Application.Base;

public interface ISqlConnectionFactory
{
    IDbConnection GetOpenConnection();
    IDbConnection CreateConnection();
}