using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Timesheets.Areas.Identity.Data;
using Timesheets.Data;
using Timesheets.Models;

namespace Timesheets.Controllers
{
    [Authorize(Roles = "Administrator, Manager, Employee")]
    public class TimesheetEntriesController : Controller
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly ApplicationDbContext _context;

        public TimesheetEntriesController(IAuthorizationService authorizationService, ApplicationDbContext context)
        {
            _authorizationService = authorizationService;
            _context = context;
        }

        // GET: TimesheetEntries
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            if (! User.Identity.IsAuthenticated)
            {
                return new ChallengeResult();
            }

            ViewBag.DateSortParm = String.IsNullOrEmpty(sortOrder) ? "date_desc" : "";
            ViewBag.HoursWorkedSortParm = sortOrder == "hoursWorked_asc" ? "hoursWorked_desc" : "hoursWorked_asc";
            ViewBag.UserNameSortParm = sortOrder == "userName_asc" ? "userName_desc" : "userName_asc";
            ViewBag.ProjectSortParm = sortOrder == "project_asc" ? "project_desc" : "project_asc";

            IEnumerable<TimesheetEntry> timesheetList;
            if (User.IsInRole("Administrator"))
            {
                timesheetList = _context.TimesheetEntries.
                Include(t => t.Project).Include(t => t.User);
            }
            else if (User.IsInRole("Manager"))
            {
                timesheetList = _context.TimesheetEntries.
                Include(t => t.Project).Include(t => t.User).
                Where(t => t.UserId == User.FindFirst(ClaimTypes.NameIdentifier).Value || t.User.ManagerId == User.FindFirst(ClaimTypes.NameIdentifier).Value);
            }
            else
            {
                timesheetList = _context.TimesheetEntries.
                Where(t => t.UserId == User.FindFirst(ClaimTypes.NameIdentifier).Value).
                Include(t => t.Project).Include(t => t.User);
            }

            //IEnumerable<TimesheetEntry> timesheetList = _context.TimesheetEntries.
            //    Include(t => t.Project).Include(t => t.User);
            //timesheetList = timesheetList.ToList().RemoveAll(TimesheetEntryAuthorizedAsync);
            //IEnumerable<TimesheetEntry> timesheetList = Enumerable.Empty<TimesheetEntry>();
            //foreach (var entry in timesheetListUnverified)
            //{
            //    var authorizationResult = await _authorizationService
            //    .AuthorizeAsync(User, entry, "SameTimesheetEntryCreator");
            //    if (authorizationResult.Succeeded)
            //    {
            //        timesheetList.Append(entry);
            //    }
            //}

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
                timesheetList = timesheetList.Where(t => t.Project.Name.Contains(searchString, StringComparison.CurrentCultureIgnoreCase)
                                       || t.User.FirstName.Contains(searchString, StringComparison.CurrentCultureIgnoreCase)
                                       || t.User.LastName.Contains(searchString, StringComparison.CurrentCultureIgnoreCase));
            }
            switch (sortOrder)
            {
                case "hoursWorked_asc":
                    timesheetList = timesheetList.OrderBy(t => t.HoursWorked);
                    break;
                case "hoursWorked_desc":
                    timesheetList = timesheetList.OrderByDescending(t => t.HoursWorked);
                    break;
                case "userName_asc":
                    timesheetList = timesheetList.OrderBy(t => t.User.FirstName);
                    break;
                case "userName_desc":
                    timesheetList = timesheetList.OrderByDescending(t => t.User.FirstName);
                    break;
                case "project_asc":
                    timesheetList = timesheetList.OrderBy(t => t.Project.Name);
                    break;
                case "project_desc":
                    timesheetList = timesheetList.OrderByDescending(t => t.Project.Name);
                    break;
                case "date_desc":
                    timesheetList = timesheetList.OrderByDescending(t => t.DateCreated);
                    break;
                default:
                    timesheetList = timesheetList.OrderBy(t => t.DateCreated);
                    break;
            }
            return View(timesheetList.ToList());
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

        // GET: TimesheetEntries/Create
        public IActionResult Create()
        {
            ViewBag.Projects = new SelectList(_context.Projects, "ProjectId", "Name");
            ViewBag.ApplicationUsers = new SelectList(_context.ApplicationUsers
               .Select(u => new { FullName = String.Format("{0} {1}", u.FirstName, u.LastName), u.Id })
               , "Id", "FullName");
            ViewData["CurrentUserId"] = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return View();
        }

        // POST: TimesheetEntries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( TimesheetEntry timesheetEntry)
        {
            if (User.IsInRole("Employee"))
            {
                timesheetEntry.UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            }
            timesheetEntry.User = _context.ApplicationUsers.SingleOrDefault(u => u.Id == timesheetEntry.UserId);

            var justCheck = _context.TimesheetEntries
            .Where(t => t.DateCreated.Date == timesheetEntry.DateCreated.Date && t.ProjectId == timesheetEntry.ProjectId).FirstOrDefault();

            if (ModelState.IsValid && justCheck == null && timesheetEntry.HoursWorked>0)
            {
                _context.Add(timesheetEntry);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

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
                ViewData["CurrentUserId"] = User.FindFirst(ClaimTypes.NameIdentifier).Value;
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
            if (User.IsInRole("Employee"))
            {
                timesheetEntry.UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            }
            timesheetEntry.User = _context.ApplicationUsers.SingleOrDefault(u => u.Id == timesheetEntry.UserId);

            var authorizationResult = await _authorizationService
                .AuthorizeAsync(User, timesheetEntry, "SameTimesheetEntryCreator");
            if (authorizationResult.Succeeded && ModelState.IsValid)
            {
                try
                {
                    //var oldTimesheetEntry = _context.TimesheetEntries.SingleOrDefault(x => x.TimesheetEntryId == id);
                    //oldTimesheetEntry.HoursWorked = timesheetEntry.HoursWorked;
                    //_context.Update(oldTimesheetEntry);
                    _context.Update(timesheetEntry);
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
            ViewBag.Projects = new SelectList(_context.Projects, "ProjectId", "Name");
            ViewBag.ApplicationUsers = new SelectList(_context.ApplicationUsers
               .Select(u => new { FullName = String.Format("{0} {1}", u.FirstName, u.LastName), u.Id })
               , "Id", "FullName");
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

        private bool TimesheetEntryAuthorizedAsync(TimesheetEntry entry)
        {
            var authorizationResult = _authorizationService.AuthorizeAsync(User, entry, "SameTimesheetEntryCreator").Result;
            return authorizationResult.Succeeded;
        }
    }
}
