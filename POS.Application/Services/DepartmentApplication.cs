using AutoMapper;
using POS.Application.Commons.Base;
using POS.Application.Dtos.Category.Response;
using POS.Application.Dtos.Department.Request;
using POS.Application.Dtos.Department.Response;
using POS.Application.Interfaces;
using POS.Domain.Entities;
using POS.Infrastructure.Commons.Bases.Request;
using POS.Infrastructure.Commons.Bases.Response;
using POS.Infrastructure.Persistences.Interfaces;
using POS.Utilities.Static;
using WatchDog;

namespace POS.Application.Services
{
    public class DepartmentApplication : IDepartmentApplication
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DepartmentApplication(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BaseResponse<BaseEntityResponse<DeparmentReponseDto>>> ListDepartment(BaseFiltersRequest filters)
        {
            var response = new BaseResponse<BaseEntityResponse<DeparmentReponseDto>>();

            try
            {
                var departatments = await _unitOfWork.Department.ListDepartments(filters);

                if (departatments is not null)
                {
                    response.IsSuccess = true;
                    response.Data = _mapper.Map<BaseEntityResponse<DeparmentReponseDto>>(departatments);
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

        public async Task<BaseResponse<DeparmentReponseDto>> GetDepartmentById(int departmentId)
        {
            var response = new BaseResponse<DeparmentReponseDto>();

            try
            {
                var departatment = await _unitOfWork.Department.GetByIdAsync(departmentId);

                if (departatment is not null)
                {
                    response.IsSuccess = true;
                    response.Data = _mapper.Map<DeparmentReponseDto>(departatment);
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

        public async Task<BaseResponse<bool>> RegisterDepartment(DeparmentRequestDto requestDto)
        {
            var response = new BaseResponse<bool>();

            try
            {
                var department = _mapper.Map<Department>(requestDto);

                response.Data = await _unitOfWork.Department.RegisterAsync(department);

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

        public async Task<BaseResponse<bool>> EditDepartment(DeparmentRequestDto requestDto, int departmentId)
        {
            var response = new BaseResponse<bool>();

            try
            {
                var departmentEdit = await GetDepartmentById(departmentId);

                if (departmentEdit.Data is null)
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                    return response;
                }

                var department = _mapper.Map<Department>(requestDto);
                department.Id = departmentId;
                response.Data = await _unitOfWork.Department.EditAsync(department);

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
        public async Task<BaseResponse<bool>> DeleteDepartment(int departmentId)
        {
            var response = new BaseResponse<bool>();

            try
            {
                var department = await GetDepartmentById(departmentId);

                if (department.Data is null)
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                }

                response.Data = await _unitOfWork.Department.DeleteAsync(departmentId);

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
