using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Timesheets.Areas.Identity.Data;
using Timesheets.Data;
using Timesheets.Models;

namespace Timesheets.Controllers
{
    public class ApplicationUsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ApplicationUsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        //Get:ApplicationUsers
        public async Task<IActionResult> Index()
        {
            ApplicationUsersViewModel model = new ApplicationUsersViewModel
            {
                ApplicationUsers=_context.ApplicationUsers.ToList(),
                TimesheetEntries=_context.TimesheetEntries.ToList()
            };
            return View(model);
        }

        //Get:ApplicationUsers/Details/{id}
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
            ApplicationUsersDetailsViewModel model = new ApplicationUsersDetailsViewModel
            {
                ApplicationUser = user,
                Manager = user.Manager,
                TimesheetEntries = _context.TimesheetEntries.ToList()
            };
            return View(model);
            
        }

        //Get:Users/Create
        public IActionResult Create()
        {
            ViewBag.ApplicationUsers = new SelectList(_context.ApplicationUsers
                .Select(u => new { FullName = String.Format("{0} {1}", u.FirstName, u.LastName), u.Id })
                , "Id", "FullName");
            return View();
        }
        //POST:Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName")] ApplicationUser user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
           
            return View(user);
        }

        // GET: Users/Edit/{id}
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
            ViewBag.ApplicationUsers = new SelectList(_context.ApplicationUsers
                .Select(u => new { FullName = String.Format("{0} {1}", u.FirstName, u.LastName), u.Id })
                , "Id", "FullName");
            return View(user);
        }

        // POST: Users/Edit/{id}
        [HttpPost]

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
            
            return View(user);
        }

        // GET: Users/Delete/{id}
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
            ApplicationUsersDetailsViewModel model = new ApplicationUsersDetailsViewModel
            {
                ApplicationUser = user,
                Manager = user.Manager,
                TimesheetEntries = _context.TimesheetEntries
            };


            return View(model);
        }

        // POST: Users/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
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