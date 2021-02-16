using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FoodOrder.Persistence
{
    public class AccountDto
    {
        [Required] public string UserName { get; set; }

        [Required] public string Password { get; set; }
    }
}
