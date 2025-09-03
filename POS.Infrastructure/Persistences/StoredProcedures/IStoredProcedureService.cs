namespace POS.Infrastructure.Persistences.StoredProcedures
{
    public interface IStoredProcedureService
    {
        Task<IEnumerable<T>> ExecuteQueryAsync<T>(string procedureName, object? parameters = null);
        Task<int> ExecuteNonQueryAsync(string procedureName, object? parameters = null);
    }
}