using Microsoft.EntityFrameworkCore;
using POS.Domain.Entities;
using POS.Infrastructure.Commons.Bases.Request;
using POS.Infrastructure.Commons.Bases.Response;
using POS.Infrastructure.Persistences.Contexts;
using POS.Infrastructure.Persistences.Interfaces;

namespace POS.Infrastructure.Persistences.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly POSContext _context;
        public ProductRepository(POSContext context) : base(context)
        {
            _context = context;
        }

        public async Task<BaseEntityResponse<Product>> ListProducts(BaseFiltersRequest filters)
        {
            var response = new BaseEntityResponse<Product>();

            var products = GetEntityQuery(x => x.AuditDeleteUser == null && x.AuditDeleteDate == null)
                .Include(c => c.Category)
                .Include(p => p.Provider)
                .AsNoTracking();

            if (filters.NumFilter is not null && !string.IsNullOrEmpty(filters.TextFilter))
            {
                switch (filters.NumFilter)
                {
                    case 1:
                        products = products.Where(x => x.Name.Contains(filters.TextFilter));
                        break;

                    case 2:
                        products = products.Where(x => x.Category.Name.Contains(filters.TextFilter));
                        break;

                    case 3:
                        products = products.Where(x => x.Code.Contains(filters.TextFilter));
                        break;
                }
            }

            if (filters.StateFilter is not null)
            {
                products = products.Where(x => x.State.Equals(filters.StateFilter));
            }

            if (filters.StartDate is not null && filters.EndDate is not null)
            {
                products = products.Where(x => x.AuditCreateDate >= Convert.ToDateTime(filters.StartDate) &&
                                                x.AuditCreateDate <= Convert.ToDateTime(filters.EndDate).AddDays(1));
            }

            if (filters.Sort is null) filters.Sort = "Id";

            response.TotalRecords = await products.CountAsync();
            response.Items = await Ordering(filters, products, !(bool)filters.Download!).ToListAsync();
            return response;
        }

        public async Task<String> SearchProductCode(string code)
        {
            var product = await _context.Products.AsNoTracking().FirstOrDefaultAsync(c => c.Code!.Equals(code));
            return product.Code!;
        }
    }
}
