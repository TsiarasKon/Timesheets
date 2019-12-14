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
        public IActionResult Index(string sortOrder, string searchString)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DepartmentHeadSortParm = sortOrder == "deptHead_asc" ? "deptHead_desc" : "deptHead_asc";

            IEnumerable<Department> deptList = _context.Departments.Include(d => d.DepartmentHead);
            if (String.IsNullOrEmpty(searchString))
            {
                searchString = ViewBag.SearchString;
            }
            else
            {
                ViewBag.SearchString = searchString;
            }
            if (!String.IsNullOrEmpty(searchString))
            {
                deptList = deptList.Where(d => d.Name.Contains(searchString, StringComparison.CurrentCultureIgnoreCase)
                                       || d.DepartmentHead.FirstName.Contains(searchString, StringComparison.CurrentCultureIgnoreCase)
                                       || d.DepartmentHead.LastName.Contains(searchString, StringComparison.CurrentCultureIgnoreCase));
            }
            switch (sortOrder)
            {
                case "deptHead_asc":
                    deptList = deptList.OrderBy(d => d.DepartmentHead.FirstName);
                    break;
                case "deptHead_desc":
                    deptList = deptList.OrderByDescending(d => d.DepartmentHead.FirstName);
                    break;
                case "name_desc":
                    deptList = deptList.OrderByDescending(d => d.Name);
                    break;
                default:
                    deptList = deptList.OrderBy(d => d.Name);
                    break;
            }
            return View(deptList.ToList());
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
            ViewBag.ApplicationUsers = new SelectList(_context.ApplicationUsers
                .Where(u => u.HeadingDepartment == null)
                .Select(u => new { FullName = String.Format("{0} {1}", u.FirstName, u.LastName), u.Id })
                , "Id", "FullName");
            return View(department);
        }

        // GET: Departments/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return View("../Shared/NotFound");
            }

            var department = await _context.Departments.FindAsync(id);
            if (department == null)
            {
                return View("../Shared/NotFound");
            }
            ViewBag.ApplicationUsers = new SelectList(_context.ApplicationUsers
                .Where(u => u.HeadingDepartment == null || u.HeadingDepartment.DepartmentId == id)
                .Select(u => new { FullName = String.Format("{0} {1}", u.FirstName, u.LastName), u.Id })
                , "Id", "FullName");
            return View(department);
        }

        // POST: Departments/Edit/5
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
            ViewBag.ApplicationUsers = new SelectList(_context.ApplicationUsers
                .Where(u => u.HeadingDepartment == null || u.HeadingDepartment.DepartmentId == id)
                .Select(u => new { FullName = String.Format("{0} {1}", u.FirstName, u.LastName), u.Id })
                , "Id", "FullName");
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
