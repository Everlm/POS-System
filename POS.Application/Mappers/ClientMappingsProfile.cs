using AutoMapper;
using POS.Application.Dtos.Client.Response;
using POS.Domain.Entities;
using POS.Utilities.Static;

namespace POS.Application.Mappers;

public class ClientMappingsProfile : Profile
{
    public ClientMappingsProfile()
    {
        CreateMap<Client, ClientResponseDto>()
                .ForMember(x => x.ClientId, x => x.MapFrom(y => y.Id))
                .ForMember(x => x.DocumentType, x => x.MapFrom(y => y.DocumentType.Name))
                .ForMember(x => x.StateClient, x => x.MapFrom(y => y.State.Equals((int)StateTypes.Active) ? "Active" : "Inactive"))
                .ReverseMap();
    }
}
