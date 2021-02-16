using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FoodOrder.Persistence;

namespace FoodOrder.Website.Services
{
    public class FoodService
    {
        private readonly FoodOrderDbContext _context;

        public FoodService(FoodOrderDbContext context)
        {
            _context = context;
        }

        public List<DrinkOrDish> GetBestTen()
        {
            return _context.DrinksOrDishes
                .OrderByDescending(l => l.Fame)
                .ThenBy(l => l.Name)
                .Take(10)
                .ToList();
        }

        public int GetSum(string orders)
        {
            var currOrders = orders;
            var sum = 0;
            foreach (var item in currOrders.Split(' '))
            {
                if (item != "")
                {
                    sum += GetDishById(int.Parse(item)).Price;
                }
            }

            return sum;
        }

        public List<DrinkOrDish> GetSearch(string searchString = null)
        {
            return _context.DrinksOrDishes
                .Where(l => l.Name.Contains(searchString ?? ""))
                .OrderBy(l => l.Name)
                .ToList();
        }

        public DrinkOrDish GetDishById(int id)
        {
            return _context.DrinksOrDishes.Find(id);
        }

        public void FameInc(int id)
        {
            _context.DrinksOrDishes.Find(id).Fame++;
            _context.SaveChanges();
        }

        public void SaveOrder(Order order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();
        }

        public List<Category> GetCategories()
        {
            return _context.Categories.ToList();
        }

        public Category GetCategory(int id)
        {
            return _context.Categories.Find(id);
        }

        public List<DrinkOrDish> GetDishes(int id)
        {
            return _context.DrinksOrDishes
                .Where(l => l.CategoryId.Equals(id))
                .ToList();
        }

    }
}
