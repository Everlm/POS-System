using AutoMapper;
using POS.Application.Commons.Base;
using POS.Application.Dtos.Business.Request;
using POS.Application.Dtos.Business.Response;
using POS.Application.Interfaces;
using POS.Domain.Entities;
using POS.Infrastructure.Commons.Bases.Request;
using POS.Infrastructure.Commons.Bases.Response;
using POS.Infrastructure.Persistences.Interfaces;
using POS.Utilities.Static;
using WatchDog;

namespace POS.Application.Services
{
    public class BusinessApplication : IBusinessApplication
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BusinessApplication(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<BaseResponse<BaseEntityResponse<BusinessResponseDto>>> ListBusiness(BaseFiltersRequest filters)
        {
            var response = new BaseResponse<BaseEntityResponse<BusinessResponseDto>>();

            try
            {
                var business = await _unitOfWork.Business.ListBusiness(filters);

                if (business is not null)
                {
                    response.IsSuccess = true;
                    response.Data = _mapper.Map<BaseEntityResponse<BusinessResponseDto>>(business);
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
        public async Task<BaseResponse<BusinessResponseDto>> GetBusinessById(int businessId)
        {
            var response = new BaseResponse<BusinessResponseDto>();

            try
            {
                var business = await _unitOfWork.Business.GetByIdAsync(businessId);

                if (business is not null)
                {
                    response.IsSuccess = true;
                    response.Data = _mapper.Map<BusinessResponseDto>(business);
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

        public async Task<BaseResponse<bool>> RegisterBusiness(BusinessRequestDto requestDto)
        {
            var response = new BaseResponse<bool>();

            try
            {
                var business = _mapper.Map<Business>(requestDto);

                response.Data = await _unitOfWork.Business.RegisterAsync(business);

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
                response.Message = ReplyMessage.MESSAGE_EXCEPTION;
                WatchLogger.Log(ex.Message);
            }

            return response;
        }
        public async Task<BaseResponse<bool>> EditBusiness(BusinessRequestDto requestDto, int businessId)
        {
            var response = new BaseResponse<bool>();

            try
            {
                var businessEdit = await GetBusinessById(businessId);

                if (businessEdit.Data is null)
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                    return response;
                }

                var business = _mapper.Map<Business>(requestDto);
                business.Id = businessId;
                response.Data = await _unitOfWork.Business.EditAsync(business);

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

        public async Task<BaseResponse<bool>> DeleteBusiness(int businessId)
        {
            var response = new BaseResponse<bool>();

            try
            {
                var business = await GetBusinessById(businessId);

                if (business.Data is null)
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                }

                response.Data = await _unitOfWork.Business.DeleteAsync(businessId);

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
    }
}
