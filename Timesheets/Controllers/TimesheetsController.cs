using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Timesheets.Data;
using Timesheets.Models;

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
                Timesheets = _context.Timesheets.ToList(),
                Users = _context.ApplicationUsers.ToList(),
                Projects = _context.Projects.ToList()
            };
            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(TimesheetDTO timesheetDTO)
        {
            // IHttpActionResult 

            //if (!ModelState.IsValid)
            //    return BadRequest("Invalid data");

            return Ok();
        }
    }
}