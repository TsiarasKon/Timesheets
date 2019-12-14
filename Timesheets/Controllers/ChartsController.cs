using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Timesheets.Areas.Identity.Data;
using Timesheets.Data;

namespace Timesheets.Controllers
{
    public class Stats
    {
        public string ProjectName { get; set; }
        public double ProjectCost { get; set; }
    }

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

        [HttpGet]
        public ActionResult GetProjectsPerTime()
        {
            List<Stats> results = new List<Stats>();

            string query = @"select p.Name as ProjectName, sum(ts.HoursWorked * u.ManHourCost) as ProjectCost
                            from TimesheetEntries ts
                            inner
                            join Projects p on ts.ProjectId = p.ProjectId
                            inner
                            join AspNetUsers u on ts.UserId = u.Id
                            group by p.Name";

            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = query;
                _context.Database.OpenConnection();
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    results.Add(new Stats { ProjectName = reader.GetString(0), ProjectCost = reader.GetDouble(1) });
                }
            }


            return Json(results.ToList());

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
