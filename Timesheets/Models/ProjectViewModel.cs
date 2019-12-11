using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Timesheets.Data;

namespace Timesheets.Models
{
    public class ProjectViewModel
    {
        public IEnumerable<Project> Projects { get; set; }
        public IEnumerable<Department> Departments { get; set; }
        
    }
}
