using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderingSystem.Data;

namespace OrderingSystem.Controllers
{
    public class ShopController : Controller
    {
        private readonly ApplicationDbContext _db;

        #region Controller

        public ShopController(ApplicationDbContext context)
        {
            _db = context;
        }

        #endregion
        

        public IActionResult Index()
        {
            return View();
        }

        #region Shop Single

        [Route("shop/single/{id}")]
        public async Task<IActionResult> Single(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var single = await _db.Dishes
                .SingleOrDefaultAsync(m => m.Id == id);

            if (single == null)
            {
                return NotFound();
            }

            return View(single);
        }
        
        #endregion


        public JsonResult FetchMeals()
        {
            var meal = _db.Meals.OrderBy(s => s.Name).ToList();
            return Json(meal);
        }

        public JsonResult FetchDishes(int id)
        {
            var dishes = _db.Dishes.Where(s => s.MealId == id).ToList();
            return Json(dishes);
        }
    }
}