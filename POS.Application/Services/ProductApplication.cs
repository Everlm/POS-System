using AutoMapper;
using Microsoft.EntityFrameworkCore;
using POS.Application.Commons.Bases.Request;
using POS.Application.Commons.Bases.Response;
using POS.Application.Commons.Ordering;
using POS.Application.Dtos.Product.Request;
using POS.Application.Dtos.Product.Response;
using POS.Application.Interfaces;
using POS.Domain.Entities;
using POS.Infrastructure.Persistences.Interfaces;
using POS.Utilities.Static;
using WatchDog;

namespace POS.Application.Services
{
    public class ProductApplication : IProductApplication
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IOrderingQuery _orderingQuery;
        private readonly IFileLocalStorageApplication _fileLocalStorageApplication;

        public ProductApplication(IUnitOfWork unitOfWork, IMapper mapper, IOrderingQuery orderingQuery,
            IFileLocalStorageApplication fileLocalStorageApplication)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _orderingQuery = orderingQuery;
            _fileLocalStorageApplication = fileLocalStorageApplication;
        }

        public async Task<BaseResponse<IEnumerable<ProductResponseDto>>> ListProducts(BaseFiltersRequest filters)
        {
            var response = new BaseResponse<IEnumerable<ProductResponseDto>>();

            try
            {
                var productQueryable = _unitOfWork.Product
                    .GetAllQueryable()
                    .Include(x => x.Category)
                    .AsQueryable();

                var products = ApplyFilters(productQueryable, filters);
                filters.Sort ??= "Id";

                var items = await _orderingQuery
                    .Ordering(filters, products, !(bool)filters.Download!)
                    .ToListAsync();

                response.IsSuccess = true;
                response.Data = _mapper.Map<IEnumerable<ProductResponseDto>>(items);
                response.TotalRecords = await products.CountAsync();
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

        public async Task<BaseResponse<ProductByIdResponseDto>> GetProductById(int productId)
        {
            var response = new BaseResponse<ProductByIdResponseDto>();

            try
            {
                var product = await _unitOfWork.Product.GetByIdAsync(productId);

                if (product is null)
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                    return response;
                }

                response.IsSuccess = true;
                response.Data = _mapper.Map<ProductByIdResponseDto>(product);
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

        public async Task<BaseResponse<bool>> CreateProduct(ProductRequestDto requestDto)
        {
            var response = new BaseResponse<bool>();
            using var transaction = _unitOfWork.BeginTransaction();

            try
            {
                var product = _mapper.Map<Product>(requestDto);

                if (requestDto.Image is not null)
                {
                    product.Image = await _fileLocalStorageApplication.SaveFileAsync(requestDto.Image, LocalContainers.PRODUCTS);
                }

                response.Data = await _unitOfWork.Product.RegisterAsync(product);
                int productId = product.Id;

                await RegisterProductStockByProduct(productId);

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

        public async Task<BaseResponse<bool>> UpdateProduct(ProductRequestDto requestDto, int productId)
        {
            var response = new BaseResponse<bool>();

            try
            {
                var productUpdate = await GetProductById(productId);

                if (productUpdate.Data is null)
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                    return response;
                }

                var product = _mapper.Map<Product>(requestDto);

                if (requestDto.Image is not null)
                {
                    product.Image = await _fileLocalStorageApplication
                        .UpdateFileAsync(requestDto.Image, LocalContainers.PRODUCTS, productUpdate.Data.Image!);
                }

                if (requestDto.Image is null)
                {
                    product.Image = productUpdate.Data.Image;
                }

                product.Id = productId;
                response.Data = await _unitOfWork.Product.EditAsync(product);

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

        public async Task<BaseResponse<bool>> DeleteProduct(int productId)
        {
            var response = new BaseResponse<bool>();

            try
            {
                var productToUpdate = await GetProductById(productId);

                if (productToUpdate.Data is null)
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                    return response;
                }

                response.Data = await _unitOfWork.Product.DeleteAsync(productId);

                await _fileLocalStorageApplication.DeleteFileAsync(LocalContainers.PRODUCTS, productToUpdate.Data.Image!);

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

        private async Task RegisterProductStockByProduct(int productId)
        {
            //TODO: Validar cuando no hallan bodegas retornar un mensaje
            var warehouses = await _unitOfWork.Warehouse.GetAllAsync();

            foreach (var warehouse in warehouses)
            {
                var newProductStock = new ProductStock
                {
                    ProductId = productId,
                    WarehouseId = warehouse.Id,
                    CurrentStock = 0,
                    PurcharsePrice = 0
                };

                await _unitOfWork.ProductStock.RegisterProductStockAsync(newProductStock);
            }
        }

        private static IQueryable<Product> ApplyFilters(IQueryable<Product> query, BaseFiltersRequest filters)
        {
            if (filters.NumFilter is not null && !string.IsNullOrEmpty(filters.TextFilter))
            {
                query = filters.NumFilter switch
                {
                    1 => query.Where(x => x.Name!.Contains(filters.TextFilter)),
                    2 => query.Where(x => x.Code!.Contains(filters.TextFilter)),
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
