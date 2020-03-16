using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CS.Db.Context;
using CS.Db.Models.Rest;

namespace CS.WebApi.Areas.Moder.Controllers
{
    [Area("Moder")]
    public class RestUserController : Controller
    {
        private readonly AppDbContext _context;

        public RestUserController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Moder/RestUser
        public async Task<IActionResult> Index()
        {
            return View(await _context.RestUser.ToListAsync());
        }

        // GET: Moder/RestUser/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var restUserEntity = await _context.RestUser
                .FirstOrDefaultAsync(m => m.ID == id);
            if (restUserEntity == null)
            {
                return NotFound();
            }

            return View(restUserEntity);
        }

        // GET: Moder/RestUser/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Moder/RestUser/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Description,AuthKey")] RestUserEntity restUserEntity)
        {
            if (ModelState.IsValid)
            {
                _context.Add(restUserEntity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(restUserEntity);
        }

        // GET: Moder/RestUser/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var restUserEntity = await _context.RestUser.FindAsync(id);
            if (restUserEntity == null)
            {
                return NotFound();
            }
            return View(restUserEntity);
        }

        // POST: Moder/RestUser/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Description,AuthKey")] RestUserEntity restUserEntity)
        {
            if (id != restUserEntity.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(restUserEntity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RestUserEntityExists(restUserEntity.ID))
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
            return View(restUserEntity);
        }

        // GET: Moder/RestUser/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var restUserEntity = await _context.RestUser
                .FirstOrDefaultAsync(m => m.ID == id);
            if (restUserEntity == null)
            {
                return NotFound();
            }

            return View(restUserEntity);
        }

        // POST: Moder/RestUser/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var restUserEntity = await _context.RestUser.FindAsync(id);
            _context.RestUser.Remove(restUserEntity);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RestUserEntityExists(int id)
        {
            return _context.RestUser.Any(e => e.ID == id);
        }
    }
}
