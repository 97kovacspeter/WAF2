using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FoodOrder.Website.Models
{
    public class LoginViewModel
    {
        [DisplayName("Név")]
        [Required]
        public string UserName { get; set; }

        [Required]
        [DisplayName("Jelszó")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [DisplayName("Név")]

        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [DisplayName("Jelszó")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        [DisplayName("Jelszó újra")]
        public string PasswordRepeat { get; set; }
    }
}
