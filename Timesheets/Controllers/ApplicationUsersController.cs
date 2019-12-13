using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Timesheets.Areas.Identity.Data;
using Timesheets.Data;

namespace Timesheets.Controllers
{
    public class ApplicationUsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ApplicationUsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        //Get:Users
        [Route("/Users")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ApplicationUsers.Include(p => p.Manager);
            return View(await applicationDbContext.ToListAsync());
        }

        //Get:ApplicationUsers/Details
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.ApplicationUsers
                .Include(p => p.Manager)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        //Get:Users/Create
        [HttpGet("/Users/Add")]
        public IActionResult Create()
        {
            //ViewData["ManagerId"] = new SelectList(_context.ApplicationUsers, "ApplicationUserId","ApplicationUserId");
            return View();
        }

        //Post:Users/Create
        [HttpPost("/Users/Add")]
        public async Task<IActionResult> Create([Bind("ApplicationUserId,Name,ManagerId")] ApplicationUser user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["ManagerId"] = new SelectList(_context.ApplicationUsers, "ApplicationUserId", "ApplicationUserId", user.ManagerId);
            return View(user);
        }

        // GET: Users/Edit
        [Route("/Users/Edit/")]
        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.ApplicationUsers.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            ViewData["ManagerId"] = new SelectList(_context.Departments, "ApplicationUserId", "ApplicationUserId", user.ManagerId);
            return View(user);
        }

        // POST: Users/Edit
        [HttpPost("/Users/Edit/"), ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ApplicationUserId,Name,ManagerId")] ApplicationUser user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
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
            ViewData["ManagerId"] = new SelectList(_context.ApplicationUsers, "ApplicationUserId", "ApplicationUserId", user.ManagerId);
            return View(user);
        }

        // GET: Users/Delete
        [Route("/Users/Delete/")]
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.ApplicationUsers
                .Include(p => p.Manager)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete
        [HttpPost("/Users/Delete/"), ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _context.ApplicationUsers.FindAsync(id);
            _context.ApplicationUsers.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(string id)
        {
            return _context.ApplicationUsers.Any(e => e.Id == id);
        }

        
    }
}