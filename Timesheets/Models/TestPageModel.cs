using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Timesheets.Models
{
    public class TestPageModel
    {
        public IEnumerable<Project> Projects { get; set; }
        public IEnumerable<Department> Departments { get; set; }
        public IEnumerable<DepartmentProject> DepartmentProjects { get; set; }
    }
}
