using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Timesheets.Areas.Identity.Data;
using Timesheets.Data;
using Timesheets.Models;

namespace Timesheets.ViewModels
{
    public class TimesheetsViewModel
    {
        public IEnumerable<TimesheetEntry> Timesheets { get; set; }
        public IEnumerable<ApplicationUser> Users { get; set; }
        public IEnumerable<Project> Projects { get; set; }
    }
}
