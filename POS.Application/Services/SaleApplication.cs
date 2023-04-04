using AutoMapper;
using POS.Application.Commons.Base;
using POS.Application.Dtos.Sale.Request;
using POS.Application.Dtos.Sale.Response;
using POS.Application.Interfaces;
using POS.Domain.Entities;
using POS.Infrastructure.Commons.Bases.Request;
using POS.Infrastructure.Commons.Bases.Response;
using POS.Infrastructure.Persistences.Interfaces;
using POS.Utilities.Static;
using WatchDog;

namespace POS.Application.Services
{
    public class SaleApplication : ISaleApplication
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SaleApplication(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BaseResponse<BaseEntityResponse<SaleResponseDto>>> ListSales(BaseFiltersRequest filters)
        {
            var response = new BaseResponse<BaseEntityResponse<SaleResponseDto>>();

            try
            {
                var providers = await _unitOfWork.Sale.ListSales(filters);

                if (providers is not null)
                {
                    response.IsSuccess = true;
                    response.Data = _mapper.Map<BaseEntityResponse<SaleResponseDto>>(providers);
                    response.Message = ReplyMessage.MESSAGE_QUERY;
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
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
        public async Task<BaseResponse<SaleResponseDto>> GetSaleById(int SaleId)
        {
            var response = new BaseResponse<SaleResponseDto>();

            try
            {
                var sale = await _unitOfWork.Sale.GetByIdAsync(SaleId);

                if (sale is not null)
                {
                    response.IsSuccess = true;
                    response.Data = _mapper.Map<SaleResponseDto>(sale);
                    response.Message = ReplyMessage.MESSAGE_QUERY;

                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
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
        public async Task<BaseResponse<bool>> RegisterSale(SaleRequestDto requestDto)
        {
            var response = new BaseResponse<bool>();

            try
            {
                var sale = _mapper.Map<Sale>(requestDto);

                response.Data = await _unitOfWork.Sale.RegisterSale(sale);

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
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                WatchLogger.Log(ex.Message);
            }

            return response;
        }
    }
}
