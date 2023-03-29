using Microsoft.EntityFrameworkCore;
using POS.Domain.Entities;
using POS.Infrastructure.Commons.Bases.Request;
using POS.Infrastructure.Commons.Bases.Response;
using POS.Infrastructure.Persistences.Contexts;
using POS.Infrastructure.Persistences.Interfaces;

namespace POS.Infrastructure.Persistences.Repositories
{
    public class SaleRepository : GenericRepository<Sale>, ISaleRepository
    {
        private readonly POSContext _context;

        public SaleRepository(POSContext context) : base(context)
        {
            _context = context;
        }

        public async Task<BaseEntityResponse<Sale>> ListSales(BaseFiltersRequest filters)
        {

            var response = new BaseEntityResponse<Sale>();

            var sales = GetEntityQuery(x => x.AuditDeleteUser == null && x.AuditDeleteDate == null)
                .Include(c => c.Client)
                .Include(u => u.User)
                .Include(s => s.SaleDetails)
                .AsNoTracking();

            if (filters.NumFilter is not null && !string.IsNullOrEmpty(filters.TextFilter))
            {
                switch (filters.NumFilter)
                {
                    case 1:
                        sales = sales.Where(x => x.Client.Name.Contains(filters.TextFilter));
                        break;

                    case 2:
                        sales = sales.Where(x => x.User.UserName.Contains(filters.TextFilter));
                        break;
                }
            }

            if (filters.StateFilter is not null)
            {
                sales = sales.Where(x => x.State.Equals(filters.StateFilter));
            }

            if (filters.StartDate is not null && filters.EndDate is not null)
            {
                sales = sales.Where(x => x.AuditCreateDate >= Convert.ToDateTime(filters.StartDate) &&
                                                x.AuditCreateDate <= Convert.ToDateTime(filters.EndDate).AddDays(1));
            }

            if (filters.Sort is null) filters.Sort = "Id";

            response.TotalRecords = await sales.CountAsync();
            response.Items = await Ordering(filters, sales, !(bool)filters.Download!).ToListAsync();
            return response;
        }

        public async Task<bool> RegisterSale(Sale sale)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    decimal total = 0;              
                    var productsIds = sale.SaleDetails.Select(x => x.ProductId).ToList();
                    var produducts = await _context.Products.Where(p => productsIds.Contains(p.Id)).ToListAsync();

                    foreach (SaleDetail item in sale.SaleDetails)
                    {
                        Product product = produducts.FirstOrDefault(p => p.Id == item.ProductId);

                        if (product == null)
                        {
                            throw new Exception($"Product with ID {item.ProductId} does not exist.");
                        }

                        product.Stock -= item.Quantity;
                        decimal subTotal = item.Price * item.Quantity;
                        decimal discountAmount = subTotal * (item.Discount ?? 0) / 100;
                        total += subTotal - discountAmount;                     

                        _context.Products.Update(product);
                    }
                    
                    sale.Total = total;
                                  
                    await _context.Sales.AddAsync(sale);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;                   
                }
                return true;
            }
        }
    }
}

