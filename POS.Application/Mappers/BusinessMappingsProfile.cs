using AutoMapper;
using POS.Application.Dtos.Business.Request;
using POS.Application.Dtos.Business.Response;
using POS.Domain.Entities;
using POS.Infrastructure.Commons.Bases.Response;
using POS.Utilities.Static;

namespace POS.Application.Mappers
{
    public class BusinessMappingsProfile : Profile
    {
        public BusinessMappingsProfile()
        {
            CreateMap<Business, BusinessResponseDto>()
             .ForMember(x => x.BusinessId, x => x.MapFrom(y => y.Id))
             .ForMember(x => x.District, x => x.MapFrom(y => y.District.Name))
             .ForMember(x => x.StateBusiness, x => x.MapFrom(y => y.State.Equals((int)StateTypes.Active) ? "Active" : "Inactive"))
             .ReverseMap();

            CreateMap<BaseEntityResponse<Business>, BaseEntityResponse<BusinessResponseDto>>()
               .ReverseMap();

            CreateMap<BusinessRequestDto, Business>();
        }
    }
}
