using System;
using System.Collections.Generic;
using System.Text;
using FoodOrder.Persistence;

namespace FoodOrder.WebAPI.Tests
{
    public static class TestDbInitializer
    {
        public static void Initialize(FoodOrderDbContext context)
        {
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
                            Price = 10001,
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

            IList<Order> defaultOrders = new List<Order>
            {
                new Order()
                {
                    Address = "Teszt utca",
                    Delivered = false,
                    DrinksOrDishes = "2 2 1",
                    Name="Teszt Név",
                    Phone = "Teszt telefon",
                    Sum=2000,
                    OrderedDate = DateTime.Now
                },
                new Order()
                {
                    Address = "Teszt2 utca",
                    Delivered = false,
                    DrinksOrDishes = "2 2 1",
                    Name="Teszt2 Név",
                    Phone = "Teszt2 telefon",
                    Sum=2000,
                    OrderedDate = DateTime.Now
                }
            };

            foreach (var list in defaultCategories)
                context.Categories.Add(list);
            foreach (var list in defaultOrders)
                context.Orders.Add(list);

            context.SaveChanges();
        }
    }
}
