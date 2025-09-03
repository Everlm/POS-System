using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace POS.Infrastructure.Persistences.StoredProcedures
{
    public class StoredProcedureService : IStoredProcedureService
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public StoredProcedureService(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<T>> ExecuteQueryAsync<T>(string procedureName, object? parameters = null)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryAsync<T>(
                procedureName,
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> ExecuteNonQueryAsync(string procedureName, object? parameters = null)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.ExecuteAsync(
                procedureName,
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

    }
}