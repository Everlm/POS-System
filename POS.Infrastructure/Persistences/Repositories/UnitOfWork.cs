using Microsoft.EntityFrameworkCore.Storage;
using POS.Domain.Entities;
using POS.Infrastructure.Persistences.Contexts;
using POS.Infrastructure.Persistences.Interfaces;
using System.Data;

namespace POS.Infrastructure.Persistences.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly POSContext _context;
        private IDbContextTransaction? _transaction;
        public IUserRepository User { get; }
        public IGenericRepository<Category> Category { get; }
        public IGenericRepository<Provider> Provider { get; }
        public IGenericRepository<DocumentType> DocumentType { get; }
        public IGenericRepository<Product> Product { get; }
        public IGenericRepository<Purcharse> Purcharse { get; }
        public IGenericRepository<Client> Client { get; }
        public IGenericRepository<Sale> Sale { get; }
        public IGenericRepository<VoucherDocumentType> VoucherDoumentType { get; }
        public IWarehouseRepository Warehouse { get; }
        public IProductStockRepository ProductStock { get; }
        public IPurcharseDetailRepository PurcharseDetail { get; }
        public ISaleDetailRepository SaleDetail { get; }
        public IUserRoleRepository UserRole { get; }
        public IRoleRepository Role { get; }

        public UnitOfWork(
            POSContext context,
            IUserRepository user,
            IGenericRepository<Category> category,
            IGenericRepository<Provider> provider,
            IGenericRepository<DocumentType> documentType,
            IGenericRepository<Product> product,
            IGenericRepository<Purcharse> purcharse,
            IGenericRepository<Client> client,
            IGenericRepository<Sale> sale,
            IGenericRepository<VoucherDocumentType> voucherDocumentType,
            IWarehouseRepository warehouse,
            IProductStockRepository productStock,
            IPurcharseDetailRepository purcharseDetail,
            ISaleDetailRepository saleDetail,
            IUserRoleRepository userRole,
            IRoleRepository role)
        {
            _context = context;
            User = user;
            Category = category;
            Provider = provider;
            DocumentType = documentType;
            Product = product;
            Purcharse = purcharse;
            Client = client;
            Sale = sale;
            VoucherDoumentType = voucherDocumentType;
            Warehouse = warehouse;
            ProductStock = productStock;
            PurcharseDetail = purcharseDetail;
            SaleDetail = saleDetail;
            UserRole = userRole;
            Role = role;
        }

        public IDbTransaction BeginTransaction()
        {
            var transaction = _context.Database.BeginTransaction();
            return transaction.GetDbTransaction();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            if (_transaction == null)
            {
                _transaction = await _context.Database.BeginTransactionAsync();
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }

        }
    }
}
