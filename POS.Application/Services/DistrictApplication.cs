using AutoMapper;
using POS.Application.Commons.Base;
using POS.Application.Dtos.District.Request;
using POS.Application.Dtos.District.Response;
using POS.Application.Dtos.Province.Response;
using POS.Application.Interfaces;
using POS.Domain.Entities;
using POS.Infrastructure.Commons.Bases.Request;
using POS.Infrastructure.Commons.Bases.Response;
using POS.Infrastructure.Persistences.Interfaces;
using POS.Utilities.Static;
using WatchDog;

namespace POS.Application.Services
{
    public class DistrictApplication : IDistrictApplication
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DistrictApplication(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BaseResponse<BaseEntityResponse<DistrictResponseDto>>> ListDistricts(BaseFiltersRequest filters)
        {
            var response = new BaseResponse<BaseEntityResponse<DistrictResponseDto>>();

            try
            {
                var districts = await _unitOfWork.District.ListDistrics(filters);

                if (districts is not null)
                {
                    response.IsSuccess = true;
                    response.Data = _mapper.Map<BaseEntityResponse<DistrictResponseDto>>(districts);
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

        public async Task<BaseResponse<DistrictResponseDto>> GetDistrictById(int districtId)
        {
            var response = new BaseResponse<DistrictResponseDto>();

            try
            {
                var district = await _unitOfWork.District.GetByIdAsync(districtId);

                if (district is not null)
                {
                    response.IsSuccess = true;
                    response.Data = _mapper.Map<DistrictResponseDto>(district);
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

        public async Task<BaseResponse<bool>> RegisterDistrict(DistrictRequestDto requestDto)
        {
            var response = new BaseResponse<bool>();

            try
            {
                var district = _mapper.Map<District>(requestDto);

                response.Data = await _unitOfWork.District.RegisterAsync(district);

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

        public async Task<BaseResponse<bool>> EditDistrict(DistrictRequestDto requestDto, int districtId)
        {
            var response = new BaseResponse<bool>();

            try
            {
                var districtEdit = await GetDistrictById(districtId);

                if (districtEdit.Data is null)
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                    return response;
                }

                var district = _mapper.Map<District>(requestDto);
                district.Id = districtId;
                response.Data = await _unitOfWork.District.EditAsync(district);

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
        public async Task<BaseResponse<bool>> DeleteDistrict(int districtId)
        {
            var response = new BaseResponse<bool>();

            try
            {
                var district = await GetDistrictById(districtId);

                if (district.Data is null)
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                    return response;
                }

                response.Data = await _unitOfWork.District.DeleteAsync(districtId);

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
