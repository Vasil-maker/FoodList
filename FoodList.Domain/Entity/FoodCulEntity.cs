using FoodList.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodList.Domain.Entity
{
    public class FoodCulEntity
    {
        [Key]
        public long ID { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public Categories Category { get; set; }

        public string Name { get; set; }
        public double Calories { get; set; }
        public double Protein { get; set; }
        public double Fats { get; set; }
        public double Carbohydrates { get; set; }


    }
}
