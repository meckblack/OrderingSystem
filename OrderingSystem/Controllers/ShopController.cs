using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OrderingSystem.Data;

namespace OrderingSystem.Controllers
{
    public class ShopController : Controller
    {
        public ApplicationDbContext app;

        public ShopController(ApplicationDbContext _app)
        {
            app = _app;
        }

        public IActionResult Index()
        {
            return View();
        }

        public JsonResult FetchMeals()
        {
            var meal = app.Meals.OrderBy(s => s.Name).ToList();
            return Json(meal);
        }

        public JsonResult FetchDishes(int id)
        {
            var dishes = app.Dishes.Where(s => s.MealId == id).ToList();
            return Json(dishes);
        }
    }
}