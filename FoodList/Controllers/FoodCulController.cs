using Azure;
using FoodList.Domain.ViewModels.FoodCul;
using FoodListSerivce.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FoodList.Controllers
{
    public class FoodCulController : Controller
    {
        private readonly IFoodCulSerivce _foodCulSerivce;

        public FoodCulController(IFoodCulSerivce foodCulSerivce)
        {
            _foodCulSerivce = foodCulSerivce;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Result()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Crafting()
        {
            return View();
        }

        [HttpPost] 
        public async Task<IActionResult> Create(CreateProductViewModels model)
        {
            if (ModelState.IsValid)
            {
                var response = await _foodCulSerivce.Create(model);
                if (response.StatusCode == Domain.Enum.StatusCode.OK)
                {
                    return Json(new { statusCode = 0, description = response.Description });
                }
                return Json(new { statusCode = 1, description = response.Description });
            }
            return Json(new { statusCode = 2, description = "Некорректные данные!" });
        }



        [HttpPost]
        public async Task<IActionResult> AddProductForDay([FromBody] AddProductForDayViewModels model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { statusCode = 1, description = "Некорректные данные!" });
            }

            var response = await _foodCulSerivce.AddProdDay(model);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return Json(new
                {
                    statusCode = 0,
                    description = "Продукт успешно добавлен!",
                    product = new
                    {
                        name = response.Data.Name,
                        weight = response.Data.Weight,
                        calories = response.Data.Calories,
                        protein = response.Data.Protein,
                        fats = response.Data.Fats,
                        carbohydrates = response.Data.Carbohydrates
                    }
                });
            }

            return Json(new { statusCode = 1, description = response.Description });
        }



        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var response = await _foodCulSerivce.GetAllProducts();

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return Json(new { statusCode = 0, products = response.Data });
            }

            return Json(new { statusCode = 1, description = response.Description });
        }

        


    }
}
