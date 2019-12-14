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

        public static string[] usernames = { 
            "petroula@test.nl", "sofia@test.nl", "nikos@test.nl", "mpampis@test.nl", "antonis@test.nl", "admin@test.nl" };

        public static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            string[] roleNames = { "Administrator", "Manager", "Employee" };
            foreach (var roleName in roleNames)
            {
                var roleExist = roleManager.RoleExistsAsync(roleName);
                if (!roleExist.Result)
                {
                    roleManager.CreateAsync(new IdentityRole(roleName)).Wait();
                }
            }
        }

        public static void SeedUsers(UserManager<ApplicationUser> userManager)
        {
            if (userManager.FindByNameAsync(usernames[0]).Result == null)
            {
                ApplicationUser user = new ApplicationUser()
                {
                    UserName = usernames[0],
                    Email = usernames[0],
                    FirstName = "Petroula",
                    LastName = "Stamouli",
                    ManHourCost = 3.14,
                    EmailConfirmed = true
                };

                IdentityResult result = userManager.CreateAsync(user, "12345^qW").Result;
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Employee").Wait();
                }
            }

            if (userManager.FindByNameAsync(usernames[1]).Result == null)
            {
                ApplicationUser user = new ApplicationUser()
                {
                    UserName = usernames[1],
                    Email = usernames[1],
                    FirstName = "Sofia",
                    LastName = "Tseranidou",
                    ManHourCost = 5,
                    EmailConfirmed = true
                };

                IdentityResult result = userManager.CreateAsync(user, "12345^qW").Result;
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Employee").Wait();
                }
            }

            if (userManager.FindByNameAsync(usernames[2]).Result == null)
            {
                ApplicationUser user = new ApplicationUser()
                {
                    UserName = usernames[2],
                    Email = usernames[2],
                    FirstName = "Nikos",
                    LastName = "Stavrou",
                    ManHourCost = 4,
                    EmailConfirmed = true
                };

                IdentityResult result = userManager.CreateAsync(user, "12345^qW").Result;
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Employee").Wait();
                }
            }

            if (userManager.FindByNameAsync(usernames[3]).Result == null)
            {
                ApplicationUser user = new ApplicationUser()
                {
                    UserName = usernames[3],
                    Email = usernames[3],
                    FirstName = "Mpampis",
                    LastName = "Sougias",
                    ManHourCost = 4.56,
                    EmailConfirmed = true
                };

                IdentityResult result = userManager.CreateAsync(user, "12345^qW").Result;
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Manager").Wait();
                }
            }

            if (userManager.FindByNameAsync(usernames[4]).Result == null)
            {
                ApplicationUser user = new ApplicationUser()
                {
                    UserName = usernames[4],
                    Email = usernames[4],
                    FirstName = "Antonis",
                    LastName = "Fragkiadakis",
                    ManHourCost = 2,
                    EmailConfirmed = true
                };

                IdentityResult result = userManager.CreateAsync(user, "12345^qW").Result;
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Manager").Wait();
                }
            }

            if (userManager.FindByNameAsync(usernames[5]).Result == null)
            {
                ApplicationUser user = new ApplicationUser()
                {
                    UserName = usernames[5],
                    Email = usernames[5],
                    FirstName = "Admin",
                    LastName = "Adminakis",
                    ManHourCost = 9,
                    EmailConfirmed = true
                };

                IdentityResult result = userManager.CreateAsync(user, "12345^qW").Result;
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Administrator").Wait();
                }
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
                    new Department() { Name = "Banking", DepartmentHeadId = usersIdList[0] },
                    new Department() { Name = "Infrastructure", DepartmentHeadId = usersIdList[2] },
                    new Department() { Name = "Networking", DepartmentHeadId = usersIdList[1] },
                    new Department() { Name = "Telecommunications", DepartmentHeadId = usersIdList[3] },
                    new Department() { Name = "Finance", DepartmentHeadId = usersIdList[4] }

                };
                context.Departments.AddRange(newDepartments);
                context.SaveChanges();

                var newDepartmentsIdList = context.Departments.Select(x => x.DepartmentId).Distinct().ToList();

                IList<Project> newProjects = new List<Project>()
                {
                    new Project() { Name = "Android App For New Product", OwnerDepartmentId = newDepartmentsIdList[0] },
                    new Project() { Name = "New Website", OwnerDepartmentId = newDepartmentsIdList[1] },
                    new Project() { Name = "New database", OwnerDepartmentId = newDepartmentsIdList[0] },
                    new Project() { Name = "Create a website for the product.", OwnerDepartmentId = newDepartmentsIdList[2] },
                    new Project() { Name = "Integrate sql db.", OwnerDepartmentId = newDepartmentsIdList[1] }

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
                    new TimesheetEntry() { DateCreated = DateTime.Now, HoursWorked = 3, UserId = usersIdList[0], ProjectId = newProjectsIdList[1] },
                    new TimesheetEntry() { DateCreated = DateTime.Now, HoursWorked = 8, UserId = usersIdList[2], ProjectId = newProjectsIdList[3] },
                    new TimesheetEntry() { DateCreated = DateTime.Now, HoursWorked = 10,UserId = usersIdList[1], ProjectId = newProjectsIdList[2] },
                    new TimesheetEntry() { DateCreated = DateTime.Now, HoursWorked = 4,UserId = usersIdList[2], ProjectId = newProjectsIdList[3] },
                    new TimesheetEntry() { DateCreated = DateTime.Now, HoursWorked = 9,UserId = usersIdList[1], ProjectId = newProjectsIdList[1] },
                    new TimesheetEntry() { DateCreated = DateTime.Now, HoursWorked = 12,UserId = usersIdList[1], ProjectId = newProjectsIdList[2] },
                    new TimesheetEntry() { DateCreated = DateTime.Now, HoursWorked = 13,UserId = usersIdList[0], ProjectId = newProjectsIdList[1] },
                    new TimesheetEntry() { DateCreated = DateTime.Now, HoursWorked = 14,UserId = usersIdList[0], ProjectId = newProjectsIdList[2] },
                    new TimesheetEntry() { DateCreated = DateTime.Now, HoursWorked = 20,UserId = usersIdList[1], ProjectId = newProjectsIdList[3] }
                };
                context.TimesheetEntries.AddRange(newTimesheetEntries);
              
                context.SaveChanges();

                var users = context.ApplicationUsers.ToList();
                users[0].DepartmentId = newDepartmentsIdList[0];
                users[0].ManagerId = usersIdList[3];
                users[1].DepartmentId = newDepartmentsIdList[0];
                users[1].ManagerId = usersIdList[3];
                users[2].DepartmentId = newDepartmentsIdList[2];
                users[2].ManagerId = usersIdList[4];
                users[3].DepartmentId = newDepartmentsIdList[3];
                users[3].ManagerId = usersIdList[3];
                users[4].DepartmentId = newDepartmentsIdList[3];
                users[4].ManagerId = usersIdList[4];
                users[5].DepartmentId = newDepartmentsIdList[3];
                users[5].ManagerId = usersIdList[5];
                context.UpdateRange(users);
                context.SaveChanges();

            }
        }
    }
}
