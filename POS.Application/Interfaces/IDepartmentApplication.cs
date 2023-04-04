using POS.Application.Commons.Base;
using POS.Application.Dtos.Department.Request;
using POS.Application.Dtos.Department.Response;
using POS.Infrastructure.Commons.Bases.Request;
using POS.Infrastructure.Commons.Bases.Response;

namespace POS.Application.Interfaces
{
    public interface IDepartmentApplication
    {
        Task<BaseResponse<BaseEntityResponse<DeparmentReponseDto>>> ListDepartment(BaseFiltersRequest filters);
        Task<BaseResponse<DeparmentReponseDto>> GetDepartmentById(int departmentId);
        Task<BaseResponse<bool>> RegisterDepartment(DeparmentRequestDto requestDto);
        Task<BaseResponse<bool>> EditDepartment(DeparmentRequestDto requestDto, int departmentId);
        Task<BaseResponse<bool>> DeleteDepartment(int departmentId);
    }
}
