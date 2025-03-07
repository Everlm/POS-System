using Microsoft.Extensions.Configuration;
using POS.Infrastructure.FileStorage;
using POS.Infrastructure.Persistences.Contexts;
using POS.Infrastructure.Persistences.Interfaces;

namespace POS.Infrastructure.Persistences.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly POSContext _context;
        public ICategoryRepository Category { get; private set; }
        public IUserRepository User { get; private set; }
        public IProviderRepository Provider { get; private set; }
        public IProductRepository Product { get; private set; }
        public IClientRepository Client { get; private set; }
        public ISaleRepository Sale { get; private set; }
        public IPurchaseRepository Purcharse { get; private set; }
        public IDepartmentRepository Department { get; private set; }
        public IProvinceRepository Province { get; private set; }
        public IDistrictRepository District { get; private set; }
        public IBusinessRepository Business { get; private set; }
        public IBranchOfficeRepository BranchOffice { get; private set; }
        public IAzureStorage AzureStorage { get; private set; }
        public IDocumentTypeRepository DocumentType { get; private set; }

        public UnitOfWork(POSContext context, IConfiguration configuration)
        {
            _context = context;
            Category = new CategoryRepository(_context);
            User = new UserRepository(_context);
            Provider = new ProviderRepository(_context);
            DocumentType = new DocumentTypeRepository(_context);
            Product = new ProductRepository(_context);
            Client = new ClientRepository(_context);
            Sale = new SaleRepository(_context);
            Purcharse = new PurchaseRepository(_context);
            Department = new DepartmentRepository(_context);
            Province = new ProvinceRepository(_context);
            District = new DistrictRepository(_context);
            Business = new BusinessRepository(_context);
            BranchOffice = new BranchOfficeRepository(_context);
            AzureStorage = new AzureStorage(configuration);

        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }


    }
}
