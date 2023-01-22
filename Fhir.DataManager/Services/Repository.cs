using Dapper;
using ICSharpCode.SharpZipLib.Zip;
using System.Data.Common;
using System.Data;
using System.Xml.Linq;
using System.Collections.Generic;
using Snowflake.Data.Client;

namespace Fhir.DataManager.Services;

public interface IRepository
{
    Task<IEnumerable<dynamic>> Get(string query, string dataSource);
 
}


public class Repository : IRepository
{
    private readonly DataContext _context;
    public Repository(DataContext context)
	{
        _context= context;
	}


    public async Task<IEnumerable<dynamic>> Get(string query, string dataSource)
    {
       if(string.IsNullOrEmpty(dataSource))
       {
            throw new ArgumentNullException($"{nameof(dataSource)} is required");
       }

        if (dataSource == DataSources.SqlDataSource)
        {
            return await GetSqlData(query);

        } else if (dataSource == DataSources.OracleDataSource)
        {
            return await GetOracleData(query);
        }

        return new List<dynamic>();
    }

    private async Task<IEnumerable<dynamic>> GetSqlData(string query)
    {
        using (var connection = _context.CreateSqlConnection())
        {
            try
            {
                var response = await connection.QueryAsync(query).ConfigureAwait(true);
                return response.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    private async Task<IEnumerable<dynamic>> GetOracleData(string query)
    {
        using (var connection = _context.CreateOracleConnection())
        {
            try
            {
                var response = await connection.QueryAsync(query);
                return response.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    private IEnumerable<dynamic> GetSnowFlakeData(string query)
    {
        var rseponse = new List<dynamic>();
        try
        {
            using (IDbConnection conn = new SnowflakeDbConnection())
            {
                conn.ConnectionString = "account=<accountname>;user=.< xxxxxx >; password =< xxxxxx >; ROLE = ACCOUNTADMIN; db =< DBNAME >; schema =< schemaname > ";
                    conn.Open();
                Console.WriteLine("Connection successful!");
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "USE WAREHOUSE XXXX_WAREHOUSE";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = "select * from TABLE1";   // sql opertion fetching 
                                                                //data from an existing table
                    IDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        //rseponse.Add((dynamic)reader.Get)
                        Console.WriteLine(reader.GetValue(0));
                    }
                    conn.Close();
                }
            }
        }
        catch (DbException exc)
        {
            Console.WriteLine("Error Message: {0}", exc.Message);
        }

        return rseponse;
    }


}
