using Microsoft.EntityFrameworkCore;
using POS.Domain.Entities;
using POS.Infrastructure.Commons.Bases.Request;
using POS.Infrastructure.Commons.Bases.Response;
using POS.Infrastructure.Persistences.Contexts;
using POS.Infrastructure.Persistences.Interfaces;

namespace POS.Infrastructure.Persistences.Repositories
{
    public class PurchaseRepository : GenericRepository<Purcharse>, IPurchaseRepository
    {
        private readonly POSContext _context;

        public PurchaseRepository(POSContext context) : base(context)
        {
            _context = context;
        }

        public async Task<BaseEntityResponse<Purcharse>> ListPurchases(BaseFiltersRequest filters)
        {
            var response = new BaseEntityResponse<Purcharse>();

            var purchase = GetEntityQuery(x => x.AuditDeleteUser == null && x.AuditDeleteDate == null)
                .Include(c => c.Provider)
                .Include(u => u.User)
                .Include(s => s.PurcharseDetails)
                .AsNoTracking();

            if (filters.NumFilter is not null && !string.IsNullOrEmpty(filters.TextFilter))
            {
                switch (filters.NumFilter)
                {
                    case 1:
                        purchase = purchase.Where(x => x.Provider.Name.Contains(filters.TextFilter));
                        break;

                    case 2:
                        purchase = purchase.Where(x => x.User.UserName.Contains(filters.TextFilter));
                        break;
                }
            }

            if (filters.StateFilter is not null)
            {
                purchase = purchase.Where(x => x.State.Equals(filters.StateFilter));
            }

            if (filters.StartDate is not null && filters.EndDate is not null)
            {
                purchase = purchase.Where(x => x.AuditCreateDate >= Convert.ToDateTime(filters.StartDate) &&
                                                x.AuditCreateDate <= Convert.ToDateTime(filters.EndDate).AddDays(1));
            }

            if (filters.Sort is null) filters.Sort = "Id";

            response.TotalRecords = await purchase.CountAsync();
            response.Items = await Ordering(filters, purchase, !(bool)filters.Download!).ToListAsync();
            return response;
        }

        public async Task<bool> RegisterPurchase(Purcharse purchase)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    decimal total = 0;
                    var productsIds = purchase.PurcharseDetails.Select(x => x.ProductId).ToList();
                    var products = await _context.Products.Where(p => productsIds.Contains(p.Id)).ToListAsync();

                    foreach (PurcharseDetail item in purchase.PurcharseDetails)
                    {
                        Product product = products.FirstOrDefault(p => p.Id == item.ProductId);

                        if (product == null)
                        {
                            throw new Exception($"Product with ID {item.ProductId} does not exist.");
                        }

                        product.Stock += item.Quantity;
                        decimal subTotal = item.Price * item.Quantity;
                        decimal taxAmount = subTotal * (purchase.Tax ?? 0) / 100;
                        total += subTotal + taxAmount;

                        _context.Products.Update(product);
                    }

                    purchase.Total = total;

                    await _context.Purcharses.AddAsync(purchase);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return true;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception(ex.Message);
                }

            }
        }
    }
}
