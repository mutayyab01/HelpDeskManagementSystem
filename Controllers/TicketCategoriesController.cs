﻿using System;
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
using HelpDeskSystem.ViewModels;

namespace HelpDeskSystem.Controllers
{
    [Authorize]
    public class TicketCategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TicketCategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TicketCategories
        [Permission("categories:view")]
        public async Task<IActionResult> Index(TicketCategoryViewModel VM)
        {

            var ticketCategories = _context.TicketCategories
                .Include(t => t.CreatedBy)
                .Include(t => t.ModifiedBy)
                .AsQueryable();

            if (VM != null)
            {
                if (VM != null && !string.IsNullOrEmpty(VM.Code))
                {
                    ticketCategories = ticketCategories.Where(x => x.Code.Contains(VM.Code));
                }
                if (VM != null && !string.IsNullOrEmpty(VM.CreatedById))
                {
                    ticketCategories = ticketCategories.Where(x => x.CreatedById == VM.CreatedById);
                }
                if (VM != null && !string.IsNullOrEmpty(VM.Name))
                {
                    ticketCategories = ticketCategories.Where(x => x.Name == VM.Name);
                }
            }

            VM.TicketCategories = await ticketCategories.ToListAsync();

            ViewData["CreatedById"] = new SelectList(_context.Users, "Id", "FullName");

            return View(VM);
        }

        // GET: TicketCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketCategory = await _context.TicketCategories
                .Include(t => t.CreatedBy)
                .Include(t => t.ModifiedBy)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticketCategory == null)
            {
                return NotFound();
            }

            return View(ticketCategory);
        }

        // GET: TicketCategories/Create
        [Permission($"categories:{nameof(Create)}")]

        public IActionResult Create()
        {
            //ViewData["CreatedById"] = new SelectList(_context.Users, "Id", "Id");
            //ViewData["ModifiedById"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: TicketCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Permission($"categories:{nameof(Create)}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TicketCategory ticketCategory)
        {
            var UserId = User.GetUserId();
            ticketCategory.CreatedOn = DateTime.Now;
            ticketCategory.CreatedById = UserId;
            _context.Add(ticketCategory);
            await _context.SaveChangesAsync(UserId);
            TempData["MESSEGE"] = "Ticket Category Created Successfully";

            return RedirectToAction(nameof(Index));

            return View(ticketCategory);
        }

        // GET: TicketCategories/Edit/5
        [Permission($"categories:{nameof(Edit)}")]

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketCategory = await _context.TicketCategories.FindAsync(id);
            if (ticketCategory == null)
            {
                return NotFound();
            }

            return View(ticketCategory);
        }

        // POST: TicketCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Permission($"categories:{nameof(Edit)}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TicketCategory ticketCategory)
        {
            if (id != ticketCategory.Id)
            {
                return NotFound();
            }


            try
            {
                var UserId = User.GetUserId();
                ticketCategory.ModifiedOn = DateTime.Now;
                ticketCategory.ModifiedById = UserId;
                _context.Update(ticketCategory);
                await _context.SaveChangesAsync(UserId);
                TempData["MESSEGE"] = "Ticket Category Updated Successfully";

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TicketCategoryExists(ticketCategory.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
            return View(ticketCategory);
        }

        // GET: TicketCategories/Delete/5
        [Permission($"categories:{nameof(Delete)}")]

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketCategory = await _context.TicketCategories
                .Include(t => t.CreatedBy)
                .Include(t => t.ModifiedBy)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticketCategory == null)
            {
                return NotFound();
            }

            return View(ticketCategory);
        }

        // POST: TicketCategories/Delete/5
        [Permission($"categories:{nameof(Delete)}")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var UserId = User.GetUserId();
            var ticketCategory = await _context.TicketCategories.FindAsync(id);
            if (ticketCategory != null)
            {
                _context.TicketCategories.Remove(ticketCategory);
            }

            await _context.SaveChangesAsync(UserId);
            TempData["MESSEGE"] = "Ticket Category Deleted Successfully";

            return RedirectToAction(nameof(Index));
        }

        private bool TicketCategoryExists(int id)
        {
            return _context.TicketCategories.Any(e => e.Id == id);
        }
    }
}
