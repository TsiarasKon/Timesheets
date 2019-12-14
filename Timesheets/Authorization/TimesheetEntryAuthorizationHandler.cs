using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Timesheets.Areas.Identity.Data;
using Timesheets.Models;

namespace Timesheets.Authorization
{
    public class TimesheetEntryAuthorizationHandler : AuthorizationHandler<SameCreatorRequirement, TimesheetEntry>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public TimesheetEntryAuthorizationHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                   SameCreatorRequirement requirement,
                                                   TimesheetEntry resource)
        {

            if (context.User == null || resource == null)
            {
                return Task.CompletedTask;
            }

            // TODO: check if admin/manager
            if (_userManager.GetUserId(context.User) == resource.User.Id)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }

    public class SameCreatorRequirement : IAuthorizationRequirement { }
}
