using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Timesheets.Areas.Identity.Data;
using Timesheets.Data;

namespace Timesheets.Controllers
{
    public class ChartsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ChartsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        /*        [HttpGet]
                public IActionResult GetProjectsPerTime()
                {
                    var costPerDept = _context.Departments
                        .OrderByDescending(d => d.Name)
                        .Select(d => new
                        {
                            Name = d.Name,
                            Users = d.Users.AsQueryable().Sum(h => h.ManHourCost)
                        }).ToList();


                    return Json(costPerDept);
                }*/

        [HttpGet]
        public IActionResult GetProjectsPerTime()
        {
            var costPerDept = _context.Departments
                .AsEnumerable()
                .GroupBy(x => x.Name)
                .Select(g => new
                {
                    Name = g.Key,
                    Total = g.Sum(u => u.Users.Sum(c => c.ManHourCost))

                }).ToList();

            return Json(costPerDept);
        }

        [HttpGet]
        public IActionResult GetUserPerTimeSheet()
        {
            var result = from timesheetEntry in _context.TimesheetEntries
                         group timesheetEntry by timesheetEntry.User.UserName into uGroup
                         select new
                         {
                             User = uGroup.Key,
                             TotalHours = uGroup.Sum(x => x.HoursWorked)
                         };
            return Json(result);
        }
    }

}
