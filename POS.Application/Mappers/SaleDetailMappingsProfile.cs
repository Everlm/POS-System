using AutoMapper;
using POS.Application.Dtos.SaleDetails;
using POS.Domain.Entities;

namespace POS.Application.Mappers
{
    public class SaleDetailMappingsProfile : Profile
    {
        public SaleDetailMappingsProfile()
        {
            CreateMap<SaleDetailDto, SaleDetail>().ReverseMap();
        }
    }
}
