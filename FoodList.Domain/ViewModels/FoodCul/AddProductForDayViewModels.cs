using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodList.Domain.ViewModels.FoodCul
{
    public class AddProductForDayViewModels
    {

        [Required(ErrorMessage = "Введите вес продукта!")]
        public int Weight { get; set; }
        public string Name { get; set; }


    }
}
