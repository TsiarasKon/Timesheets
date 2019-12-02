using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Timesheets.Data;
using Timesheets.Models;

namespace Timesheets.Controllers
{
    public class TestController : Controller
    {

        private ApplicationDbContext _context;

        public TestController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            TestPageModel model = new TestPageModel();
            model.Projects = _context.Projects.ToList();
            model.Departments = _context.Departments.ToList();
            model.DepartmentProjects = _context.DepartmentProjects.ToList();
            return View(model);
        }
    }
}