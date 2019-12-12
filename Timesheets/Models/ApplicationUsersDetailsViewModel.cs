using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Timesheets.Areas.Identity.Data;

namespace Timesheets.Models
{
    public class ApplicationUsersDetailsViewModel
    {
        
        public ApplicationUser ApplicationUser { get; set; }
        public ApplicationUser Manager { get; set; }
        public IEnumerable<TimesheetEntry> TimesheetEntries { get; set; }


    }
}
