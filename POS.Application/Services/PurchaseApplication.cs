using AutoMapper;
using POS.Application.Commons.Base;
using POS.Application.Dtos.Purchase.Request;
using POS.Application.Dtos.Purchase.Response;
using POS.Application.Interfaces;
using POS.Domain.Entities;
using POS.Infrastructure.Commons.Bases.Request;
using POS.Infrastructure.Commons.Bases.Response;
using POS.Infrastructure.Persistences.Interfaces;
using POS.Utilities.Static;
using WatchDog;

namespace POS.Application.Services
{
    public class PurchaseApplication : IPurchaseApplication
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PurchaseApplication(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<BaseResponse<BaseEntityResponse<PurchaseResponseDto>>> ListPurchase(BaseFiltersRequest filters)
        {
            var response = new BaseResponse<BaseEntityResponse<PurchaseResponseDto>>();

            try
            {
                var purchases = await _unitOfWork.Purcharse.ListPurchases(filters);

                if (purchases is not null)
                {
                    response.IsSuccess = true;
                    response.Data = _mapper.Map<BaseEntityResponse<PurchaseResponseDto>>(purchases);
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

        public async Task<BaseResponse<PurchaseResponseDto>> GetPurchaseById(int PurchaseId)
        {
            var response = new BaseResponse<PurchaseResponseDto>();

            try
            {
                var purchase = await _unitOfWork.Purcharse.GetByIdAsync(PurchaseId);

                if (purchase is not null)
                {
                    response.IsSuccess = true;
                    response.Data = _mapper.Map<PurchaseResponseDto>(purchase);
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

        public async Task<BaseResponse<bool>> RegisterPurchase(PurchaseRequestDto requestDto)
        {
            var response = new BaseResponse<bool>();

            try
            {
                var purchase = _mapper.Map<Purcharse>(requestDto);

                response.Data = await _unitOfWork.Purcharse.RegisterPurchase(purchase);

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
