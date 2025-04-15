using AutoMapper;
using Microsoft.EntityFrameworkCore;
using POS.Application.Commons.Bases.Request;
using POS.Application.Commons.Bases.Response;
using POS.Application.Commons.Filters;
using POS.Application.Commons.Ordering;
using POS.Application.Commons.Select.Response;
using POS.Application.Dtos.Category.Response;
using POS.Application.Dtos.Client.Request;
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

        public async Task<BaseResponse<IEnumerable<SelectResponse>>> ListSelectClient()
        {
            var response = new BaseResponse<IEnumerable<SelectResponse>>();
            var clients = await _unitOfWork.Client.GetAllAsync();

            if (clients is not null)
            {
                response.IsSuccess = true;
                response.Data = _mapper.Map<IEnumerable<SelectResponse>>(clients);
                response.TotalRecords = clients.Count();
                response.Message = ReplyMessage.MESSAGE_QUERY;

            }
            else
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
            }

            return response;
        }

        public async Task<BaseResponse<ClientResponseDto>> GetClientById(int clientId)
        {
            var response = new BaseResponse<ClientResponseDto>();

            var client = await _unitOfWork.Client.GetByIdAsync(clientId);

            if (client is null)
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                return response;
            }

            response.IsSuccess = true;
            response.Data = _mapper.Map<ClientResponseDto>(client);
            response.Message = ReplyMessage.MESSAGE_QUERY;
            return response;
        }

        public async Task<BaseResponse<bool>> CreateClient(ClientRequestDto requestDto)
        {
            var response = new BaseResponse<bool>();

            var client = _mapper.Map<Client>(requestDto);
            response.Data = await _unitOfWork.Client.RegisterAsync(client);

            if (response.Data)
            {
                response.IsSuccess = true;
                response.Message += ReplyMessage.MESSAGE_SAVE;
            }
            else
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_FAILED;
            }

            return response;
        }

        public async Task<BaseResponse<bool>> UpdateClient(ClientRequestDto requestDto, int clientId)
        {
            var response = new BaseResponse<bool>();

            var clientToUpdate = await GetClientById(clientId);

            if (clientToUpdate.Data is null)
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                return response;
            }

            var client = _mapper.Map<Client>(requestDto);
            client.Id = clientId;
            response.Data = await _unitOfWork.Client.EditAsync(client);

            if (response.Data)
            {
                response.IsSuccess = true;
                response.Message = ReplyMessage.MESSAGE_UPDATE;
            }
            else
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_FAILED;
            }

            return response;
        }

        public async Task<BaseResponse<bool>> DeleteClient(int clientId)
        {
            var response = new BaseResponse<bool>();

            var clientTodelete = await GetClientById(clientId);

            if (clientTodelete.Data is null)
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                return response;
            }

            response.Data = await _unitOfWork.Client.DeleteAsync(clientId);

            if (response.Data)
            {
                response.IsSuccess = true;
                response.Message = ReplyMessage.MESSAGE_DELETE;
            }
            else
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_FAILED;
            }

            return response;
        }

    }
}
