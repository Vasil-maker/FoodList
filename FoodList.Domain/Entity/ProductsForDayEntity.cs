using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodList.Domain.Entity
{
    public class ProductsForDayEntity
    {
        [Key]
        public long ID { get; set; }

        public DateTime Date { get; set; }
        public int Weight { get; set; }
        public string Name { get; set; }
        public double Calories { get; set; }
        public double Protein { get; set; }
        public double Fats { get; set; }
        public double Carbohydrates { get; set; }
    }
}
