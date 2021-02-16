using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using FoodOrder.Website.Services;
using FoodOrder.Persistence;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace FoodOrder.Website.Controllers
{
    public class HomeController : Controller
    {
        private readonly FoodService _foodService;

        public HomeController(FoodService foodService)
        {
            _foodService = foodService;
        }

        public IActionResult Index()
        {
            ViewBag.CategoriesBag = _foodService.GetCategories();
            ViewBag.FoodBag = _foodService.GetBestTen();
            return View();
        }

        public IActionResult CategoryPage(int? id, string searchString)
        {
            if (id == null)
            {
                return BadRequest();
            }

            ViewBag.CategoriesBag = _foodService.GetCategories();

            var category = _foodService.GetCategory((int)id);

            var foods = _foodService.GetDishes((int)id);

            if (category == null)
            {
                return BadRequest();
            }

            ViewBag.SearchString = searchString;

            ViewBag.CategoryBag = category;

            ViewBag.FoodBag = foods;

            return View(_foodService.GetSearch(searchString));
        }

        [HttpGet]
        public IActionResult AddedToCart(int id)
        {
            var ordered = _foodService.GetDishById(id);

            ViewBag.CategoriesBag = _foodService.GetCategories();

            if (ordered == null)
            {
                return BadRequest();
            }

            var send = "";
            if (Request.Cookies.ContainsKey("order"))
            {
                send = Request.Cookies["order"];
            }

            var sum = 0;
            foreach (var item in send.Split(" "))
            {
                if (item != "")
                {
                    sum += _foodService.GetDishById(int.Parse(item)).Price;
                }
            }

            if (sum + _foodService.GetDishById(id).Price <= 20000)
            {
                send += id.ToString() + " ";
                Response.Cookies.Append("order", send, new CookieOptions { Expires = DateTime.Today.AddDays(2) });
            }
            else
            {
                return View(-1);
            }

            return View(ordered.CategoryId);
        }

        public IActionResult Cart()
        {

            ViewBag.CategoriesBag = _foodService.GetCategories();
            var send = "";
            if (Request.Cookies.ContainsKey("order"))
            {
                send = Request.Cookies["order"];
            }

            var orders = new List<DrinkOrDish>();
            foreach (var item in send.Split(" "))
            {
                if (item != "")
                {
                    orders.Add(_foodService.GetDishById(int.Parse(item)));
                }
            }

            int id = 0;
            if (send == "")
            {
                id = -1;
            }

            ViewBag.OrderBag = orders;

            return View(id);
        }

        public IActionResult RemovedFromCart(int id)
        {

            ViewBag.CategoriesBag = _foodService.GetCategories();
            if (id != -1)
            {
                var send = "";
                if (Request.Cookies.ContainsKey("order"))
                {
                    send = Request.Cookies["order"];
                }

                if (send.Contains(id.ToString()))
                {
                    if (send.IndexOf(id.ToString(), StringComparison.Ordinal) >= 0 && send.Length > send.IndexOf(id.ToString(), StringComparison.Ordinal))
                        send = send.Remove(send.IndexOf(id.ToString(), StringComparison.Ordinal), 2);
                }

                Response.Cookies.Append("order", send, new CookieOptions { Expires = DateTime.Today.AddDays(2) });

                return View();
            }
            else
            {
                const string send = "";
                Response.Cookies.Append("order", send, new CookieOptions { Expires = DateTime.Today.AddDays(2) });
                return View();
            }

        }

        [HttpPost]
        public IActionResult End(Order model)
        {
            if (model == null)
            {
                return BadRequest();
            }

            if (string.IsNullOrEmpty(model.Name))
            {
                ModelState.AddModelError("Name", "Required!");
            }
            if (string.IsNullOrEmpty(model.Address))
            {
                ModelState.AddModelError("Address", "Required!");
            }
            if (string.IsNullOrEmpty(model.Phone))
            {
                ModelState.AddModelError("Phone", "Required!");
            }

            if (ModelState.IsValid)
            {
                var send = "";
                if (Request.Cookies.ContainsKey("order"))
                {
                    send = Request.Cookies["order"];
                }

                var order = new Order
                {
                    Address = model.Address,
                    Name = model.Name,
                    Phone = model.Phone,
                    Delivered = false,
                    DrinksOrDishes = send,
                    Sum = _foodService.GetSum(send),
                    OrderedDate = DateTime.Now
                };

                _foodService.SaveOrder(order);

                foreach (var item in send.Split(" "))
                {
                    if (item != "")
                    {
                        _foodService.FameInc(int.Parse(item));
                    }
                }
                send = "";
                Response.Cookies.Append("order", send, new CookieOptions { Expires = DateTime.Today.AddDays(2) });

                return RedirectToAction("Success");
            }

            return BadRequest();
        }

        public IActionResult SendOrder()
        {
            var model = new Order();
            return View(model);
        }

        public IActionResult Success()
        {
            return View();
        }
    }
}
