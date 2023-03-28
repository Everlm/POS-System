using Azure;
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

        public async Task<Sale> RegisterSale(Sale sale)
        {
            Sale Sale = new Sale();

            using (var transaction = _context.Database.BeginTransaction())
            {
                foreach (SaleDetail item in sale.SaleDetails)
                {
                    Product product = _context.Products.Where(p => p.Id.Equals(item.ProductId)).First();
                    product.Stock = product.Stock - item.Quantity;
                }

                return Sale;
            }
        }
    }
}

