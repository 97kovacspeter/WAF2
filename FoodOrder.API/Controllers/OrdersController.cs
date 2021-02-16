using System;
using System.Collections.Generic;
using System.Linq;
using FoodOrder.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace FoodOrder.API.Controllers
{
    ///[Authorize]
    [Produces("application/json")]
    [Route("api/Orders")]
    public class OrdersController : Controller
    {
        private readonly FoodOrderDbContext _context;

        public OrdersController(FoodOrderDbContext context)
        {
            _context = context;
        }

        // GET: api/Orders
        [HttpGet]
        public IEnumerable<Order> GetOrders()
        {
            var tmp = _context.Orders.OrderBy(l => l.OrderedDate);
            return tmp;
        }

        // GET: api/Orders/5
        [HttpGet]
        [Route("{listId}")]
        public IEnumerable<DrinkOrDish> GetItems([FromRoute] int listId)
        {
            var items = new List<DrinkOrDish>();
            var orders = _context.Orders.Where(i => i.Id == listId);
            if (orders.Any())
            {
                var orderString = orders.First().DrinksOrDishes;

                foreach (var meals in orderString.Split(' '))
                {
                    if (meals != "")
                        items.Add(_context.DrinksOrDishes.Find(int.Parse(meals)));
                }

                if (items.Any())
                {
                    return items;
                }
            }
            return new List<DrinkOrDish>();
        }


        // GET: api/Orders/search
        [HttpGet]
        [Route("search")]
        public List<Order> GetEmptySearch()
        {
            return _context.Orders.ToList();
        }

        // GET: api/Orders/search/<string>
        [HttpGet]
        [Route("search/{searchString}")]
        public List<Order> GetSearch(string searchString)
        {
            //            Debug.WriteLine("**************************************");
            //            Debug.WriteLine(searchString);
            //            Debug.WriteLine("**************************************");
            return _context.Orders
                .Where(l => (l.Name.Contains(searchString) || l.Address.Contains(searchString)))
                .OrderBy(l => l.Name)
                .ToList();

        }

        // PUT: api/Orders/Deliver/5
        [HttpPut]
        [Route("Deliver/{id}")]
        public List<Order> DeliverOrder([FromRoute] int id)
        {
            var order = _context.Orders.Find(id);
            if (order != null)
            {
                if (order.Delivered == false)
                {
                    order.Delivered = true;
                    order.DeliveredDate = DateTime.Now;
                    _context.SaveChanges();
                }
            }
            return _context.Orders.ToList();
        }

        // POST: api/Orders/NewItem
        [HttpPost]
        [Route("NewItem")]
        public bool MakeNewItem([FromBody] DrinkOrDish dod)
        {
            var contains = false;
            if (string.IsNullOrWhiteSpace(dod.Name) ||
                string.IsNullOrWhiteSpace(dod.Description) || dod.Price == 0)
            {
                return false;
            }
            foreach (var meal in _context.DrinksOrDishes)
            {
                if (meal.Name == dod.Name)
                {
                    contains = true;
                }
            }
            if (!contains)
            {
                _context.DrinksOrDishes.Add(dod);
                _context.SaveChanges();
                return true;
            }

            return false;
        }

        //GET: api/Orders/Delivered
        [HttpGet]
        [Route("Delivered")]
        public IEnumerable<Order> GetDeliveredOrders()
        {
            return _context.Orders
                .Where(l => l.Delivered == true)
                .ToList();
        }

        //GET: api/Orders/Undelivered
        [HttpGet]
        [Route("Undelivered")]
        public IEnumerable<Order> GetUndeliveredOrders()
        {
            return _context.Orders
                .Where(l => l.Delivered == false)
                .ToList();
        }
    }
}