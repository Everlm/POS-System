using AutoMapper;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.EntityFrameworkCore;
using POS.Application.Commons.Bases.Response;
using POS.Application.Dtos.Product.Response;
using POS.Application.Dtos.ProductStock;
using POS.Application.Interfaces;
using POS.Domain.Entities;
using POS.Infrastructure.Persistences.Interfaces;
using POS.Utilities.Static;
using WatchDog;

namespace POS.Application.Services
{
    public class ProductStockApplication : IProductStockApplication
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ProductStockApplication(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse<IEnumerable<ProductStockByWarehouseDto>>> GetProductStockByWarehouseAsync(int productId)
        {
            var response = new BaseResponse<IEnumerable<ProductStockByWarehouseDto>>();

            try
            {
                var productStockByWarehouse = await _unitOfWork.ProductStock.GetProductStockByWarehouse(productId);

                if(!productStockByWarehouse.Any())
                {
                    response.IsSuccess = true;
                    response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                    return response;
                }

                response.IsSuccess = true;
                response.Data = _mapper.Map<IEnumerable<ProductStockByWarehouseDto>>(productStockByWarehouse);
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
    }
}
