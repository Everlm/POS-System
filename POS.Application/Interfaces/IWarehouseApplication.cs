using POS.Application.Commons.Bases.Request;
using POS.Application.Commons.Bases.Response;
using POS.Application.Commons.Select.Response;
using POS.Application.Dtos.Warehouse.Request;
using POS.Application.Dtos.Warehouse.Response;

namespace POS.Application.Interfaces
{
    public interface IWarehouseApplication
    {
        Task<BaseResponse<IEnumerable<WarehouseResponseDto>>> ListWarehouses(BaseFiltersRequest filters);
        Task<BaseResponse<IEnumerable<SelectResponse>>> GetSelectWarehouse();
        Task<BaseResponse<WarehouseByIdResponseDto>> GetWarehouseById(int warehouseId);
        Task<BaseResponse<bool>> RegisterWarehouse(WarehouseRequestDto requestDto);
        Task<BaseResponse<bool>> EditWarehouse(WarehouseRequestDto requestDto, int warehouseId);
        Task<BaseResponse<bool>> RemoveWarehouse(int warehouseId);
    }
}
