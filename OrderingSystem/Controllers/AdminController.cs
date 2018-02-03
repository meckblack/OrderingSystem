using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OrderingSystem.Data;

namespace OrderingSystem.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _db;

        #region Controller

        public AdminController (ApplicationDbContext context)
        {
            _db = context;
        }

        #endregion


        public IActionResult Index()
        {
            ViewData["GetAllMeals"] = _db.Meals.ToArray().Length;
            ViewData["GetAllDishes"] = _db.Dishes.ToArray().Length;
            return View();
        }
    }
}