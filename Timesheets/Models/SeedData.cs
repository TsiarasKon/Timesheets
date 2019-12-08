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
        // and: http://www.binaryintellect.net/articles/5e180dfa-4438-45d8-ac78-c7cc11735791.aspx

        public static void SeedUsers(UserManager<ApplicationUser> userManager)
        {
            var username1 = "a@a.a";
            if (userManager.FindByNameAsync(username1).Result == null)
            {
                ApplicationUser user = new ApplicationUser(){
                    UserName = username1,
                    Email = username1,
                    FirstName = "That",
                    LastName = "Guy",
                    ManHourCost = 3.14,
                    EmailConfirmed = true
                };

                IdentityResult result = userManager.CreateAsync(user, "12345^qW").Result;
                //if (result.Succeeded)
                //{
                //    userManager.AddToRoleAsync(user, "NormalUser").Wait();
                //}
            }
            var username2 = "b@b.b";
            if (userManager.FindByNameAsync(username2).Result == null)
            {
                ApplicationUser user = new ApplicationUser()
                {
                    UserName = username2,
                    Email = username2,
                    FirstName = "Other",
                    LastName = "Guy",
                    ManHourCost = 5,
                    EmailConfirmed = true
                };

                IdentityResult result = userManager.CreateAsync(user, "12345^qW").Result;
                //if (result.Succeeded)
                //{
                //    userManager.AddToRoleAsync(user, "NormalUser").Wait();
                //}
            }
            var username3 = "c@c.c";
            if (userManager.FindByNameAsync(username3).Result == null)
            {
                ApplicationUser user = new ApplicationUser()
                {
                    UserName = username3,
                    Email = username3,
                    FirstName = "C",
                    LastName = "CC",
                    ManHourCost = 4,
                    EmailConfirmed = true
                };

                IdentityResult result = userManager.CreateAsync(user, "12345^qW").Result;
                //if (result.Succeeded)
                //{
                //    userManager.AddToRoleAsync(user, "NormalUser").Wait();
                //}
            }
        }

        public static void SeedRest(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<ApplicationDbContext>>()))
            {
                if (context.Departments.Any())
                {
                    return;   // DB has been seeded
                }

                var usersIdList = context.ApplicationUsers.Select(x => x.Id).Distinct().ToList();

                IList<Department> newDepartments = new List<Department>() 
                {
                    new Department() { Name = "Dept1", DepartmentHeadId = usersIdList[0] },
                    new Department() { Name = "Dept2", DepartmentHeadId = usersIdList[2] },
                    new Department() { Name = "Dept3", DepartmentHeadId = usersIdList[1] }
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

                IList<TimesheetEntry> newTimesheetEntries = new List<TimesheetEntry>()
                {
                    new TimesheetEntry() { DateCreated = DateTime.Now, HoursWorked = 7, UserId = usersIdList[0], ProjectId = newProjectsIdList[0] },
                    new TimesheetEntry() { DateCreated = DateTime.Now, HoursWorked = 3, UserId = usersIdList[0], ProjectId = newProjectsIdList[1] }
                };
                context.TimesheetEntries.AddRange(newTimesheetEntries);
                context.SaveChanges();

                // TODO: insert more data; update user DepartmentIds
            }
        }
    }
}
