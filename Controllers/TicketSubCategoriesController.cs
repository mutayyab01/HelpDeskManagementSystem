﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HelpDeskSystem.Data;
using HelpDeskSystem.Models;
using HelpDeskSystem.ViewModels;
using System.Security.Claims;
using HelpDeskSystem.Services;
using Microsoft.AspNetCore.Authorization;
using HelpDeskSystem.ClaimManagement;

namespace HelpDeskSystem.Controllers
{
    [Authorize]
    public class TicketSubCategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TicketSubCategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TicketSubCategories
        [Permission("subcategories:view")]
        public async Task<IActionResult> Index(int id, TicketSubCategoriesVM VM)
        {
            var ticketSubCategories = _context.TicketSubCategories
                .Include(t => t.Category)
                .Include(t => t.CreatedBy)
                .Include(t => t.ModifiedBy)
                .Where(x => x.CategoryId == id)
                .AsQueryable();

            if (VM != null)
            {
                if (VM != null && !string.IsNullOrEmpty(VM.Code))
                {
                    ticketSubCategories = ticketSubCategories.Where(x => x.Code.Contains(VM.Code));
                }
                if (VM != null && !string.IsNullOrEmpty(VM.CreatedById))
                {
                    ticketSubCategories = ticketSubCategories.Where(x => x.CreatedById == VM.CreatedById);
                }
                if (VM != null && !string.IsNullOrEmpty(VM.Name))
                {
                    ticketSubCategories = ticketSubCategories.Where(x => x.Name.Contains(VM.Name));
                }
                if (VM != null && VM.CategoryId > 0)
                {
                    ticketSubCategories = ticketSubCategories.Where(x => x.CategoryId == VM.CategoryId);
                }
            }

            VM.CategoryId = id;
            VM.TicketSubCategories = await ticketSubCategories.ToListAsync();
            ViewData["CreatedById"] = new SelectList(_context.Users, "Id", "FullName");
            ViewData["CategoryId"] = new SelectList(_context.TicketSubCategories, "Id", "Name");

            return View(VM);

        }
        public async Task<IActionResult> SubCategories(TicketSubCategoriesVM VM)
        {
            var ticketSubCategories =  _context.TicketSubCategories
                .Include(t => t.Category)
                .Include(t => t.CreatedBy)
                .Include(t => t.ModifiedBy)
                .AsQueryable();

            if (VM != null)
            {
                if (VM != null && !string.IsNullOrEmpty(VM.Code))
                {
                    ticketSubCategories = ticketSubCategories.Where(x => x.Code.Contains(VM.Code));
                }
                if (VM != null && !string.IsNullOrEmpty(VM.CreatedById))
                {
                    ticketSubCategories = ticketSubCategories.Where(x => x.CreatedById == VM.CreatedById);
                }
                if (VM != null && !string.IsNullOrEmpty(VM.Name))
                {
                    ticketSubCategories = ticketSubCategories.Where(x => x.Name.Contains(VM.Name));
                }
                if (VM != null && VM.CategoryId > 0)
                {
                    ticketSubCategories = ticketSubCategories.Where(x => x.CategoryId == VM.CategoryId);
                }
            }

            VM.TicketSubCategories = await ticketSubCategories.ToListAsync();
            ViewData["CreatedById"] = new SelectList(_context.Users, "Id", "FullName");
            ViewData["CategoryId"] = new SelectList(_context.TicketSubCategories, "Id", "Name");
            return View(VM);

        }
        // GET: TicketSubCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketSubCategory = await _context.TicketSubCategories
                .Include(t => t.Category)
                .Include(t => t.CreatedBy)
                .Include(t => t.ModifiedBy)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticketSubCategory == null)
            {
                return NotFound();
            }

            return View(ticketSubCategory);
        }

        // GET: TicketSubCategories/Create
        [Permission($"subcategories:{nameof(Create)}")]

        public IActionResult Create(int Id)
        {
            TicketSubCategory category = new();
            category.CategoryId = Id;

            return View(category);
        }

        // POST: TicketSubCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Permission($"subcategories:{nameof(Create)}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id, TicketSubCategory ticketSubCategory)
        {
            var UserId = User.GetUserId();
            ticketSubCategory.CreatedOn = DateTime.Now;
            ticketSubCategory.CreatedById = UserId;

            ticketSubCategory.Id = 0;
            ticketSubCategory.CategoryId = id;

            _context.Add(ticketSubCategory);
            await _context.SaveChangesAsync(UserId);



            TempData["MESSEGE"] = "Ticket Sub-Category Created Successfully";

            return RedirectToAction("Index", new { id = id });

            return View(ticketSubCategory);
        }

        // GET: TicketSubCategories/Edit/5
        [Permission($"subcategories:{nameof(Edit)}")]

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketSubCategory = await _context.TicketSubCategories.FindAsync(id);
            if (ticketSubCategory == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.TicketCategories, "Id", "Name", ticketSubCategory.CategoryId);
            ViewData["CreatedById"] = new SelectList(_context.Users, "Id", "Id", ticketSubCategory.CreatedById);
            ViewData["ModifiedById"] = new SelectList(_context.Users, "Id", "Id", ticketSubCategory.ModifiedById);
            return View(ticketSubCategory);
        }

        // POST: TicketSubCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Permission($"subcategories:{nameof(Edit)}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TicketSubCategory ticketSubCategory)
        {
            if (id != ticketSubCategory.Id)
            {
                return NotFound();
            }


            try
            {
                var UserId = User.GetUserId();
                ticketSubCategory.ModifiedOn = DateTime.Now;
                ticketSubCategory.ModifiedById = UserId;
                _context.Update(ticketSubCategory);
                await _context.SaveChangesAsync(UserId);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TicketSubCategoryExists(ticketSubCategory.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index), new { id = ticketSubCategory.CategoryId });


            return View(ticketSubCategory);
        }

        // GET: TicketSubCategories/Delete/5
        [Permission($"subcategories:{nameof(Delete)}")]

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketSubCategory = await _context.TicketSubCategories
                .Include(t => t.Category)
                .Include(t => t.CreatedBy)
                .Include(t => t.ModifiedBy)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticketSubCategory == null)
            {
                return NotFound();
            }

            return View(ticketSubCategory);
        }

        // POST: TicketSubCategories/Delete/5
        [Permission($"subcategories:{nameof(Delete)}")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var UserId = User.GetUserId();
            var ticketSubCategory = await _context.TicketSubCategories.FindAsync(id);
            if (ticketSubCategory != null)
            {
                _context.TicketSubCategories.Remove(ticketSubCategory);
            }

            await _context.SaveChangesAsync(UserId);
            return RedirectToAction(nameof(Index));
        }

        private bool TicketSubCategoryExists(int id)
        {
            return _context.TicketSubCategories.Any(e => e.Id == id);
        }
    }
}
