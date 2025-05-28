using AutoMapper;
using POS.Application.Commons.Select.Response;
using POS.Domain.Entities;

namespace POS.Application.Mappers;
public class VoucherDoumentTypeMappingsProfile : Profile
{
    public VoucherDoumentTypeMappingsProfile()
    {
        CreateMap<VoucherDocumentType, SelectResponse>()
             .ForMember(x => x.Description, x => x.MapFrom(y => y.Description))
             .ReverseMap();
    }
}
