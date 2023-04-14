using AutoMapper;
using POS.Application.Commons.Base;
using POS.Application.Dtos.BranchOffice.Request;
using POS.Application.Dtos.BranchOffice.Response;
using POS.Application.Dtos.Provider.Response;
using POS.Application.Interfaces;
using POS.Domain.Entities;
using POS.Infrastructure.Commons.Bases.Request;
using POS.Infrastructure.Commons.Bases.Response;
using POS.Infrastructure.Persistences.Interfaces;
using POS.Utilities.Static;
using WatchDog;

namespace POS.Application.Services
{
    public class BranchOfficeIdApplication : IBranchOfficeApplication
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BranchOfficeIdApplication(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<BaseResponse<BaseEntityResponse<BranchOfficeResponseDto>>> ListBranchOffices(BaseFiltersRequest filters)
        {
            var response = new BaseResponse<BaseEntityResponse<BranchOfficeResponseDto>>();

            try
            {
                var branchOffice = await _unitOfWork.BranchOffice.ListBranchOffices(filters);

                if (branchOffice is not null)
                {
                    response.IsSuccess = true;
                    response.Data = _mapper.Map<BaseEntityResponse<BranchOfficeResponseDto>>(branchOffice);
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

        public async Task<BaseResponse<BranchOfficeResponseDto>> GetBranchOfficeById(int BranchOfficeId)
        {
            var response = new BaseResponse<BranchOfficeResponseDto>();

            try
            {
                var branchOffice = await _unitOfWork.BranchOffice.GetByIdAsync(BranchOfficeId);

                if (branchOffice is not null)
                {
                    response.IsSuccess = true;
                    response.Data = _mapper.Map<BranchOfficeResponseDto>(branchOffice);
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

        public async Task<BaseResponse<bool>> RegisterBranchOffice(BranchOfficeRequestDto requestDto)
        {
            var response = new BaseResponse<bool>();

            try
            {
                var branchOffice = _mapper.Map<BranchOffice>(requestDto);

                response.Data = await _unitOfWork.BranchOffice.RegisterAsync(branchOffice);

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

        public async Task<BaseResponse<bool>> EditBranchOffice(BranchOfficeRequestDto requestDto, int BranchOfficeId)
        {
            var response = new BaseResponse<bool>();

            try
            {
                var branchOfficeEdit = await GetBranchOfficeById(BranchOfficeId);

                if (branchOfficeEdit.Data is null)
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                    return response;
                }

                var branchOffice = _mapper.Map<BranchOffice>(requestDto);
                branchOffice.Id = BranchOfficeId;
                response.Data = await _unitOfWork.BranchOffice.EditAsync(branchOffice);

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

        public async Task<BaseResponse<bool>> DeleteBranchOffice(int BranchOfficeId)
        {
            var response = new BaseResponse<bool>();

            try
            {
                var branchOffice = await GetBranchOfficeById(BranchOfficeId);

                if (branchOffice.Data is null)
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                    return response;
                }

                response.Data = await _unitOfWork.BranchOffice.DeleteAsync(BranchOfficeId);

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
