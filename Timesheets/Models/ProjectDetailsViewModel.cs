using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Timesheets.Models
{
    public class ProjectDetailsViewModel
    {
        public Project Project { get; set; }
        public Department OwnerDepartment { get; set; }
        public IEnumerable<Department> RelatedDepartments { get; set; }
    }
}
