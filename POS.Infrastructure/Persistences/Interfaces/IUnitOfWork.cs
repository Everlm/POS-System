﻿using POS.Domain.Entities;
using System.Data;

namespace POS.Infrastructure.Persistences.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Category> Category { get; }
        IGenericRepository<Provider> Provider { get; }
        IGenericRepository<DocumentType> DocumentType { get; }
        IGenericRepository<Product> Product { get; }
        IGenericRepository<Purcharse> Purcharse { get; }
        IGenericRepository<Client> Client { get; }
        IGenericRepository<Sale> Sale { get; }
        IGenericRepository<VoucherDocumentType> VoucherDoumentType { get; }
        IPurcharseDetailRepository PurcharseDetail { get; }
        ISaleDetailRepository SaleDetail { get; }
        IProductStockRepository ProductStock { get; }
        IWarehouseRepository Warehouse { get; }
        IUserRepository User { get; }
        IUserRoleRepository UserRole { get; }
        IRoleRepository Role { get; }
       
        void SaveChanges();
        Task SaveChangesAsync();
        IDbTransaction BeginTransaction();
        Task BeginTransactionAsync();
        Task RollbackTransactionAsync();
        Task CommitTransactionAsync();
    }
}
