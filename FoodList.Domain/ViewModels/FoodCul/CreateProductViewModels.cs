using FoodList.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodList.Domain.ViewModels.FoodCul
{

    /*
      код работает на стороне сервера и срабатывает только после того, как форма
    отправлена. А браузерная валидация делает так, чтобы пользователи сразу видели ошибки,
    не дожидаясь отправки.
     */
    public class CreateProductViewModels
    {
        [Required(ErrorMessage = "Выбирете категорию")]
        public Categories Category { get; set; }

        [Required(ErrorMessage = "Введите название продукта")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Введите каллорийность")]
        public double Calories { get; set; }

        [Required(ErrorMessage = "Введите кол-во белков")]
        public double Protein { get; set; }

        [Required(ErrorMessage = "Введите кол-во жиров")]
        public double Fats { get; set; }

        [Required(ErrorMessage = "Введите кол-во углеводов")]
        public double Carbohydrates { get; set; }
    }
}
