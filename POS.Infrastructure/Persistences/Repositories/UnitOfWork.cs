﻿using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using POS.Domain.Entities;
using POS.Infrastructure.Persistences.Contexts;
using POS.Infrastructure.Persistences.Interfaces;
using System.Data;

namespace POS.Infrastructure.Persistences.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly POSContext _context;

        private IUserRepository _user = null!;
        private IGenericRepository<Category> _category = null!;
        private IGenericRepository<Provider> _provider = null!;
        private IGenericRepository<DocumentType> _documentType = null!;
        private IGenericRepository<Product> _product = null!;
        private IGenericRepository<Purcharse> _purcharse = null!;
        private IGenericRepository<Client> _client = null!;
        private IGenericRepository<Sale> _sale = null!;
        private IGenericRepository<VoucherDocumentType> _voucherDocumentType = null!;
        private IWarehouseRepository _warehouse = null!;
        private IProductStockRepository _productStock = null!;
        private IPurcharseDetailRepository _purcharseDetail = null!;
        private ISaleDetailRepository _saleDetail = null!;
        private IUserRoleRepository _userRole = null!;
        private IRoleRepository _role = null!;

        public UnitOfWork(POSContext context, IConfiguration configuration)
        {
            _context = context;
        }

        public IUserRepository User => _user ?? new UserRepository(_context);
        public IGenericRepository<Category> Category => _category ?? new GenericRepository<Category>(_context);
        public IGenericRepository<Provider> Provider => _provider ?? new GenericRepository<Provider>(_context);
        public IGenericRepository<DocumentType> DocumentType => _documentType ?? new GenericRepository<DocumentType>(_context);
        public IGenericRepository<Product> Product => _product ?? new GenericRepository<Product>(_context);
        public IGenericRepository<Purcharse> Purcharse => _purcharse ?? new GenericRepository<Purcharse>(_context);
        public IGenericRepository<Client> Client => _client ?? new GenericRepository<Client>(_context);
        public IGenericRepository<Sale> Sale => _sale ?? new GenericRepository<Sale>(_context);
        public IGenericRepository<VoucherDocumentType> VoucherDoumentType => _voucherDocumentType ?? new GenericRepository<VoucherDocumentType>(_context);
        public IWarehouseRepository Warehouse => _warehouse ?? new WarehouseRepository(_context);
        public IProductStockRepository ProductStock => _productStock ?? new ProductStockRepository(_context);
        public IPurcharseDetailRepository PurcharseDetail => _purcharseDetail ?? new PurcharseDetailRepository(_context);
        public ISaleDetailRepository SaleDetail => _saleDetail ?? new SaleDetailRepository(_context);
        public IUserRoleRepository UserRole => _userRole ?? new UserRoleRepository(_context);
        public IRoleRepository Role => _role ?? new RoleRepository(_context);


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
    }
}
