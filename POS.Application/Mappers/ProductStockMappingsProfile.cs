using AutoMapper;
using POS.Application.Commons.Select.Response;
using POS.Application.Dtos.ProductStock;
using POS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
