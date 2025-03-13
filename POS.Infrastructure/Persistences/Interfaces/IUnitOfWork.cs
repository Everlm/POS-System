using POS.Domain.Entities;

namespace POS.Infrastructure.Persistences.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {

        IGenericRepository<Category> Category { get; }
        IGenericRepository<Provider> Provider { get; }
        IGenericRepository<DocumentType> DocumentType { get; }
        IUserRepository User { get; }
        void SaveChanges();
        Task SaveChangesAsync();
    }
}
