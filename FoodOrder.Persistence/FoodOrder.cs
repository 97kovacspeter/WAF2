using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace FoodOrder.Persistence
{
    public class Category
    {
        [Key] public int Id { get; set; }

        [Required] [MaxLength(50)] public string Name { get; set; }

        public ICollection<DrinkOrDish> Items { get; set; }
    }

    public class DrinkOrDish
    {
        [Key] public int Id { get; set; }

        [Required] [MaxLength(50)] public string Name { get; set; }

        [DataType(DataType.MultilineText)] public string Description { get; set; }

        [Required] [DisplayName("Category")] public int CategoryId { get; set; }

        public int Price { get; set; }

        public bool Spicy { get; set; }

        public bool Vegetarian { get; set; }

        public int Fame { get; set; } //how many times it was ordered
    }


    public class Order
    {
        [Key] public int Id { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(50)]
        public string Address { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(50)]
        public string Phone { get; set; }

        public string DrinksOrDishes { get; set; }

        public DateTime OrderedDate { get; set; }

        public DateTime DeliveredDate { get; set; }

        public int Sum { get; set; }

        public bool Delivered { get; set; }
    }

    /*
     Regisztrált felhasználók modellje. A legalapvetőbb tulajdonságokat
     (pl.: név jelszó) az IdentityUser már tartalmazza, de ebből leszármaztatva
     plusz mezőkkel egészíthetjük ezt.
     */
    public class ApplicationUser : IdentityUser
    {

    }
}
