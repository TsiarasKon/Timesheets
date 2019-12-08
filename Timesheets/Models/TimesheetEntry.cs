using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Timesheets.Areas.Identity.Data;

namespace Timesheets.Models
{
    public class TimesheetEntry
    {
        public long TimesheetEntryId { get; set; }
        public DateTime DateCreated { get; set; }
        public int HoursWorked { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public long ProjectId { get; set; }
        public Project Project { get; set; }
    }
}
