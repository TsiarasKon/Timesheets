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
    public class DepartmentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DepartmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Departments
        public IActionResult Index()
        {
            DepartmentsViewModel model = new DepartmentsViewModel
            {
                Departments = _context.Departments.ToList(),
                Users = _context.ApplicationUsers.ToList()
            };
            return View(model);
        }

        // GET: Departments/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments
                .FirstOrDefaultAsync(m => m.DepartmentId == id);
            if (department == null)
            {
                return NotFound();
            }
            DepartmentDetailsViewModel model = new DepartmentDetailsViewModel
            {
                Department = department,
                DepartmentHead = _context.ApplicationUsers.FirstOrDefault(u => u.Id == department.DepartmentHeadId),
                OwnedProjects = _context.Projects.Where(p => p.OwnerDepartmentId == department.DepartmentId).ToList(),
                RelatedProjects = _context.Projects.Where(p => p.RelatedDepartments.Any(d => d.DepartmentId == department.DepartmentId)).ToList()
            };

            return View(model);
        }

        // GET: Departments/Create
        public IActionResult Create()
        {
            ViewBag.ApplicationUsers = new SelectList(_context.ApplicationUsers
                .Where(u => u.HeadingDepartment == null)
                .Select(u => new { FullName = String.Format("{0} {1}", u.FirstName, u.LastName), u.Id })
                , "Id", "FullName");
            return View();
        }

        // POST: Departments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DepartmentId,Name,DepartmentHeadId")] Department department)
        {
            if (ModelState.IsValid)
            {
                _context.Add(department);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(department);
        }

        // GET: Departments/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }
            ViewBag.ApplicationUsers = new SelectList(_context.ApplicationUsers
                .Where(u => u.HeadingDepartment == null || u.HeadingDepartment.DepartmentId == id)
                .Select(u => new { FullName = String.Format("{0} {1}", u.FirstName, u.LastName), u.Id })
                , "Id", "FullName");
            return View(department);
        }

        // POST: Departments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("DepartmentId,Name,DepartmentHeadId")] Department department)
        {
            if (id != department.DepartmentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(department);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DepartmentExists(department.DepartmentId))
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
            return View(department);
        }

        // GET: Departments/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments
                .FirstOrDefaultAsync(m => m.DepartmentId == id);
            if (department == null)
            {
                return NotFound();
            }
            DepartmentDetailsViewModel model = new DepartmentDetailsViewModel
            {
                Department = department,
                DepartmentHead = _context.ApplicationUsers.FirstOrDefault(u => u.Id == department.DepartmentHeadId),
                OwnedProjects = _context.Projects.Where(p => p.OwnerDepartmentId == department.DepartmentId).ToList(),
                RelatedProjects = _context.Projects.Where(p => p.RelatedDepartments.Any(d => d.DepartmentId == department.DepartmentId)).ToList()
            };

            return View(model);
        }

        // POST: Departments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var department = await _context.Departments.FindAsync(id);
            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DepartmentExists(long id)
        {
            return _context.Departments.Any(e => e.DepartmentId == id);
        }
    }
}
