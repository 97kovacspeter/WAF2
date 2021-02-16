using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace FoodOrder.Persistence
{
    public class FoodOrderDbContext : IdentityDbContext<ApplicationUser>
    {
        public FoodOrderDbContext(DbContextOptions<FoodOrderDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }

        public DbSet<DrinkOrDish> DrinksOrDishes { get; set; }

        public DbSet<Order> Orders { get; set; }

    }
}
