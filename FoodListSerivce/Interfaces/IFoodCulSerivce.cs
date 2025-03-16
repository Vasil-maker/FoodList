using FoodList.Domain.Entity;
using FoodList.Domain.Response;
using FoodList.Domain.ViewModels.FoodCul;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodListSerivce.Interfaces
{
    public interface IFoodCulSerivce
    {
        Task<IBaseResponse<FoodCulEntity>> Create(CreateProductViewModels model);

        Task<IBaseResponse<ProductsForDayEntity>> AddProdDay(AddProductForDayViewModels model);
        Task<IBaseResponse<IEnumerable<FoodCulEntity>>> GetAllProducts();

        Task<IBaseResponse<IEnumerable<ProductsForDayEntity>>> GetProductsForDay(DateTime data);

        Task<IBaseResponse<FoodCulEntity>> CreateCombinedProduct(CreateProductViewModels model);

        Task<IBaseResponse<bool>> DeleteProduct(long id);

    }
}
