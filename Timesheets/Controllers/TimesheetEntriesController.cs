using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Timesheets.Data;
using Timesheets.Models;

namespace Timesheets.Controllers
{
    public class TimesheetEntriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TimesheetEntriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TimesheetEntries
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.TimesheetEntries.Include(t => t.Project).Include(t => t.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: TimesheetEntries/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var timesheetEntry = await _context.TimesheetEntries
                .Include(t => t.Project)
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.TimesheetEntryId == id);

            if (timesheetEntry == null)
            {
                return NotFound();
            }

            return View(timesheetEntry);
        }

        // GET: TimesheetEntries/Create
        public IActionResult Create()
        {
            //ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "ProjectId");
            //ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id");

            ViewBag.Projects = new SelectList(_context.Projects, "ProjectId", "Name");
            ViewBag.ApplicationUsers = new SelectList(_context.ApplicationUsers
               .Select(u => new { FullName = String.Format("{0} {1}", u.FirstName, u.LastName), u.Id })
               , "Id", "FullName");
            return View();
        }

        // POST: TimesheetEntries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( TimesheetEntry timesheetEntry)
        {
            var justCheck = _context.TimesheetEntries
                .Where(t => t.DateCreated.Date == timesheetEntry.DateCreated.Date && t.ProjectId == timesheetEntry.ProjectId).FirstOrDefault();

            if (ModelState.IsValid && justCheck == null && timesheetEntry.HoursWorked>0)
            {
                _context.Add(timesheetEntry);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "ProjectId", timesheetEntry.ProjectId);
            //ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", timesheetEntry.UserId);
            ViewBag.Projects = new SelectList(_context.Projects, "ProjectId", "Name");
            ViewBag.ApplicationUsers = new SelectList(_context.ApplicationUsers
               .Select(u => new { FullName = String.Format("{0} {1}", u.FirstName, u.LastName), u.Id })
               , "Id", "FullName");

            return View(timesheetEntry);
        }

        // GET: TimesheetEntries/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var timesheetEntry = await _context.TimesheetEntries
                .Include(t => t.Project)
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.TimesheetEntryId == id);
            if (timesheetEntry == null)
            {
                return NotFound();
            }

            var authorizationResult = await _authorizationService
                .AuthorizeAsync(User, timesheetEntry, "SameTimesheetEntryCreator");
            if (authorizationResult.Succeeded)
            {
                ViewBag.Projects = new SelectList(_context.Projects, "ProjectId", "Name");
                ViewBag.ApplicationUsers = new SelectList(_context.ApplicationUsers
                   .Select(u => new { FullName = String.Format("{0} {1}", u.FirstName, u.LastName), u.Id })
                   , "Id", "FullName");
                return View(timesheetEntry);
            }
            else if (User.Identity.IsAuthenticated)
            {
                return new ForbidResult();
            }
            else
            {
                return new ChallengeResult();
            }
        }

        // POST: TimesheetEntries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, TimesheetEntry timesheetEntry)
        {
            if (id != timesheetEntry.TimesheetEntryId)
            {
                return NotFound();
            }
            var authorizationResult = await _authorizationService
                .AuthorizeAsync(User, timesheetEntry, "SameTimesheetEntryCreator");
            if (authorizationResult.Succeeded && timesheetEntry.HoursWorked > 0 )
            {
                try
                {
                    var oldTimesheetEntry = _context.TimesheetEntries.SingleOrDefault(x => x.TimesheetEntryId == id);
                    oldTimesheetEntry.HoursWorked = timesheetEntry.HoursWorked;
                    _context.Update(oldTimesheetEntry);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TimesheetEntryExists(timesheetEntry.TimesheetEntryId))
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
            ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "ProjectId", timesheetEntry.ProjectId);
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", timesheetEntry.UserId);
            return View(timesheetEntry);
        }

        // GET: TimesheetEntries/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var timesheetEntry = await _context.TimesheetEntries
                .Include(t => t.Project)
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.TimesheetEntryId == id);
            if (timesheetEntry == null)
            {
                return NotFound();
            }
            var authorizationResult = await _authorizationService
                .AuthorizeAsync(User, timesheetEntry, "SameTimesheetEntryCreator");
            if (authorizationResult.Succeeded)
            {
                return View(timesheetEntry);
            }
            else if (User.Identity.IsAuthenticated)
            {
                return new ForbidResult();
            }
            else
            {
                return new ChallengeResult();
            }
        }

        // POST: TimesheetEntries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var timesheetEntry = await _context.TimesheetEntries.FindAsync(id);
            var authorizationResult = await _authorizationService
                   .AuthorizeAsync(User, timesheetEntry, "SameTimesheetEntryCreator");
            if (authorizationResult.Succeeded)
            {
                _context.TimesheetEntries.Remove(timesheetEntry);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        private bool TimesheetEntryExists(long id)
        {
            return _context.TimesheetEntries.Any(e => e.TimesheetEntryId == id);
        }
    }
}
