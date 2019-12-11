using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Timesheets.Areas.Identity.Data;
using Timesheets.Data;
using Timesheets.Models;

namespace Timesheets.ViewModels
{
    public class CreateTimesheetEntryViewModel: TimesheetEntry
    {
         private readonly ApplicationDbContext _context;

         public CreateTimesheetEntryViewModel(ApplicationDbContext context)
         {
                _context = context;
         }

        public List<SelectListItem> ProjectList { get; set; }
        public List<SelectListItem> UserList { get; set; }
    }
}
