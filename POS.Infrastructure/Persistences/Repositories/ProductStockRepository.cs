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

        public async Task<bool> UpdateCurrentStockByProducts(ProductStock productStock)
        {
            _context.Update(productStock);
            var recordsUpdate = await _context.SaveChangesAsync();
            return recordsUpdate > 0;
        }
        public async Task<bool> UpdateCurrentStockByProducts(IEnumerable<ProductStock> productStock)
        {
            _context.UpdateRange(productStock);
            var recordsUpdate = await _context.SaveChangesAsync();
            return recordsUpdate > 0;
        }

        public async Task<ProductStock?> GetProductStockByProduct(int productId, int warehouseId)
        {
            var productStock = await _context.ProductStock
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ProductId == productId && x.WarehouseId == warehouseId);

            return productStock;
        }

        public async Task<IEnumerable<ProductStock>> GetProductStockByProduct(List<int> productsId, int warehouseId)
        {
            var productStock = await _context.ProductStock
                 .AsNoTracking()
                 .Where(x => productsId.Contains(x.ProductId) && x.WarehouseId == warehouseId)
                 .ToListAsync();

            return productStock;
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
                    PurcharsePrice = x.ProductStock.PurcharsePrice

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
