using POS.Application.Commons.Base;
using POS.Application.Dtos.BranchOffice.Request;
using POS.Application.Dtos.BranchOffice.Response;
using POS.Infrastructure.Commons.Bases.Request;
using POS.Infrastructure.Commons.Bases.Response;

namespace POS.Application.Interfaces
{
    public interface IBranchOfficeApplication
    {
        Task<BaseResponse<BaseEntityResponse<BranchOfficeResponseDto>>> ListBranchOffices(BaseFiltersRequest filters);
        Task<BaseResponse<BranchOfficeResponseDto>> GetBranchOfficeById(int BranchOfficeId);
        Task<BaseResponse<bool>> RegisterBranchOffice(BranchOfficeRequestDto requestDto);
        Task<BaseResponse<bool>> EditBranchOffice(BranchOfficeRequestDto requestDto, int BranchOfficeId);
        Task<BaseResponse<bool>> DeleteBranchOffice(int BranchOfficeId);
    }
}
