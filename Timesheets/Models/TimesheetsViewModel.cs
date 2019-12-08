using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Timesheets.Areas.Identity.Data;

namespace Timesheets.Models
{
    public class TimesheetsViewModel
    {
        public IEnumerable<TimesheetEntry> Timesheets { get; set; }
        public IEnumerable<ApplicationUser> Users { get; set; }
        public IEnumerable<Project> Projects { get; set; }
    }
}
