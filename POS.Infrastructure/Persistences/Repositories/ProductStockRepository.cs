using Microsoft.EntityFrameworkCore;
using POS.Domain.Entities;
using POS.Infrastructure.Persistences.Contexts;
using POS.Infrastructure.Persistences.Interfaces;

namespace POS.Infrastructure.Persistences.Repositories
{
    public class ProductStockRepository : IProductStockRepository
    {
        private readonly POSContext _context;

        public ProductStockRepository(POSContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductStock>> GetProductStockByWarehouse(int productId)
        {
            var query = await _context.ProductStock
                .AsNoTracking()
                .Join(_context.Warehouses, ps => ps.WarehouseId, w => w.Id, (ps, w)
                    => new { ProductStock = ps, Warehouse = w })
                .Where(x => x.ProductStock.ProductId == productId)
                .OrderBy(x => x.Warehouse.Id)
                .Select(x => new ProductStock
                {
                    Warehouse = new Warehouse { Name = x.Warehouse.Name },
                    CurrentStock = x.ProductStock.CurrentStock,
                    PurchasePrice = x.ProductStock.PurchasePrice

                }).ToArrayAsync();

            return query;
        }

        public async Task<bool> RegisterProductStockAsync(ProductStock productStock)
        {
            await _context.AddAsync(productStock);
            var recordsAffected = await _context.SaveChangesAsync();
            return recordsAffected > 0;
        }
    }
}
