using AutoMapper;
using Microsoft.EntityFrameworkCore;
using POS.Application.Commons.Bases.Request;
using POS.Application.Commons.Bases.Response;
using POS.Application.Commons.Filters;
using POS.Application.Commons.Ordering;
using POS.Application.Dtos.Sale.Request;
using POS.Application.Dtos.Sale.Response;
using POS.Application.Interfaces;
using POS.Domain.Entities;
using POS.Infrastructure.Persistences.Interfaces;
using POS.Utilities.Static;
using System.Linq.Expressions;
using WatchDog;

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
                .Include(x => x.VoucherDocumentType)
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

        public async Task<BaseResponse<IEnumerable<ProductStockByWarehouseIdResponseDto>>> GetProductStockByWarehouse(BaseFiltersRequest filters)
        {
            var response = new BaseResponse<IEnumerable<ProductStockByWarehouseIdResponseDto>>();

            var products = _unitOfWork.SaleDetail.GetProductStockByWarehouse(filters.Id);

            var customFilters = new Dictionary<int, Expression<Func<Product, bool>>>
            {
                { 1, x => x.Name!.Contains(filters.TextFilter!) },
                { 2, x => x.Code!.Contains(filters.TextFilter!) },

            };

            var filteredQuery = _filterService.ApplyFilters(products, filters, customFilters);
            var sql = products.ToQueryString();
            Console.WriteLine(sql);

            filters.Sort ??= "Id";

            var items = await _orderingQuery
                .Ordering(filters, filteredQuery, !(bool)filters.Download!)
                .ToListAsync();


            //Revisar
            //if (!items.Any())
            //{
            //    response.IsSuccess = false;
            //    response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
            //    return response;
            //}

            response.IsSuccess = true;
            response.Data = _mapper.Map<IEnumerable<ProductStockByWarehouseIdResponseDto>>(items);
            response.TotalRecords = await filteredQuery.CountAsync();
            response.Message = ReplyMessage.MESSAGE_QUERY;

            return response;
        }

        public async Task<BaseResponse<SaleByIdResponseDto>> GetSaleById(int saleId)
        {
            var response = new BaseResponse<SaleByIdResponseDto>();

            var sale = await _unitOfWork.Sale.GetByIdAsync(saleId);

            if (sale is null)
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                return response;
            }

            var saleDetails = await _unitOfWork.SaleDetail
                .GetSaleDetailBySaleId(sale.Id);

            sale.SaleDetails = saleDetails.ToList();

            response.IsSuccess = true;
            response.Data = _mapper.Map<SaleByIdResponseDto>(sale);
            response.Message = ReplyMessage.MESSAGE_QUERY;
            return response;

        }

        public async Task<BaseResponse<bool>> CreateSale(SaleRequestDto requestDto)
        {
            var response = new BaseResponse<bool>();
            using var transaction = _unitOfWork.BeginTransaction();

            try
            {
                var sale = _mapper.Map<Sale>(requestDto);
                sale.State = (int)StateTypes.Active;
                await _unitOfWork.Sale.RegisterAsync(sale);

                var productsId = sale.SaleDetails.Select(x => x.ProductId).ToList();

                var productsStock = await _unitOfWork.ProductStock.GetProductStockByProduct(productsId, requestDto.WarehouseId);

                foreach (var item in sale.SaleDetails)
                {
                    var product = productsStock.FirstOrDefault(x => x.ProductId == item.ProductId);

                    if (product is null)
                    {
                        continue;
                    }

                    product.CurrentStock -= item.Quantity;
                }

                await _unitOfWork.ProductStock.UpdateCurrentStockByProducts(productsStock);

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

        public async Task<BaseResponse<bool>> CancelSale(int saleId)
        {
            var response = new BaseResponse<bool>();
            using var transaction = _unitOfWork.BeginTransaction();

            try
            {
                var sale = await GetSaleById(saleId);

                if (sale.Data is null)
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                    return response;
                }

                response.Data = await _unitOfWork.Sale.DeleteAsync(saleId);

                var productsId = sale.Data.SaleDetail.Select(x => x.ProductId).ToList();

                var productsStock = await _unitOfWork.ProductStock.GetProductStockByProduct(productsId, sale.Data.WarehouseId);

                foreach (var item in sale.Data.SaleDetail)
                {
                    var product = productsStock.FirstOrDefault(x => x.ProductId == item.ProductId);

                    if (product is null)
                    {
                        continue;
                    }

                    product.CurrentStock += item.Quantity;
                }

                await _unitOfWork.ProductStock.UpdateCurrentStockByProducts(productsStock);

                transaction.Commit();
                response.IsSuccess = true;
                response.Message = ReplyMessage.MESSAGE_SALE_CANCEL;
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


    }
}
