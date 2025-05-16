using POS.Domain.Entities;

namespace POS.Infrastructure.Persistences.Interfaces;
public interface ISaleDetailRepository
{
    Task<IEnumerable<SaleDetail>> GetSaleDetailBySaleId(int saleId);
    IQueryable<Product> GetProductStockByWarehouse(int warehouseId);
}
