using System.Data;

namespace POS.Infrastructure.Persistences.StoredProcedures
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}
