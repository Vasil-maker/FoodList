using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodList.Domain.Enum
{
    public enum StatusCode
    {
        ProductIsHasAlready = 1,
        ProductNotFound = 2,

        OK = 200,
        InternalServerError = 500
    }
}
