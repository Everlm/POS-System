namespace POS.Infrastructure.Persistences.Interfaces
{
    public interface IUnitOfWork :IDisposable
    {
        ICategoryRepository Category { get; }
        void SaveChanges();
        Task SaveChangesAsync();
    }
}
