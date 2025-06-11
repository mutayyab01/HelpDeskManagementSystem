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
using HelpDeskSystem.ViewModels;
using AutoMapper;
using HelpDeskSystem.Services;
using Microsoft.AspNetCore.Authorization;
using HelpDeskSystem.ClaimManagement;

namespace HelpDeskSystem.Controllers
{
    [Authorize]
    public class SystemCodesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public SystemCodesController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: SystemCodes
        [Permission("systemcodes:view")]
        public async Task<IActionResult> Index(SystemCodeViewModel VM)
        {
            var systemCodes = _context
                     .SystemCodes
                     .Include(x => x.CreatedBy)
                     .AsQueryable();

            if (VM != null)
            {
                if (VM != null && !string.IsNullOrEmpty(VM.Code))
                {
                    systemCodes = systemCodes.Where(x => x.Code.Contains(VM.Code));
                }
                if (VM != null && !string.IsNullOrEmpty(VM.CreatedById))
                {
                    systemCodes = systemCodes.Where(x => x.CreatedById == VM.CreatedById);
                }
                if (VM != null && !string.IsNullOrEmpty(VM.Description))
                {
                    systemCodes = systemCodes.Where(x => x.Description.Contains(VM.Description));
                }
            }

            VM.SystemCodes = await systemCodes.ToListAsync();

            ViewData["CreatedById"] = new SelectList(_context.Users, "Id", "FullName");
            return View(VM);
        }

        // GET: SystemCodes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var systemCode = await _context.SystemCodes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (systemCode == null)
            {
                return NotFound();
            }

            return View(systemCode);
        }

        // GET: SystemCodes/Create
        [Permission($"systemcodes:{nameof(Create)}")]

        public IActionResult Create()
        {
            return View();
        }

        // POST: SystemCodes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Permission($"systemcodes:{nameof(Create)}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SystemCodeViewModel VM)
        {
            var UserId = User.GetUserId();
            SystemCode systemCodeDetails = new();
            var systemCode = _mapper.Map(VM, systemCodeDetails);

            systemCode.CreatedOn = DateTime.Now;
            systemCode.CreatedById = UserId;
            _context.Add(systemCode);
            await _context.SaveChangesAsync(UserId);


            TempData["MESSEGE"] = "System Codes Created Successfully";
            return RedirectToAction(nameof(Index));

            return View(systemCode);
        }

        // GET: SystemCodes/Edit/5
        [Permission($"systemcodes:{nameof(Edit)}")]

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var systemCode = await _context.SystemCodes.FindAsync(id);
            if (systemCode == null)
            {
                return NotFound();
            }
            return View(systemCode);
        }

        // POST: SystemCodes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Permission($"systemcodes:{nameof(Edit)}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SystemCode systemCode)
        {
            if (id != systemCode.Id)
            {
                return NotFound();
            }


            try
            {
                var UserId = User.GetUserId();
                systemCode.ModifiedOn = DateTime.Now;
                systemCode.ModifiedById = UserId;

                _context.Update(systemCode);
                await _context.SaveChangesAsync(UserId);


                TempData["MESSEGE"] = "System Codes Updated Successfully";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SystemCodeExists(systemCode.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));

            return View(systemCode);
        }

        // GET: SystemCodes/Delete/5
        [Permission($"systemcodes:{nameof(Delete)}")]

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var systemCode = await _context.SystemCodes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (systemCode == null)
            {
                return NotFound();
            }

            return View(systemCode);
        }

        // POST: SystemCodes/Delete/5
        [Permission($"systemcodes:{nameof(Delete)}")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var UserId = User.GetUserId();
            var systemCode = await _context.SystemCodes.FindAsync(id);
            if (systemCode != null)
            {
                _context.SystemCodes.Remove(systemCode);
            }

            await _context.SaveChangesAsync(UserId);
            return RedirectToAction(nameof(Index));
        }

        private bool SystemCodeExists(int id)
        {
            return _context.SystemCodes.Any(e => e.Id == id);
        }
    }
}
