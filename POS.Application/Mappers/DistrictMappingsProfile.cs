using AutoMapper;
using POS.Application.Dtos.District.Request;
using POS.Application.Dtos.District.Response;
using POS.Domain.Entities;
using POS.Infrastructure.Commons.Bases.Response;
using POS.Utilities.Static;

namespace POS.Application.Mappers
{
    public class DistrictMappingsProfile : Profile
    {
        public DistrictMappingsProfile()
        {
            CreateMap<District, DistrictResponseDto>()
             .ForMember(x => x.DistrictId, x => x.MapFrom(y => y.Id))
             .ForMember(x => x.Province, x => x.MapFrom(y => y.Province.Name))
             .ForMember(x => x.StateDistrict, x => x.MapFrom(y => y.State.Equals((int)StateTypes.Active) ? "Active" : "Inactive"))
             .ReverseMap();

            CreateMap<BaseEntityResponse<District>, BaseEntityResponse<DistrictResponseDto>>()
               .ReverseMap();

            CreateMap<DistrictRequestDto, District>();
        }
    }
}
