using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Timesheets.Areas.Identity.Data;

namespace Timesheets.Models
{
    public class DepartmentsViewModel
    {
        public IEnumerable<Department> Departments { get; set; }
        public IEnumerable<ApplicationUser> Users { get; set; }
    }
}
