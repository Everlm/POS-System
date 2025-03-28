﻿using POS.Application.Commons.Bases.Response;
using POS.Application.Dtos.ProductStock;

namespace POS.Application.Interfaces
{
    public interface IProductStockApplication
    {
        Task<BaseResponse<IEnumerable<ProductStockByWarehouseDto>>> GetProductStockByWarehouseAsync(int productId);
    }
}
