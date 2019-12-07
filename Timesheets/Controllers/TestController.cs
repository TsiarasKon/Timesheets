using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Timesheets.Data;
using Timesheets.Models;

namespace Timesheets.Controllers
{
    public class TestController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TestController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            TestIndexViewModel model = new TestIndexViewModel
            {
                Projects = _context.Projects.ToList(),
                Departments = _context.Departments.ToList(),
                DepartmentProjects = _context.DepartmentProjects.ToList()
            };
            return View(model);
        }

        public IActionResult Test1()
        {
            return Json(_context.Projects.ToList());        // just an example
        }
    }
}