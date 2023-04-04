using POS.Domain.Entities;
using POS.Infrastructure.Commons.Bases.Request;
using POS.Infrastructure.Commons.Bases.Response;

namespace POS.Infrastructure.Persistences.Interfaces
{
    public interface IPurchaseRepository
    {
        Task<BaseEntityResponse<Purcharse>> ListPurchases(BaseFiltersRequest filters);
        Task<bool> RegisterPurchase(Purcharse purchase);
    }
}
