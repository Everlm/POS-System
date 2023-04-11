using AutoMapper;
using POS.Application.Dtos.Province.Request;
using POS.Application.Dtos.Province.Response;
using POS.Domain.Entities;
using POS.Infrastructure.Commons.Bases.Response;
using POS.Utilities.Static;

namespace POS.Application.Mappers
{
    public class ProvinceMappingsProfile : Profile
    {
        public ProvinceMappingsProfile()
        {
            CreateMap<Province, ProvinceResponseDto>()
              .ForMember(x => x.ProvinceId, x => x.MapFrom(y => y.Id))
              .ForMember(x => x.Department, x => x.MapFrom(y => y.Department.Name))
              .ForMember(x => x.StateProvince, x => x.MapFrom(y => y.State.Equals((int)StateTypes.Active) ? "Active" : "Inactive"))
              .ReverseMap();

            CreateMap<BaseEntityResponse<Province>, BaseEntityResponse<ProvinceResponseDto>>()
               .ReverseMap();

            CreateMap<ProvinceRequestDto, Province>();
        }
    }
}
