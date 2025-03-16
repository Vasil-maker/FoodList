using Azure;
using FoodList.Domain.Entity;
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



        [HttpGet]
        public async Task<IActionResult> Crafting()
        {
            var response = await _foodCulSerivce.GetAllProducts();
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                ViewBag.Products = response.Data;
            }
            else
            {
                ViewBag.Products = new List<FoodCulEntity>();
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCombinedProduct([FromBody] CreateProductViewModels model)
        {
            //Атрибут [FromBody] говорит ASP.NET Core, что данные нужно взять из тела
            //запроса (в формате JSON) и преобразовать их в объект CreateProductViewModels.

            if (ModelState.IsValid)
            {
                var response = await _foodCulSerivce.CreateCombinedProduct(model);
                if (response.StatusCode == Domain.Enum.StatusCode.OK)
                {
                    return Json(new { statusCode = 0, description = response.Description });
                }
                return Json(new { statusCode = 1, description = response.Description });
            }

            // Логирование ошибок валидации
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            return Json(new { statusCode = 2, description = "Некорректные данные: " + string.Join(", ", errors) });
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


        /*
         НИЖЕ ФУНКЦИОНАЛ ОТОБРАЖЕНИЯ СЪЕДЕННЫХ ПРОДУКТОВ ЗА ДЕНЬ!
        Метод Result:
        - Используется для первоначальной загрузки страницы.
        - Возвращает HTML-страницу с продуктами за выбранную дату.

        Метод GetProductsForDay:
        - Используется для асинхронной загрузки данных (AJAX).
        - Позволяет обновлять таблицу с продуктами без перезагрузки страницы.
        
        Если убрать GetProductsForDay: При изменении даты страница будет перезагружаться,
        чтобы отобразить новые данные.
         */
        [HttpGet]
        public async Task<IActionResult> Result(DateTime? date)
        {
            var selectedDate = date ?? DateTime.Today;

            var response = await _foodCulSerivce.GetProductsForDay(selectedDate);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                ViewBag.SelectedDate = selectedDate.ToString("yyyy-MM-dd");
                ViewBag.Products = response.Data;
            }
            else
            {
                ViewBag.Products = new List<ProductsForDayEntity>();
            }

            return View();
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
        public async Task<IActionResult> GetProductsForDay(DateTime date)
        {
            var response = await _foodCulSerivce.GetProductsForDay(date);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return Json(new { statusCode = 0, products = response.Data });
            }

            return Json(new { statusCode = 1, description = response.Description });
        }


        //Метод Deleting должен загружать список продуктов и передавать их в представление.
        [HttpGet]
        public async Task<IActionResult> Deleting()
        {
            var response = await _foodCulSerivce.GetAllProducts();
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                ViewBag.Products = response.Data;
            }
            else
            {
                ViewBag.Products = new List<FoodCulEntity>();
            }

            return View();
        }


        //Добавим метод DeleteProducts в контроллере для обработки запроса на удаление.
        //Этот метод будет принимать массив ID продуктов и удалять их из базы данных.
        [HttpPost]
        public async Task<IActionResult> DeleteProducts([FromBody] List<long> productIds)
        {
            if (productIds == null || productIds.Count == 0)
            {
                return Json(new { statusCode = 1, description = "Не выбрано ни одного продукта!" });
            }

            foreach (var id in productIds)
            {
                var response = await _foodCulSerivce.DeleteProduct(id);
                if (response.StatusCode != Domain.Enum.StatusCode.OK)
                {
                    return Json(new { statusCode = 2, description = $"Ошибка при удалении продукта с ID {id}: {response.Description}" });
                }
            }

            return Json(new { statusCode = 0, description = "Продукты успешно удалены!" });
        }

    }
}
