using POS.Application.Commons.Bases.Response;
using POS.Application.Commons.Select.Response;

namespace POS.Application.Interfaces;
public interface IVoucherDoumentTypeApplication
{
    Task<BaseResponse<IEnumerable<SelectResponse>>> GetAllVoucherDoumentType();
}
