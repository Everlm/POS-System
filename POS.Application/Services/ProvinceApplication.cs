using AutoMapper;
using POS.Application.Commons.Base;
using POS.Application.Dtos.Province.Request;
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
    public class ProvinceApplication : IProvinceApplication
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProvinceApplication(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BaseResponse<BaseEntityResponse<ProvinceResponseDto>>> ListProvinces(BaseFiltersRequest filters)
        {
            var response = new BaseResponse<BaseEntityResponse<ProvinceResponseDto>>();

            try
            {
                var provinces = await _unitOfWork.Province.ListProvinces(filters);

                if (provinces is not null)
                {
                    response.IsSuccess = true;
                    response.Data = _mapper.Map<BaseEntityResponse<ProvinceResponseDto>>(provinces);
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

        public async Task<BaseResponse<ProvinceResponseDto>> GetProvinceById(int provinceId)
        {
            var response = new BaseResponse<ProvinceResponseDto>();

            try
            {
                var province = await _unitOfWork.Province.GetByIdAsync(provinceId);

                if (province is not null)
                {
                    response.IsSuccess = true;
                    response.Data = _mapper.Map<ProvinceResponseDto>(province);
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

        public async Task<BaseResponse<bool>> Registerprovince(ProvinceRequestDto requestDto)
        {
            var response = new BaseResponse<bool>();

            try
            {
                var province = _mapper.Map<Province>(requestDto);

                response.Data = await _unitOfWork.Province.RegisterAsync(province);

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

        public async Task<BaseResponse<bool>> EditProvince(ProvinceRequestDto requestDto, int provinceId)
        {
            var response = new BaseResponse<bool>();

            try
            {
                var provinceEdit = await GetProvinceById(provinceId);

                if (provinceEdit.Data is null)
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                    return response;
                }

                var province = _mapper.Map<Province>(requestDto);
                province.Id = provinceId;
                response.Data = await _unitOfWork.Province.EditAsync(province);

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

        public async Task<BaseResponse<bool>> DeleteProvince(int provinceId)
        {
            var response = new BaseResponse<bool>();

            try
            {
                var province = await GetProvinceById(provinceId);

                if (province.Data is null)
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                    return response;
                }

                response.Data = await _unitOfWork.Province.DeleteAsync(provinceId);

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
