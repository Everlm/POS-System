using AutoMapper;
using POS.Application.Dtos.PurchaseDetail;
using POS.Domain.Entities;

namespace POS.Application.Mappers
{
    public class PurchaseDetailMappingsProfile : Profile
    {
        public PurchaseDetailMappingsProfile()
        {
            CreateMap<PurchaseDetailDto, PurcharseDetail>().ReverseMap();
        }
    }
}
