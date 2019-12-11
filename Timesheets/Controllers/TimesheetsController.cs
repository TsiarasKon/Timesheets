using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Timesheets.Data;
using Timesheets.Models;
using Timesheets.ViewModels;

namespace Timesheets.Controllers
{
    public class TimesheetsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TimesheetsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // TODO: check current user and return Timesheets accordingly
            TimesheetsViewModel model = new TimesheetsViewModel
            {
                Timesheets = _context.TimesheetEntries.ToList(),
                Users = _context.ApplicationUsers.ToList(),
                Projects = _context.Projects.ToList()
            };
            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            //ViewBag.ProjectList = _context.Projects
            //    .OrderBy(u => u.Name)
            //    .Select(u => new SelectListItem { Text = u.Name, Value = u.ProjectId.ToString() }).ToList();
            CreateTimesheetEntryViewModel c = new CreateTimesheetEntryViewModel(_context);
            c.ProjectList = _context.Projects
                .OrderBy(u => u.Name)
                .Select(u => new SelectListItem { Text = u.Name, Value = u.ProjectId.ToString() }).ToList();

            c.UserList = _context.ApplicationUsers
                .Select(u => new SelectListItem { Text = String.Format("{0} {1}", u.FirstName, u.LastName), Value = u.Id.ToString() }).ToList();
            return View(c);
        }

        [HttpPost]
        public IActionResult Create(TimesheetEntry data)
        {
            if (!ModelState.IsValid)
                return View();

            _context.TimesheetEntries.Add(data);
            _context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
    }
}