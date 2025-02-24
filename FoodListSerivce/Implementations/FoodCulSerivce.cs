using FoodList.DAL.Interfaces;
using FoodList.Domain.Entity;
using FoodList.Domain.Enum;
using FoodList.Domain.Response;
using FoodList.Domain.ViewModels.FoodCul;
using FoodListSerivce.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodListSerivce.Implementations
{
    public class FoodCulSerivce : IFoodCulSerivce
    {
        private readonly IBaseRepository<FoodCulEntity> _foodCulRepository;
        private ILogger<FoodCulSerivce> _logger;

        public FoodCulSerivce(IBaseRepository<FoodCulEntity> foodCul, 
            ILogger<FoodCulSerivce> logger)
        {
            _foodCulRepository = foodCul;
            _logger = logger;
        }


        public async Task<IBaseResponse<FoodCulEntity>> Create(CreateProductViewModels model)
        {
            try
            {
                _logger.LogInformation($"Запрос на создании задачи - {model.Name}");

                var product = await _foodCulRepository.GetAll()
                    .FirstOrDefaultAsync(x => x.Name == model.Name);
                if(product != null)
                {
                    return new BaseResponse<FoodCulEntity>()
                    {
                        Description = "Продукт с таким названием уже есть",
                        StatusCode = StatusCode.ProductIsHasAlready,
                        Data = product                
                    };
                }

                product = new FoodCulEntity()
                {
                    Category = model.Category,
                    Name = model.Name,
                    Calories = model.Calories,
                    Protein = model.Protein,
                    Fats = model.Fats,
                    Carbohydrates = model.Carbohydrates
                };

                await _foodCulRepository.Create(product);
                _logger.LogInformation($"Продукт добавлен: {product.Name}");

                return new BaseResponse<FoodCulEntity>()
                {
                    Description = $"Продукт добавлен!",
                    StatusCode = StatusCode.OK
                };

            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"[FoodCulSerivce.Create]: {ex.Message}");
                return new BaseResponse<FoodCulEntity>()
                {
                    Description = $"{ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
    }
}
