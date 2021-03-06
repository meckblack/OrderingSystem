﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OrderingSystem.Data;
using OrderingSystem.Models;

namespace OrderingSystem.Controllers
{
    public class DishesController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IHostingEnvironment _environment;

        #region Controller

        public DishesController(ApplicationDbContext context, IHostingEnvironment environment)
        {
            _db = context;
            _environment = environment;
        }

        #endregion

        #region Dish Index

        // GET: Dishes
        public async Task<IActionResult> Index()
        {
            var dishes = _db.Dishes.Include(d => d.Meal);
            return View(await dishes.ToListAsync());
        }

        #endregion

        #region Dish Details

        // GET: Dishes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dish = await _db.Dishes
                .Include(d => d.Meal)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (dish == null)
            {
                return NotFound();
            }

            return View(dish);
        }

        #endregion

        #region Dish Create

        // GET: Dishes/Create
        public IActionResult Create()
        {
            ViewData["MealId"] = new SelectList(_db.Meals, "Id", "Name");
            
            return PartialView();
        }

        // POST: Dishes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Image,Amount,MealId")] Dish dish, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("null_img", "File not selected");
            }
            else
            {
                var fileinfo = new FileInfo(file.FileName);
                var filename = DateTime.Now.ToFileTime() + fileinfo.Extension;
                var uploads = Path.Combine(_environment.WebRootPath, "uploads");

                if (file.Length > 0)
                {
                    using (var fileStream = new FileStream(Path.Combine(uploads, filename), FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                }

                if (ModelState.IsValid)
                {
                    dish.Image = filename;
                    _db.Dishes.Add(dish);
                    await _db.SaveChangesAsync();
                    TempData["success"] = "Dish has been added!";
                    return RedirectToAction(nameof(Index));
                }
            }

            ViewData["MealId"] = new SelectList(_db.Meals, "Id", "Name", dish.MealId);
            return View(dish);
        }

        #endregion

        #region Dish Edit

        // GET: Dishes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dish = await _db.Dishes.SingleOrDefaultAsync(m => m.Id == id);
            if (dish == null)
            {
                return NotFound();
            }
            ViewData["MealId"] = new SelectList(_db.Meals, "Id", "Name", dish.MealId);
            return View(dish);
        }

        // POST: Dishes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Image,Amount,MealId")] Dish dish, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("null_img", "File not selected");
            }
            else
            {
                var fileinfo = new FileInfo(file.FileName);
                var filename = DateTime.Now.ToFileTime() + fileinfo.Extension;
                var uploads = Path.Combine(_environment.WebRootPath, "uploads");

                if (file.Length > 0)
                {
                    using (var fileStream = new FileStream(Path.Combine(uploads, filename), FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                }

                if (id != dish.Id)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _db.Update(dish);
                        await _db.SaveChangesAsync();
                        TempData["success"] = "Meal has been modified!";
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!DishExists(dish.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return RedirectToAction(nameof(Index));
                }

            
            }
            ViewData["MealId"] = new SelectList(_db.Meals, "Id", "Name", dish.MealId);
            return View(dish);
        }

        #endregion

        #region Dish Delete

        // GET: Dishes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dish = await _db.Dishes
                .Include(d => d.Meal)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (dish == null)
            {
                return NotFound();
            }

            return View(dish);
        }

        // POST: Dishes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dish = await _db.Dishes.SingleOrDefaultAsync(m => m.Id == id);
            _db.Dishes.Remove(dish);
            await _db.SaveChangesAsync();
            TempData["success"] = "Dish has been removed!";
            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Dish Dispose

        private bool DishExists(int id)
        {
            return _db.Dishes.Any(e => e.Id == id);
        }

        #endregion




    }
}
