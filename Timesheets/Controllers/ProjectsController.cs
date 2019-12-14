using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Timesheets.Data;
using Timesheets.Models;

namespace Timesheets.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProjectsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Projects
        public IActionResult Index()
        {
            ProjectViewModel model = new ProjectViewModel
            {
                Projects = _context.Projects.ToList(),
                Departments = _context.Departments.ToList()
            };
            return View(model);
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
            ViewData["OwnerDepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentId", project.OwnerDepartmentId);
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
            ViewData["OwnerDepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentId", project.OwnerDepartmentId);
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
                .Include(p => p.TimesheetEntries)
                .SingleOrDefaultAsync(m => m.ProjectId == id);
            if (project == null)
            {
                return NotFound();
            }
            if(project.TimesheetEntries.Count != 0)
            {
                //ViewBag.Message = string.Format("That project that you going to remove contains timesheet");
                return View("NoDelete");
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
