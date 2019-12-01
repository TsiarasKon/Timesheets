using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Timesheets.Models
{
    public class DepartmentProject
    {
        public long DepartmentId { get; set; }
        public Department Department { get; set; }

        public long ProjectId { get; set; }
        public Project Project { get; set; }
    }
}
