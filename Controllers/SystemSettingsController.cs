using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HelpDeskSystem.Data;
using HelpDeskSystem.Models;
using System.Security.Claims;
using HelpDeskSystem.Services;
using Microsoft.AspNetCore.Authorization;
using HelpDeskSystem.ClaimManagement;

namespace HelpDeskSystem.Controllers
{
    [Authorize]
    public class SystemSettingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SystemSettingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SystemSettings
        [Permission("systemsetting:view")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.SystemSettings
                .Include(s => s.CreatedBy)
                .Include(s => s.ModifiedBy);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: SystemSettings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var systemSetting = await _context.SystemSettings
                .Include(s => s.CreatedBy)
                .Include(s => s.ModifiedBy)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (systemSetting == null)
            {
                return NotFound();
            }

            return View(systemSetting);
        }


        // GET: SystemSettings/Create
        [Permission($"systemsetting:{nameof(Create)}")]

        public IActionResult Create()
        {
            ViewData["CreatedById"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["ModifiedById"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: SystemSettings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SystemSetting systemSetting)
        {

            var UserId = User.GetUserId();
            systemSetting.CreatedOn = DateTime.Now;
            systemSetting.CreatedById = UserId;

            _context.Add(systemSetting);
            await _context.SaveChangesAsync(UserId);
            return RedirectToAction(nameof(Index));

            ViewData["CreatedById"] = new SelectList(_context.Users, "Id", "Id", systemSetting.CreatedById);
            ViewData["ModifiedById"] = new SelectList(_context.Users, "Id", "Id", systemSetting.ModifiedById);
            return View(systemSetting);
        }

        // GET: SystemSettings/Edit/5
        [Permission($"systemsetting:{nameof(Edit)}")]

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var systemSetting = await _context.SystemSettings.FindAsync(id);
            if (systemSetting == null)
            {
                return NotFound();
            }
            ViewData["CreatedById"] = new SelectList(_context.Users, "Id", "Id", systemSetting.CreatedById);
            ViewData["ModifiedById"] = new SelectList(_context.Users, "Id", "Id", systemSetting.ModifiedById);
            return View(systemSetting);
        }

        // POST: SystemSettings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SystemSetting systemSetting)
        {
            if (id != systemSetting.Id)
            {
                return NotFound();
            }


            try
            {
                var UserId = User.GetUserId();
                systemSetting.ModifiedOn = DateTime.Now;
                systemSetting.ModifiedById = UserId;

                _context.Update(systemSetting);
                await _context.SaveChangesAsync(UserId);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SystemSettingExists(systemSetting.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));

            ViewData["CreatedById"] = new SelectList(_context.Users, "Id", "Id", systemSetting.CreatedById);
            ViewData["ModifiedById"] = new SelectList(_context.Users, "Id", "Id", systemSetting.ModifiedById);
            return View(systemSetting);
        }

        // GET: SystemSettings/Delete/5
        [Permission($"systemsetting:{nameof(Delete)}")]

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var systemSetting = await _context.SystemSettings
                .Include(s => s.CreatedBy)
                .Include(s => s.ModifiedBy)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (systemSetting == null)
            {
                return NotFound();
            }

            return View(systemSetting);
        }

        // POST: SystemSettings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var UserId = User.GetUserId();
            var systemSetting = await _context.SystemSettings.FindAsync(id);
            if (systemSetting != null)
            {
                _context.SystemSettings.Remove(systemSetting);
            }

            await _context.SaveChangesAsync(UserId);
            return RedirectToAction(nameof(Index));
        }

        private bool SystemSettingExists(int id)
        {
            return _context.SystemSettings.Any(e => e.Id == id);
        }
    }
}
