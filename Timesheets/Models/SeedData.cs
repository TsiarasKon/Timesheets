using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using Timesheets.Areas.Identity.Data;
using Timesheets.Data;

namespace Timesheets.Models
{
    public class SeedData
    {
        // check: https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-mvc-app/working-with-sql?view=aspnetcore-3.0&tabs=visual-studio

        //private UserManager<IdentityUser> _userManager;

        //public SeedData(UserManager<IdentityUser> userManager)
        //{
        //    _userManager = userManager;
        //}

        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<ApplicationDbContext>>()))
            {

                if (context.Departments.Any())
                {
                    return;   // DB has been seeded
                }

                IList<Department> newDepartments = new List<Department>() 
                {
                    new Department() { Name = "Dept1" },
                    new Department() { Name = "Dept2" },
                    new Department() { Name = "Dept3" }
                };
                context.Departments.AddRange(newDepartments);
                context.SaveChanges();

                var newDepartmentsIdList = context.Departments.Select(x => x.DepartmentId).Distinct().ToList();

                IList<Project> newProjects = new List<Project>()
                {
                    new Project() { Name = "Proj1", OwnerDepartmentId = newDepartmentsIdList[0] },
                    new Project() { Name = "Proj2", OwnerDepartmentId = newDepartmentsIdList[1] },
                    new Project() { Name = "Proj3", OwnerDepartmentId = newDepartmentsIdList[0] },
                    new Project() { Name = "Proj4", OwnerDepartmentId = newDepartmentsIdList[2] }
                };
                context.Projects.AddRange(newProjects);
                context.SaveChanges();

                var newProjectsIdList = context.Projects.Select(x => x.ProjectId).Distinct().ToList();

                IList<DepartmentProject> newDPs = new List<DepartmentProject>()
                {
                    new DepartmentProject() { DepartmentId = newDepartmentsIdList[0], ProjectId = newProjectsIdList[0] },
                    new DepartmentProject() { DepartmentId = newDepartmentsIdList[1], ProjectId = newProjectsIdList[0] },
                    new DepartmentProject() { DepartmentId = newDepartmentsIdList[1], ProjectId = newProjectsIdList[1] },
                    new DepartmentProject() { DepartmentId = newDepartmentsIdList[0], ProjectId = newProjectsIdList[2] },
                    new DepartmentProject() { DepartmentId = newDepartmentsIdList[1], ProjectId = newProjectsIdList[2] },
                    new DepartmentProject() { DepartmentId = newDepartmentsIdList[2], ProjectId = newProjectsIdList[2] },
                    new DepartmentProject() { DepartmentId = newDepartmentsIdList[0], ProjectId = newProjectsIdList[3] },
                    new DepartmentProject() { DepartmentId = newDepartmentsIdList[2], ProjectId = newProjectsIdList[3] }
                };
                context.DepartmentProjects.AddRange(newDPs);
                context.SaveChanges();

                // TODO: WIP
                //var user = new ApplicationUser
                //{
                //    UserName = "a@a.a",
                //    Email = "a@a.a",
                //    FirstName = "That",
                //    LastName = "Guy",
                //    DepartmentId = newDepartmentsIdList[0],
                //    ManHourCost = 3.14
                //};
                //_userManager.CreateAsync(user, "12345^qW");
                //context.SaveChanges();

                //var usersIdList = context.ApplicationUsers.Select(x => x.Id).Distinct().ToList();

                //IList<Timesheet> newTimesheets = new List<Timesheet>()
                //{
                //    new Timesheet() { DateCreated = DateTime.Now, HoursWorked = 7, UserId = usersIdList[0], ProjectId = newProjectsIdList[0] },
                //    new Timesheet() { DateCreated = DateTime.Now, HoursWorked = 3, UserId = usersIdList[0], ProjectId = newProjectsIdList[1] }
                //};
                //context.Timesheets.AddRange(newTimesheets);
                //context.SaveChanges();

                // TODO: insert more data; update DepartmentHeadIds here, after inserting ApplicationUsers
            }
        }
    }
}
