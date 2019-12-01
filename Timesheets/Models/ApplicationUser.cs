using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Timesheets.Models
{
    public class ApplicationUser
    {
        // TODO: use AspNetUsers instead

        public long ApplicationUserId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public double ManHourCost { get; set; }

        public long DepartmentId { get; set; }
        public Department Department { get; set; }

        public long? ManagerId { get; set; }
        public ApplicationUser Manager { get; set; }
    }
}
