using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Timesheets.Areas.Identity.Data;

namespace Timesheets.Models
{
    public class Department
    {
        public long DepartmentId { get; set; }
        public string Name { get; set; }

        public string? DepartmentHeadId { get; set; }
        public ApplicationUser DepartmentHead { get; set; }

        public ICollection<ApplicationUser> Users { get; set; }

        public ICollection<DepartmentProject> RelatedProjects { get; set; }

        public ICollection<Project> OwnedProjects { get; set; }
    }
}
