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
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        //Get:Users
        public async Task<IActionResult> Index(string searchString)
        {
            IEnumerable<ApplicationUser> userList = _context.ApplicationUsers.Include(p => p.Manager);
            if (String.IsNullOrEmpty(searchString))
            {
                searchString = ViewBag.SearchString;
            }
            else
            {
                ViewBag.SearchString = searchString;
            }
            if (!String.IsNullOrEmpty(searchString))
            {
                userList = userList.Where(u => u.UserName.Contains(searchString, StringComparison.CurrentCultureIgnoreCase)
                                       || u.Email.Contains(searchString, StringComparison.CurrentCultureIgnoreCase)
                                       || u.FirstName.Contains(searchString, StringComparison.CurrentCultureIgnoreCase)
                                       || u.LastName.Contains(searchString, StringComparison.CurrentCultureIgnoreCase));
            }
            return View(userList.ToList());
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
        [HttpGet]
        public IActionResult Create()
        {
            //ViewData["ManagerId"] = new SelectList(_context.ApplicationUsers, "ApplicationUserId","ApplicationUserId");
            return View();
        }

        //Post:Users/Create
        [HttpPost]
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, ApplicationUser user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (!String.IsNullOrEmpty(user.FirstName) && !String.IsNullOrEmpty(user.LastName) 
                && !String.IsNullOrEmpty(user.Email) && user.ManHourCost > 0)
            {
                try
                {
                    var oldUser = await _context.ApplicationUsers.FindAsync(id);
                    oldUser.FirstName = user.FirstName;
                    oldUser.LastName = user.LastName;
                    oldUser.Email = user.Email;
                    oldUser.ManHourCost = user.ManHourCost;
                    _context.Update(oldUser);
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
        [HttpPost]
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