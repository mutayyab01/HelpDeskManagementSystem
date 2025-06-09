using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HelpDeskSystem.Data;
using HelpDeskSystem.Models;
using HelpDeskSystem.Data.Migrations;
using System.Security.Claims;
using HelpDeskSystem.Services;
using Microsoft.AspNetCore.Authorization;
using HelpDeskSystem.ClaimManagement;

namespace HelpDeskSystem.Controllers
{
    [Authorize]
    public class DepartmentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DepartmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Departments
        [Permission("departments:view")]

        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Departments
                .Include(d => d.CreatedBy)
                .Include(d => d.ModifiedBy);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Departments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments
                .Include(d => d.CreatedBy)
                .Include(d => d.ModifiedBy)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        // GET: Departments/Create
        [Permission($"departments:{nameof(Create)}")]

        public IActionResult Create()
        {

            return View();
        }

        // POST: Departments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Permission($"departments:{nameof(Create)}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Department department)
        {
            var UserId = User.GetUserId();
            department.CreatedOn = DateTime.Now;
            department.CreatedById = UserId;

            _context.Add(department);
            await _context.SaveChangesAsync(UserId);
            TempData["MESSEGE"] = "Department Created Successfully";

            return RedirectToAction(nameof(Index));

            return View(department);
        }

        // GET: Departments/Edit/5
        [Permission($"departments:{nameof(Edit)}")]

        public async Task<IActionResult> Edit(int? id)
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
            return View(department);
        }

        // POST: Departments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Permission($"departments:{nameof(Edit)}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Department department)
        {
            if (id != department.Id)
            {
                return NotFound();
            }


            try
            {
                var UserId = User.GetUserId();
                department.ModifiedOn = DateTime.Now;
                department.ModifiedById = UserId;

                _context.Update(department);
                await _context.SaveChangesAsync(UserId);
                TempData["MESSEGE"] = "Department Updated Successfully";

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DepartmentExists(department.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));

            return View(department);
        }

        // GET: Departments/Delete/5
        [Permission($"departments:{nameof(Delete)}")]

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments
                .Include(d => d.CreatedBy)
                .Include(d => d.ModifiedBy)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        // POST: Departments/Delete/5
        [Permission($"departments:{nameof(Delete)}")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var UserId = User.GetUserId();
            var department = await _context.Departments.FindAsync(id);
            if (department != null)
            {
                _context.Departments.Remove(department);
            }

            await _context.SaveChangesAsync(UserId);
            return RedirectToAction(nameof(Index));
        }

        private bool DepartmentExists(int id)
        {
            return _context.Departments.Any(e => e.Id == id);
        }
    }
}
