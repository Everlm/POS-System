using AutoMapper;
using POS.Application.Dtos.BranchOffice.Request;
using POS.Application.Dtos.BranchOffice.Response;
using POS.Domain.Entities;
using POS.Infrastructure.Commons.Bases.Response;
using POS.Utilities.Static;

namespace POS.Application.Mappers
{
    public class BranchOfficeMappingsProfile : Profile
    {
        public BranchOfficeMappingsProfile()
        {
            CreateMap<BranchOffice, BranchOfficeResponseDto>()
            .ForMember(x => x.BranchOfficeId, x => x.MapFrom(y => y.Id))
            .ForMember(x => x.District, x => x.MapFrom(y => y.District.Name))
            .ForMember(x => x.Business, x => x.MapFrom(y => y.Business.BusinessName))
            .ForMember(x => x.StateBranchOffice, x => x.MapFrom(y => y.State.Equals((int)StateTypes.Active) ? "Active" : "Inactive"))
            .ReverseMap();

            CreateMap<BaseEntityResponse<BranchOffice>, BaseEntityResponse<BranchOfficeResponseDto>>()
                .ReverseMap();

            CreateMap<BranchOfficeRequestDto, BranchOffice>();
        }
    }
}
