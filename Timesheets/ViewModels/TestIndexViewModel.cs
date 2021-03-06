﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Timesheets.Models;

namespace Timesheets.ViewModels
{
    public class TestIndexViewModel
    {
        public IEnumerable<Project> Projects { get; set; }
        public IEnumerable<Department> Departments { get; set; }
        public IEnumerable<DepartmentProject> DepartmentProjects { get; set; }
    }
}
