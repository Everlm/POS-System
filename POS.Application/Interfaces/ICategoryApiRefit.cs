using POS.Application.Commons.Bases.Response;
using POS.Application.Commons.Select.Response;
using Refit;

namespace POS.Application.Interfaces;

public interface ICategoryApiRefit
{
    [Get("/api/Category/Select")]
    Task<BaseResponse<IEnumerable<SelectResponse>>> GetCategoriesAsync();
}
