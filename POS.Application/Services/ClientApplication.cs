using AutoMapper;
using Microsoft.EntityFrameworkCore;
using POS.Application.Commons.Bases.Request;
using POS.Application.Commons.Bases.Response;
using POS.Application.Commons.Filters;
using POS.Application.Commons.Ordering;
using POS.Application.Dtos.Client.Response;
using POS.Application.Interfaces;
using POS.Domain.Entities;
using POS.Infrastructure.Persistences.Interfaces;
using POS.Utilities.Static;
using System.Linq.Expressions;


namespace POS.Application.Services
{
    public class ClientApplication : IClientApplication
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IOrderingQuery _orderingQuery;
        private readonly IFilterService _filterService;

        public ClientApplication(IUnitOfWork unitOfWork, IMapper mapper, IOrderingQuery orderingQuery, IFilterService filterService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _orderingQuery = orderingQuery;
            _filterService = filterService;
        }

        public async Task<BaseResponse<IEnumerable<ClientResponseDto>>> ListClient(BaseFiltersRequest filters)
        {
            var response = new BaseResponse<IEnumerable<ClientResponseDto>>();

            var clientsQueryable = _unitOfWork.Client
                .GetAllQueryable()
                .Include(x => x.DocumentType)
                .AsQueryable();

            var customFilters = new Dictionary<int, Expression<Func<Client, bool>>>
            {
                { 1, x => x.Name!.Contains(filters.TextFilter!) },
                { 2, x => x.Email!.Contains(filters.TextFilter!) },
                { 3, x => x.DocumentNumber!.Contains(filters.TextFilter!) }
            };

            var filteredQuery = _filterService.ApplyFilters(clientsQueryable, filters, customFilters);
            filters.Sort ??= "Id";

            var items = await _orderingQuery
                .Ordering(filters, filteredQuery, !(bool)filters.Download!)
                .ToListAsync();

            response.IsSuccess = true;
            response.Data = _mapper.Map<IEnumerable<ClientResponseDto>>(items);
            response.TotalRecords = await filteredQuery.CountAsync();
            response.Message = ReplyMessage.MESSAGE_QUERY;

            return response;
        }
    }
}
