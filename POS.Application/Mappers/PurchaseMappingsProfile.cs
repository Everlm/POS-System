using AutoMapper;
using POS.Application.Dtos.Purchase.Request;
using POS.Application.Dtos.Purchase.Response;
using POS.Domain.Entities;
using POS.Infrastructure.Commons.Bases.Response;
using POS.Utilities.Static;

namespace POS.Application.Mappers
{
    public class PurchaseMappingsProfile : Profile
    {
        public PurchaseMappingsProfile()
        {
            CreateMap<Purcharse, PurchaseResponseDto>()
             .ForMember(x => x.PurcharseId, x => x.MapFrom(y => y.Id))
             .ForMember(x => x.Provider, x => x.MapFrom(y => y.Provider!.Name))
             .ForMember(x => x.User, x => x.MapFrom(y => y.User!.UserName))
             .ForMember(x => x.StatePurchase, x => x.MapFrom(y => y.State.Equals((int)StateTypes.Active) ? "Active" : "Inactive"))
             .ReverseMap();

            CreateMap<BaseEntityResponse<Purcharse>, BaseEntityResponse<PurchaseResponseDto>>()
                    .ReverseMap();

            CreateMap<PurchaseRequestDto, Purcharse>();
        }

    }
}
