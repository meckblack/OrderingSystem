using System;
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
    public class MealsController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IHostingEnvironment _environment;

        #region Controller

        public MealsController(ApplicationDbContext context, IHostingEnvironment environment)
        {
            _db = context;
            _environment = environment;
        }

        #endregion

        #region Meal Index

        // GET: Meals
        public async Task<IActionResult> Index()
        {
            return View(await _db.Meals.ToListAsync());
        }

        #endregion

        #region Meal Details

        // GET: Meals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var meal = await _db.Meals
                .SingleOrDefaultAsync(m => m.Id == id);
            if (meal == null)
            {
                return NotFound();
            }

            return View(meal);
        }

        #endregion

        #region Meal Create

        // GET: Meals/Create
        public ActionResult Create()
        { 
            return View();
        }

        // POST: Meal/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Meal meal, IFormFile file)
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
                    meal.Image = filename;
                    _db.Meals.Add(meal);
                    await _db.SaveChangesAsync();
                    TempData["success"] = "Meal has been added!";
                    return RedirectToAction(nameof(Index));
                }
            }
            return View();
        }

        #endregion

        #region Meal Edit

        // GET: Meals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var meal = await _db.Meals.SingleOrDefaultAsync(m => m.Id == id);
            if (meal == null)
            {
                return NotFound();
            }
            return View(meal);
        }

        // POST: Meals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Image")] Meal meal, IFormFile file)
        {
            if (id != meal.Id)
            {
                return NotFound();
            }
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
                    try
                    {
                        meal.Image = filename;
                        _db.Update(meal);
                        await _db.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!MealExists(meal.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    TempData["success"] = "Meal has been modified!";
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(meal);
        }




        #endregion

        #region Meal Delete

        // GET: Meals/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var meal = await _db.Meals
                .SingleOrDefaultAsync(m => m.Id == id);
            if (meal == null)
            {
                return NotFound();
            }

            return View(meal);
        }

        // POST: Meals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var meal = await _db.Meals.SingleOrDefaultAsync(m => m.Id == id);
            _db.Meals.Remove(meal);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region MealExits

        private bool MealExists(int id)
        {
            return _db.Meals.Any(e => e.Id == id);
        }

        #endregion



    }
}
