using POS.Domain.Entities;

namespace POS.Infrastructure.Persistences.Interfaces
{
    public interface IProductStockRepository
    {
        Task<bool> RegisterProductStockAsync(ProductStock productStock);
        Task<IEnumerable<ProductStock>> GetProductStockByWarehouse(int productId);
    }
}
