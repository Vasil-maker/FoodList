using FoodList.DAL.Interfaces;
using FoodList.Domain.Entity;
using FoodList.Domain.Enum;
using FoodList.Domain.Response;
using FoodList.Domain.ViewModels.FoodCul;
using FoodListSerivce.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodListSerivce.Implementations
{
    public class FoodCulSerivce : IFoodCulSerivce
    {
        private readonly IBaseRepository<FoodCulEntity> _foodCulRepository;
        private readonly IBaseRepository<ProductsForDayEntity> _productsForDayRepository;

        private ILogger<FoodCulSerivce> _logger;

        public FoodCulSerivce(IBaseRepository<FoodCulEntity> foodCul, IBaseRepository<ProductsForDayEntity> prodDay,
            ILogger<FoodCulSerivce> logger)
        {
            _foodCulRepository = foodCul;
            _productsForDayRepository = prodDay;
            _logger = logger;
        }

        public async Task<IBaseResponse<ProductsForDayEntity>> AddProdDay(AddProductForDayViewModels model)
        {
            try
            { 
                // Ищем продукт в базе по названию
                var product = await _foodCulRepository.GetAll()
                    .FirstOrDefaultAsync(p => p.Name.ToLower() == model.Name.ToLower());

                if (product == null)
                {
                    return new BaseResponse<ProductsForDayEntity>()
                    {
                        Description = "Продукт не найден!",
                        StatusCode = StatusCode.ProductNotFound
                    };
                }

                // Рассчитываем БЖУ и калории
                double factor = model.Weight / 100.0;

                var prodForDay = new ProductsForDayEntity()
                {
                    Date = DateTime.UtcNow.Date, // Убираем возможные проблемы с форматированием даты
                    Weight = model.Weight,
                    Name = product.Name,
                    Calories = Math.Round(product.Calories * factor, 2),
                    Protein = Math.Round(product.Protein * factor, 2),
                    Fats = Math.Round(product.Fats * factor, 2),
                    Carbohydrates = Math.Round(product.Carbohydrates * factor, 2)
                };

                // Логируем перед сохранением (на всякий случай)
                _logger.LogInformation($"Добавляем продукт в базу: {prodForDay.Name}, {prodForDay.Weight} г.");

                // Сохраняем в базу
                await _productsForDayRepository.Create(prodForDay);

                return new BaseResponse<ProductsForDayEntity>()
                {
                    Data = prodForDay,
                    Description = "Продукт успешно добавлен!",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[FoodCulService.AddProdDay]: {ex.Message}");
                return new BaseResponse<ProductsForDayEntity>()
                {
                    Description = $"{ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<FoodCulEntity>> CreateCombinedProduct(CreateProductViewModels model)
        {
            try
            {
                _logger.LogInformation($"Запрос на создание комбинированного продукта - {model.Name}");

                var product = await _foodCulRepository.GetAll()
                    .FirstOrDefaultAsync(x => x.Name == model.Name);
                if (product != null)
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
                _logger.LogInformation($"Комбинированный продукт добавлен: {product.Name}");

                return new BaseResponse<FoodCulEntity>()
                {
                    Description = $"Комбинированный продукт добавлен!",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при создании комбинированного продукта");
                return new BaseResponse<FoodCulEntity>()
                {
                    Description = "Ошибка при создании комбинированного продукта",
                    StatusCode = StatusCode.InternalServerError
                };
            }
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


        public async Task<IBaseResponse<IEnumerable<FoodCulEntity>>> GetAllProducts()
        {
            try
            {
                var allProducts = await _foodCulRepository.GetAll().ToListAsync();

                return new BaseResponse<IEnumerable<FoodCulEntity>>()
                {
                    Data = allProducts,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[FoodCulSerivce.GetAllProducts]: {ex.Message}");
                return new BaseResponse<IEnumerable<FoodCulEntity>>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<IEnumerable<ProductsForDayEntity>>>
            GetProductsForDay(DateTime data)
        {
            try
            {
                var products = await _productsForDayRepository.GetAll()
                    .Where(x => x.Date == data)
                    .ToListAsync();

                return new BaseResponse<IEnumerable<ProductsForDayEntity>>()
                {
                    Data = products,
                    StatusCode = StatusCode.OK
                };
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"[FoodCulSerivce.GetProductsForDay]: {ex.Message}");
                return new BaseResponse<IEnumerable<ProductsForDayEntity>>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<bool>> DeleteProduct(long id)
        {
            try
            {
                var product = await _foodCulRepository.GetAll().FirstOrDefaultAsync(x => x.ID == id);

                if(product == null)
                {
                    return new BaseResponse<bool>()
                    {
                        Description = "Продукт не найден!",
                        StatusCode = StatusCode.ProductNotFound
                    };
                }

                await _foodCulRepository.Delete(product);

                return new BaseResponse<bool>()
                {
                    Description = "Продукт успешно удалён!",
                    StatusCode = StatusCode.OK
                };
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"[FoodCulSerivce.DeleteProduct]: {ex.Message}");
                return new BaseResponse<bool>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
    }
}
