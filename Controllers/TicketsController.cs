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
using Microsoft.AspNetCore.Hosting;
using System.Configuration;

namespace HelpDeskSystem.Controllers
{
    public class TicketsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;

        public TicketsController(ApplicationDbContext context, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Tickets
        public async Task<IActionResult> Index(TicketViewModel VM)
        {
            VM.Tickets = await _context.Tickets
                .Include(t => t.CreatedBy)
                .Include(t => t.SubCategory)
                .Include(t => t.Priority)
                .Include(t => t.Status)
                .Include(t => t.TicketComments)
                .OrderBy(x => x.CreatedOn)
                .ToListAsync();



            return View(VM);
        }

        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(int? id, TicketViewModel VM)
        {
            if (id == null)
            {
                return NotFound();
            }

            VM.TicketDetails = await _context.Tickets
                .Include(t => t.CreatedBy)
                .Include(t => t.SubCategory)
                .Include(t => t.Status)
                .Include(t => t.Priority)
                .FirstOrDefaultAsync(m => m.Id == id);

            VM.TicketComments = await _context.Comments
                .Include(x => x.CreatedBy)
                .Include(x => x.Ticket)
                .Where(x => x.TicketId == id)
                .ToListAsync();

            if (VM.TicketDetails == null)
            {
                return NotFound();
            }

            return View(VM);
        }

        // GET: Tickets/Create
        public IActionResult Create()
        {
            ViewData["PriorityId"] = new SelectList(_context.SystemCodeDetails.Include(x => x.SystemCode).Where(x => x.SystemCode.Code == "PRIORITY"), "Id", "Description");
            ViewData["CategoryId"] = new SelectList(_context.TicketCategories, "Id", "Name");
            ViewData["CreatedById"] = new SelectList(_context.Users, "Id", "FullName");
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TicketViewModel ticketVM, IFormFile accachmentFile)
        {
            ViewData["PriorityId"] = new SelectList(_context.SystemCodeDetails.Include(x => x.SystemCode).Where(x => x.SystemCode.Code == "Priority"), "Id", "Description", ticketVM.PriorityId);
            ViewData["CategoryId"] = new SelectList(_context.TicketCategories, "Id", "Name");
            ViewData["CreatedById"] = new SelectList(_context.Users, "Id", "FullName", ticketVM.CreatedById);
            if (accachmentFile != null)
            {
                var ext = Path.GetExtension(accachmentFile.FileName);
                var size = accachmentFile.Length;
                if (ext == ".png" || ext == ".jpeg" || ext == ".jpg" || ext == ".pdf" || ext == ".docx")
                {
                    if (size <= 1000000)//1Mb
                    {
                        var filename = "Ticket_Attachment" + DateTime.Now.ToString("dd-MM-yyyy hh mm ss tt") + "_" + accachmentFile.FileName;
                        //var path = _configuration["FileSettings:UploadFolder"]!;
                        string folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "TicketAttachment");
                        var filepath = Path.Combine(folderPath, filename);
                        var steam = new FileStream(filepath, FileMode.Create);
                        await accachmentFile.CopyToAsync(steam);
                        ticketVM.Attachment = filename;
                    }
                    else
                    {
                        TempData["sizeError"] = "Document Must be less Than 1 Mb";
                        return View(ticketVM);
                    }
                }
                else
                {
                    TempData["extError"] = "Only PNG,JPEG,JPG,PDF,DOCX Documents Are Allowed";
                    return View(ticketVM);

                }
            }


            var pendingstatus = await _context.SystemCodeDetails.Include(x => x.SystemCode)
                .Where(x => x.SystemCode.Code == "STATUS" && x.Code == "PENDING")
                .FirstOrDefaultAsync();
            Ticket ticket = new();
            ticket.Id = ticketVM.Id;
            ticket.Title = ticketVM.Title;
            ticket.Description = ticketVM.Description;
            ticket.StatusId = pendingstatus.Id;
            ticket.PriorityId = ticketVM.PriorityId;
            ticket.SubCategoryId = ticketVM.SubCategoryId;
            ticket.Attachment = ticketVM.Attachment;



            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ticket.CreatedOn = DateTime.Now;
            ticket.CreatedById = UserId;
            _context.Add(ticket);
            await _context.SaveChangesAsync();

            //log The Audit Trails
            var activity = new AuditTrail()
            {
                Action = "Create",
                TimeStamp = DateTime.Now,
                IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                UserId = UserId,
                Module = "Tickets",
                AffectedTable = "Tickets",
            };

            _context.Add(activity);
            await _context.SaveChangesAsync();
            TempData["MESSEGE"] = "Ticket Created Successfully";

            return RedirectToAction(nameof(Index));
            return View(ticket);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(int id, TicketViewModel VM)
        {
            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Comment comment = new();
            comment.TicketId = id;
            comment.CreatedOn = DateTime.Now;
            comment.CreatedById = UserId;
            comment.Description = VM.CommentsDescription;
            _context.Add(comment);
            await _context.SaveChangesAsync();
            //log The Audit Trails
            var activity = new AuditTrail()
            {
                Action = "Create",
                TimeStamp = DateTime.Now,
                IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                UserId = UserId,
                Module = "Comments",
                AffectedTable = "Comments",
            };

            _context.Add(activity);
            await _context.SaveChangesAsync();
            TempData["MESSEGE"] = "Ticket Comment Created Successfully";

            return RedirectToAction(nameof(Details), new {id=id});
        }


        // GET: Tickets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }
            ViewData["CreatedById"] = new SelectList(_context.Users, "Id", "Id", ticket.CreatedById);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Ticket ticket)
        {
            if (id != ticket.Id)
            {
                return NotFound();
            }


            try
            {
                _context.Update(ticket);
                await _context.SaveChangesAsync();
                TempData["MESSEGE"] = "Ticket Updated Successfully";

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TicketExists(ticket.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));

            ViewData["CreatedById"] = new SelectList(_context.Users, "Id", "Id", ticket.CreatedById);
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .Include(t => t.CreatedBy)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket != null)
            {
                _context.Tickets.Remove(ticket);
                TempData["MESSEGE"] = "Ticket Deleted Successfully";

            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TicketExists(int id)
        {
            return _context.Tickets.Any(e => e.Id == id);
        }
    }
}
