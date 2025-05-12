using AutoMapper;
using Microsoft.EntityFrameworkCore;
using POS.Application.Commons.Bases.Request;
using POS.Application.Commons.Bases.Response;
using POS.Application.Commons.Ordering;
using POS.Application.Dtos.Product.Response;
using POS.Application.Dtos.Purcharse.Request;
using POS.Application.Dtos.Purchase.Response;
using POS.Application.Interfaces;
using POS.Domain.Entities;
using POS.Infrastructure.Persistences.Interfaces;
using POS.Utilities.Static;
using WatchDog;

namespace POS.Application.Services
{
    public class PurcharseApplication : IPurcharseApplication
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderingQuery _orderingQuery;

        public PurcharseApplication(IMapper mapper, IUnitOfWork unitOfWork, IOrderingQuery orderingQuery)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _orderingQuery = orderingQuery;
        }

        public async Task<BaseResponse<IEnumerable<PurcharseResponseDto>>> ListPurcharses(BaseFiltersRequest filters)
        {
            var response = new BaseResponse<IEnumerable<PurcharseResponseDto>>();

            var purcharseQueryable = _unitOfWork.Purcharse
                    .GetAllQueryable()
                    .Include(x => x.Provider)
                    .Include(x => x.Warehouse)
                    .AsQueryable();

            var purcharsesFiltered = ApplyFilters(purcharseQueryable, filters);
            filters.Sort ??= "Id";

            var sql = purcharsesFiltered.ToQueryString();
            Console.WriteLine(sql);

            var items = await _orderingQuery
                .Ordering(filters, purcharsesFiltered, !(bool)filters.Download!)
                .ToListAsync();

            response.IsSuccess = true;
            response.Data = _mapper.Map<IEnumerable<PurcharseResponseDto>>(items);
            response.TotalRecords = await purcharsesFiltered.CountAsync();
            response.Message = ReplyMessage.MESSAGE_QUERY;
            return response;
        }

        public async Task<BaseResponse<PurcharseByIdResponseDto>> PurcharseById(int purcharseId)
        {
            var response = new BaseResponse<PurcharseByIdResponseDto>();

            try
            {
                var purcharse = await _unitOfWork.Purcharse.GetByIdAsync(purcharseId);

                if (purcharse is null)
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                    return response;
                }

                var purcharseDetails = await _unitOfWork.PurcharseDetail
                    .GetPurcharseDetailByPurcharseId(purcharse.Id);

                purcharse.PurcharseDetails = purcharseDetails.ToList();

                response.IsSuccess = true;
                response.Data = _mapper.Map<PurcharseByIdResponseDto>(purcharse);
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

        public async Task<BaseResponse<bool>> CreatePurcharse(PurcharseRequestDto requestDto)
        {
            var response = new BaseResponse<bool>();
            using var transaction = _unitOfWork.BeginTransaction();

            try
            {
                var purcharse = _mapper.Map<Purcharse>(requestDto);
                purcharse.State = (int)StateTypes.Active;
                await _unitOfWork.Purcharse.RegisterAsync(purcharse);

                var productsId = purcharse.PurcharseDetails.Select(x => x.ProductId).ToList();

                var products = await _unitOfWork.ProductStock.GetProductStockByProduct(productsId, requestDto.WarehouseId);

                foreach (var item in purcharse.PurcharseDetails)
                {
                    var product = products.FirstOrDefault(x => x.ProductId == item.ProductId);

                    if (product is null)
                    {
                        continue;
                    }

                    product.CurrentStock += item.Quantity;
                    product.PurcharsePrice = item.UnitPurcharsePrice;
                }

                await _unitOfWork.ProductStock.UpdateCurrentStockByProducts(products);

                //foreach (var item in purcharse.PurcharseDetails)
                //{
                //    var productStock = await _unitOfWork.ProductStock
                //        .GetProductStockByProduct(item.ProductId, requestDto.WarehouseId);

                //    productStock.CurrentStock += item.Quantity;
                //    productStock.PurcharsePrice = item.UnitPurcharsePrice;
                //    await _unitOfWork.ProductStock.UpdateCurrentStockByProducts(productStock);
                //}

                transaction.Commit();
                response.IsSuccess = true;
                response.Message = ReplyMessage.MESSAGE_SAVE;
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

        public async Task<BaseResponse<bool>> CancelPurcharse(int purcharseId)
        {
            var response = new BaseResponse<bool>();
            using var transaction = _unitOfWork.BeginTransaction();

            try
            {
                var purcharse = await PurcharseById(purcharseId);

                if (purcharse.Data is null)
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                    return response;
                }

                response.Data = await _unitOfWork.Purcharse.DeleteAsync(purcharseId);

                var productsId = purcharse.Data.PurcharseDetailsById.Select(x => x.ProductId).ToList();

                var products = await _unitOfWork.ProductStock.GetProductStockByProduct(productsId, purcharse.Data.WarehouseId);

                foreach (var item in purcharse.Data.PurcharseDetailsById)
                {
                    var product = products.FirstOrDefault(x => x.ProductId == item.ProductId);

                    if (product is null)
                    {
                        continue;
                    }

                    product.CurrentStock -= item.Quantity;
                }

                await _unitOfWork.ProductStock.UpdateCurrentStockByProducts(products);

                transaction.Commit();
                response.IsSuccess = true;
                response.Message = ReplyMessage.MESSAGE_DELETE;

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

        private static IQueryable<Purcharse> ApplyFilters(IQueryable<Purcharse> query, BaseFiltersRequest filters)
        {
            if (filters.NumFilter is not null && !string.IsNullOrEmpty(filters.TextFilter))
            {
                query = filters.NumFilter switch
                {
                    1 => query.Where(x => x.Provider.Name!.Contains(filters.TextFilter)),
                    _ => query
                };
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
