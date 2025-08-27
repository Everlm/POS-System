using AutoMapper;
using POS.Application.Dtos.ProductStock;
using POS.Domain.Entities;

namespace POS.Application.Mappers
{
    public class ProductStockMappingsProfile : Profile
    {
        public ProductStockMappingsProfile()
        {
            CreateMap<ProductStock, ProductStockByWarehouseDto>()
              .ForMember(x => x.Warehouse, x => x.MapFrom(y => y.Warehouse.Name))
              .ReverseMap();
        }
    }
}
