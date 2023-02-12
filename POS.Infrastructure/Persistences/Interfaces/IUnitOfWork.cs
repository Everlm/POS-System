namespace POS.Infrastructure.Persistences.Interfaces
{
    public interface IUnitOfWork :IDisposable
    {
        ICategoryRepository Category { get; }
        IUserRepository User { get; }
        void SaveChanges();
        Task SaveChangesAsync();
    }
}
