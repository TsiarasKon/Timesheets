using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Timesheets.Data;
using Timesheets.Models;

namespace Timesheets.Controllers
{
    [Authorize(Roles = "Administrator, Manager")]
    public class ProjectsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProjectsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Projects
        public IActionResult Index(string sortOrder, string searchString)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return new ChallengeResult();
            }

            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.OwnerDepartmentSortParm = sortOrder == "deptOwner_asc" ? "deptOwner_desc" : "deptOwner_asc";

            IEnumerable<Project> projectList = _context.Projects.Include(p => p.OwnerDepartment);
            if (String.IsNullOrEmpty(searchString))
            {
                searchString = ViewBag.SearchString;
            } else
            {
                ViewBag.SearchString = searchString;
            }
            if (!String.IsNullOrEmpty(searchString))
            {
                projectList = projectList.Where(p => p.Name.Contains(searchString, StringComparison.CurrentCultureIgnoreCase)
                                       || p.OwnerDepartment.Name.Contains(searchString, StringComparison.CurrentCultureIgnoreCase));
            }
            switch (sortOrder)
            {
                case "deptOwner_asc":
                    projectList = projectList.OrderBy(p => p.OwnerDepartment.Name);
                    break;
                case "deptOwner_desc":
                    projectList = projectList.OrderByDescending(p => p.OwnerDepartment.Name);
                    break;
                case "name_desc":
                    projectList = projectList.OrderByDescending(p => p.Name);
                    break;
                default:
                    projectList = projectList.OrderBy(p => p.Name);
                    break;
            }
            return View(projectList.ToList());
        }

        // GET: Projects/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .FirstOrDefaultAsync(m => m.ProjectId == id);
            if (project == null)
            {
                return NotFound();
            }

            ProjectDetailsViewModel model = new ProjectDetailsViewModel
            {
                Project = project,
                OwnerDepartment = _context.Departments.FirstOrDefault(d => d.DepartmentId == project.OwnerDepartmentId),
                RelatedDepartments = _context.Departments.Where(d => d.RelatedProjects.Any(p => p.ProjectId == project.ProjectId)).ToList()
               
            };

            return View(model);
        }

        // GET: Projects/Create
        public IActionResult Create()
        {
            ViewBag.Departments = new SelectList(_context.Departments
                .Select(d => new { d.Name, d.DepartmentId })
                , "DepartmentId", "Name");
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProjectId,Name,OwnerDepartmentId")] Project project)
        {
            if (ModelState.IsValid)
            {
                _context.Add(project);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Departments = new SelectList(_context.Departments
                .Select(d => new { d.Name, d.DepartmentId })
                , "DepartmentId", "Name"); 
            return View(project);
        }

        // GET: Projects/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            ViewBag.Departments = new SelectList(_context.Departments
                .Select(d => new { d.Name, d.DepartmentId })
                , "DepartmentId", "Name");
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("ProjectId,Name,OwnerDepartmentId")] Project project)
        {
            if (id != project.ProjectId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(project);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(project.ProjectId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Departments = new SelectList(_context.Departments
                .Select(d => new { d.Name, d.DepartmentId })
                , "DepartmentId", "Name"); 
            return View(project);
        }

        // GET: Projects/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .Include(p => p.OwnerDepartment)
                .FirstOrDefaultAsync(m => m.ProjectId == id);
            ViewData["ExistingProjects"] = (_context.TimesheetEntries.Where(t => t.ProjectId == id).ToList().Count > 0);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var project = await _context.Projects.FindAsync(id);
            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectExists(long id)
        {
            return _context.Projects.Any(e => e.ProjectId == id);
        }
    }
}
