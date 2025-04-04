using POS.Domain.Entities;

namespace POS.Infrastructure.Persistences.Interfaces
{
    public interface IProductStockRepository
    {
        Task<bool> RegisterProductStockAsync(ProductStock productStock);
        Task<IEnumerable<ProductStock>> GetProductStockByWarehouse(int productId);
        Task<ProductStock?> GetProductStockByProduct(int productId, int warehouseId);
        Task<bool> UpdateCurrentStockByProducts(ProductStock productStock);
        Task<bool> UpdateCurrentStockByProducts(IEnumerable<ProductStock> productStock);
        Task<IEnumerable<ProductStock>> GetProductStockByProduct(List<int> productsId, int warehouseId);
    }
}
