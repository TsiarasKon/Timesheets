using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Timesheets.Areas.Identity.Data;

namespace Timesheets.Models
{
    public class Department
    {
        public long DepartmentId { get; set; }
        [Required]
        public string Name { get; set; }

        [Required]
        public string DepartmentHeadId { get; set; }

        [Display(Name = "Department Head")]
        public ApplicationUser DepartmentHead { get; set; }

        public ICollection<ApplicationUser> Users { get; set; }

        [Display(Name = "Related Projects")]
        public ICollection<DepartmentProject> RelatedProjects { get; set; }

        [Display(Name = "Owned Projects")]
        public ICollection<Project> OwnedProjects { get; set; }
    }
}
