using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Timesheets.Models
{
    public class Project
    {
        public long ProjectId { get; set; }
        public string Name { get; set; }
        public ICollection<DepartmentProject> RelatedDepartments { get; set; }
        public long OwnerDepartmentId { get; set; }
        [Display(Name = "Owner Department")]
        public Department OwnerDepartment { get; set; }
       // public ICollection<TimesheetEntry> TimeSheetEntries { get; internal set; }
    }
}