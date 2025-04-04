using AutoMapper;
using Microsoft.EntityFrameworkCore;
using POS.Application.Commons.Bases.Request;
using POS.Application.Commons.Bases.Response;
using POS.Application.Commons.Ordering;
using POS.Application.Dtos.Warehouse.Request;
using POS.Application.Dtos.Warehouse.Response;
using POS.Application.Interfaces;
using POS.Domain.Entities;
using POS.Infrastructure.Persistences.Interfaces;
using POS.Utilities.Static;
using WatchDog;

namespace POS.Application.Services
{
    public class WarehouseApplication : IWarehouseApplication
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IOrderingQuery _orderingQuery;

        public WarehouseApplication(IUnitOfWork unitOfWork, IMapper mapper, IOrderingQuery orderingQuery)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _orderingQuery = orderingQuery;
        }

        public async Task<BaseResponse<IEnumerable<WarehouseResponseDto>>> ListWarehouses(BaseFiltersRequest filters)
        {
            var response = new BaseResponse<IEnumerable<WarehouseResponseDto>>();

            try
            {
                var warehouseQueryable = _unitOfWork.Warehouse.GetAllQueryable();

                var warehouses = ApplyFilters(warehouseQueryable, filters);
                filters.Sort ??= "Id";

                var items = await _orderingQuery
                    .Ordering(filters, warehouses, !(bool)filters.Download!)
                    .ToListAsync();

                response.IsSuccess = true;
                response.Data = _mapper.Map<IEnumerable<WarehouseResponseDto>>(items);
                response.TotalRecords = await warehouses.CountAsync();
                response.Message = ReplyMessage.MESSAGE_QUERY;

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_EXCEPTION;
                WatchLogger.Log(ex.Message);
            }

            return response;

        }

        public async Task<BaseResponse<WarehouseByIdResponseDto>> GetWarehouseById(int WarehouseId)
        {
            var response = new BaseResponse<WarehouseByIdResponseDto>();

            try
            {
                var warehouse = await _unitOfWork.Warehouse.GetByIdAsync(WarehouseId);

                if (warehouse is null)
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                    return response;
                }

                response.IsSuccess = true;
                response.Data = _mapper.Map<WarehouseByIdResponseDto>(warehouse);
                response.Message = ReplyMessage.MESSAGE_QUERY;

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_EXCEPTION;
                WatchLogger.Log(ex.Message);
            }

            return response;
        }

        public async Task<BaseResponse<bool>> RegisterWarehouse(WarehouseRequestDto requestDto)
        {
            var response = new BaseResponse<bool>();
            using var transaction = _unitOfWork.BeginTransaction();

            try
            {
                var warehouse = _mapper.Map<Warehouse>(requestDto);
                response.Data = await _unitOfWork.Warehouse.RegisterAsync(warehouse);
                int warehouseId = warehouse.Id;

                await RegisterProductStockByWarehouse(warehouseId);

                transaction.Commit();

                response.IsSuccess = true;
                response.Message += ReplyMessage.MESSAGE_SAVE;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_EXCEPTION;
                WatchLogger.Log(ex.Message);
            }

            return response;
        }

        public async Task<BaseResponse<bool>> EditWarehouse(WarehouseRequestDto requestDto, int warehouseId)
        {
            var response = new BaseResponse<bool>();

            try
            {
                var warehouseEdit = await GetWarehouseById(warehouseId);

                if (warehouseEdit.Data is null)
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                    return response;
                }

                var warehouse = _mapper.Map<Warehouse>(requestDto);
                warehouse.Id = warehouseId;
                response.Data = await _unitOfWork.Warehouse.EditAsync(warehouse);

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
            }
            catch (Exception ex)
            {

                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_EXCEPTION;
                WatchLogger.Log(ex.Message);
            }

            return response;
        }

        public async Task<BaseResponse<bool>> RemoveWarehouse(int warehouseId)
        {
            var response = new BaseResponse<bool>();

            try
            {
                var warehouse = await GetWarehouseById(warehouseId);

                if (warehouse.Data is null)
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                    return response;
                }

                response.Data = await _unitOfWork.Warehouse.DeleteAsync(warehouseId);

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
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_EXCEPTION;
                WatchLogger.Log(ex.Message);
            }

            return response;
        }

        private async Task RegisterProductStockByWarehouse(int warehouseId)
        {
            //TODO: Validar cuando no hallan productos retornar un mensaje
            var products = await _unitOfWork.Product.GetAllAsync();

            foreach (var product in products)
            {
                var newProductStock = new ProductStock
                {
                    ProductId = product.Id,
                    WarehouseId = warehouseId,
                    CurrentStock = 0,
                    PurcharsePrice = 0
                };

                await _unitOfWork.ProductStock.RegisterProductStockAsync(newProductStock);
            }
        }

        private static IQueryable<Warehouse> ApplyFilters(IQueryable<Warehouse> query, BaseFiltersRequest filters)
        {
            if (filters.NumFilter is not null && !string.IsNullOrEmpty(filters.TextFilter))
            {
                query = filters.NumFilter switch
                {
                    1 => query.Where(x => x.Name!.Contains(filters.TextFilter)),
                    _ => query
                };
            }

            if (filters.StateFilter is not null)
            {
                query = query.Where(x => x.State == filters.StateFilter);
            }

            if (!string.IsNullOrEmpty(filters.StartDate) && !string.IsNullOrEmpty(filters.EndDate) &&
                DateTime.TryParse(filters.StartDate, out var startDate) && DateTime.TryParse(filters.EndDate, out var endDate))
            {
                query = query.Where(x => x.AuditCreateDate >= startDate && x.AuditCreateDate <= endDate.AddDays(1));
            }

            return query;
        }


    }
}
