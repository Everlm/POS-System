using AutoMapper;
using POS.Application.Dtos.Department.Request;
using POS.Application.Dtos.Department.Response;
using POS.Domain.Entities;
using POS.Infrastructure.Commons.Bases.Response;
using POS.Utilities.Static;

namespace POS.Application.Mappers
{
    public class DepartmentMappingsProfile : Profile
    {
        public DepartmentMappingsProfile()
        {
            CreateMap<Department, DeparmentReponseDto>()
               .ForMember(x => x.StateDeparment, x => x.MapFrom(y => y.State.Equals((int)StateTypes.Active) ? "Active" : "Inactive"))
               .ReverseMap();

            CreateMap<BaseEntityResponse<Department>, BaseEntityResponse<DeparmentReponseDto>>()
                .ReverseMap();

            CreateMap<DeparmentRequestDto, Department>();
        }
    }
}
