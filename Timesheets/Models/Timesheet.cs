using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Timesheets.Models
{
    public class Timesheet
    {
        public long TimesheetId { get; set; }
        public DateTime DateCreated { get; set; }
        public int HoursWorked { get; set; }

        public long UserId { get; set; }
        public ApplicationUser User { get; set; }

        public long ProjectId { get; set; }
        public Project Project { get; set; }
    }
}
