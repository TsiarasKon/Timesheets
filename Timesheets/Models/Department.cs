using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Timesheets.Models
{
    public class Department
    {
        public long DepartmentId { get; set; }
        public string Name { get; set; }

        public long DepartmentHeadId { get; set; }
        public ApplicationUser DepartmentHead { get; set; }


        public ICollection<DepartmentProject> RelatedProjects { get; set; }

        public ICollection<Project> OwnedProjects { get; set; }
    }
}
