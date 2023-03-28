using AutoMapper;
using POS.Application.Dtos.Product.Request;
using POS.Application.Dtos.Sale.Response;
using POS.Domain.Entities;
using POS.Infrastructure.Commons.Bases.Response;
using POS.Utilities.Static;

namespace POS.Application.Mappers
{
    public class SaleMappingsProfile : Profile
    {
        public SaleMappingsProfile()
        {
            CreateMap<Sale, SaleResponseDto>()
              .ForMember(x => x.SaleId, x => x.MapFrom(y => y.Id))
              .ForMember(x => x.Client, x => x.MapFrom(y => y.Client.Name))
              .ForMember(x => x.User, x => x.MapFrom(y => y.User.UserName))
              .ForMember(x => x.StateSale, x => x.MapFrom(y => y.State.Equals((int)StateTypes.Active) ? "Active" : "Inactive"))
              .ReverseMap();

            CreateMap<BaseEntityResponse<Client>, BaseEntityResponse<SaleResponseDto>>()
                .ReverseMap();

            CreateMap<SaleRequestDto, Sale>();
        }
    }
}
