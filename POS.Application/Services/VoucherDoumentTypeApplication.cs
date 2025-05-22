using AutoMapper;
using POS.Application.Commons.Bases.Response;
using POS.Application.Commons.Select.Response;
using POS.Application.Interfaces;
using POS.Infrastructure.Persistences.Interfaces;
using POS.Utilities.Static;

namespace POS.Application.Services;
public class VoucherDoumentTypeApplication : IVoucherDoumentTypeApplication
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public VoucherDoumentTypeApplication(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<BaseResponse<IEnumerable<SelectResponse>>> GetAllVoucherDoumentType()
    {
        var response = new BaseResponse<IEnumerable<SelectResponse>>();
        
        //Revisar cuando metodo es el correcto GetAllAsync o GetSelectAsync
        var documentTypes = await _unitOfWork.VoucherDoumentType.GetSelectAsync();

        if (documentTypes is null)
        {
            response.IsSuccess = false;
            response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
            return response;
        }

        response.IsSuccess = true;
        response.Data = _mapper.Map<IEnumerable<SelectResponse>>(documentTypes);
        response.Message = ReplyMessage.MESSAGE_QUERY;
        return response;
    }
}
