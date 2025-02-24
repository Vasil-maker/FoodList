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

        public IActionResult Index()
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
    }
}
