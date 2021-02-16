using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace FoodOrder.Persistence
{
    public static class DbInitializer
    {
        public static void Initialize(FoodOrderDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            /*
			 Adatbázis létrehozása, amennyiben false létezik. A változásokat false kezeli. Ha változtattunk az
             adatbázis modelljén (a code-first osztályainkon), akkor az adatbázist törölni kell, hogy itt
             újra létrejöhessen a frissített szerkezettel.
             Törléshez az Sql Server Object Explorer ablak használható a Visual Studioban.
             Itt SQL Server > localdb > databases, vagy valami hasonló útvonalon találjuk az adatbázisainkat.
			 */
            //context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            if (context.Categories.Any())
            {
                return; // Az adatbázis már inicializálva van.
            }

            IList<Category> defaultCategories = new List<Category>
            {
                new Category()
                {
                    Name = "Levesek",
                    Items = new List<DrinkOrDish>()
                    {
                        new DrinkOrDish()
                        {
                            Name = "Gulyásleves",
                            Description = "Soup made with beef",
                            Price = 1000,
                            Spicy = true,
                            Vegetarian = false,
                            Fame = 0
                        },
                        new DrinkOrDish()
                        {
                            Name = "Brokkoli krémleves",
                            Description = "Soup made with broccoli",
                            Price = 500,
                            Spicy = false,
                            Vegetarian = true,
                            Fame = 0
                        },
                        new DrinkOrDish()
                        {
                            Name = "Jókai bableves",
                            Description = "Soup made with bean",
                            Price = 750,
                            Spicy = false,
                            Vegetarian = false,
                            Fame = 0
                        }
                    }
                },
                new Category()
                {
                    Name = "Főételek",
                    Items = new List<DrinkOrDish>()
                    {
                        new DrinkOrDish()
                        {
                            Name = "Hortobágyi palacsinta",
                            Description = "Pancakes with meat",
                            Price = 1250,
                            Spicy = false,
                            Vegetarian = false,
                            Fame = 0
                        },
                        new DrinkOrDish()
                        {
                            Name = "Szilvás gombóc",
                            Description = "Sweet pastry with plum",
                            Price = 1000,
                            Spicy = false,
                            Vegetarian = true,
                            Fame = 0
                        },
                        new DrinkOrDish()
                        {
                            Name = "Spenótfőzelék",
                            Description = "Spinach with eggs",
                            Price = 800,
                            Spicy = false,
                            Vegetarian = true,
                            Fame = 0
                        }

                    }
                },
                new Category()
                {
                    Name = "Pizzák",
                    Items = new List<DrinkOrDish>()
                    {
                        new DrinkOrDish()
                        {
                            Name = "Mozzarella caprese",
                            Description = "Pizza with mozzarella",
                            Price = 1250,
                            Spicy = false,
                            Vegetarian = true,
                            Fame = 0
                        },
                        new DrinkOrDish()
                        {
                            Name = "Húsimádó",
                            Description = "Pizza with 4 type of meat",
                            Price = 1000,
                            Spicy = false,
                            Vegetarian = false,
                            Fame = 0
                        },
                        new DrinkOrDish()
                        {
                            Name = "Csípős SonGoKu",
                            Description = "Spicy pizza with corn and mushroom",
                            Price = 800,
                            Spicy = true,
                            Vegetarian = false,
                            Fame = 0
                        }

                    }
                },
                new Category()
                {
                    Name = "Italok",
                    Items = new List<DrinkOrDish>()
                    {
                        new DrinkOrDish()
                        {
                            Name = "Jalapeño Margarita",
                            Description = "A 'hot' beverage",
                            Price = 1250,
                            Spicy = true,
                            Vegetarian = true,
                            Fame = 0
                        },
                        new DrinkOrDish()
                        {
                            Name = "Blonde Manhattan",
                            Description = "Taliesin's favourite",
                            Price = 1000,
                            Spicy = false,
                            Vegetarian = true,
                            Fame = 0
                        },
                        new DrinkOrDish()
                        {
                            Name = "Fanta",
                            Description = "The old fashioned non-alcoholic drink",
                            Price = 350,
                            Spicy = false,
                            Vegetarian = true,
                            Fame = 0
                        }

                    }
                }
            };

            Task.Run(async () =>
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));
                var adminUser = new ApplicationUser()
                {
                    UserName = "admin",
                    Email = "admin@xy.zz"
                };
                await userManager.CreateAsync(adminUser, "123");
                await userManager.AddToRoleAsync(adminUser, "admin");
            }).GetAwaiter().GetResult();


            foreach (var list in defaultCategories)
                context.Categories.Add(list);

            context.SaveChanges();

        }
    }
}
