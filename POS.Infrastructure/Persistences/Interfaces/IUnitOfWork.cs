using POS.Infrastructure.FileStorage;

namespace POS.Infrastructure.Persistences.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ICategoryRepository Category { get; }
        IUserRepository User { get; }
        IProviderRepository Provider { get; }
        IProductRepository Product { get; }
        IClientRepository Client { get; }
        ISaleRepository Sale { get; }
        IPurchaseRepository Purcharse { get; }
        IDepartmentRepository Department { get; }
        IProvinceRepository Province { get; }
        IDistrictRepository District { get; }
        IBusinessRepository Business { get; }
        IBranchOfficeRepository BranchOffice { get; }
        IAzureStorage AzureStorage { get; }
        void SaveChanges();
        Task SaveChangesAsync();
    }
}
