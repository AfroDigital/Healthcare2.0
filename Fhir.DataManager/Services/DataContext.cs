using Microsoft.Data.SqlClient;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace Fhir.DataManager.Services;

public class DataContext
{
    private readonly IConfiguration _configuration;
    private readonly string _connectionString;

    public DataContext(IConfiguration configuration)
    {
        _configuration = configuration;
        _connectionString = _configuration["ConnectionStrings:DataConnection"];
    }

    public IDbConnection CreateSqlConnection()
        => new SqlConnection(_connectionString);

    public IDbConnection CreateOracleConnection()
    => new OracleConnection(_connectionString);
}
