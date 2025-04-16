using AutoMapper;
using Microsoft.EntityFrameworkCore;
using POS.Application.Commons.Bases.Request;
using POS.Application.Commons.Bases.Response;
using POS.Application.Commons.Filters;
using POS.Application.Commons.Ordering;
using POS.Application.Dtos.Client.Response;
using POS.Application.Dtos.Sale.Response;
using POS.Application.Interfaces;
using POS.Domain.Entities;
using POS.Infrastructure.Persistences.Interfaces;
using POS.Utilities.Static;
using System.Linq.Expressions;

namespace POS.Application.Services
{
    public class SaleApplication : ISaleApplication
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderingQuery _orderingQuery;
        private readonly IFilterService _filterService;

        public SaleApplication(IMapper mapper, IUnitOfWork unitOfWork, IOrderingQuery orderingQuery, IFilterService filterService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _orderingQuery = orderingQuery;
            _filterService = filterService;
        }

        public async Task<BaseResponse<IEnumerable<SaleResponseDto>>> ListSale(BaseFiltersRequest filters)
        {
            var response = new BaseResponse<IEnumerable<SaleResponseDto>>();

            var saleQueryable = _unitOfWork.Sale
                .GetAllQueryable()
                .Include(x => x.Warehouse)
                .Include(x => x.Client)
                .Include(x => x.VoucherDoumentType)
                .AsQueryable();

            var customFilters = new Dictionary<int, Expression<Func<Sale, bool>>>
            {
                { 1, x => x.VoucherNumber!.Contains(filters.TextFilter!) },
            };

            var filteredQuery = _filterService.ApplyFilters(saleQueryable, filters, customFilters);
            filters.Sort ??= "Id";

            var items = await _orderingQuery
                .Ordering(filters, filteredQuery, !(bool)filters.Download!)
                .ToListAsync();

            response.IsSuccess = true;
            response.Data = _mapper.Map<IEnumerable<SaleResponseDto>>(items);
            response.TotalRecords = await filteredQuery.CountAsync();
            response.Message = ReplyMessage.MESSAGE_QUERY;

            return response;
        }
    }
}
