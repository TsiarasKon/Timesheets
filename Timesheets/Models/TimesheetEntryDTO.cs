using System;

namespace Timesheets.Models
{
    public class TimesheetEntryDTO
    {
        public string UserId { get; set; }
        public long ProjectId { get; set; }
        public DateTime DateCreated { get; internal set; }
        public int HoursWorked { get; internal set; }
    }
}