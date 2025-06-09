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
using HelpDeskSystem.Data.Migrations;
using AutoMapper;
using HelpDeskSystem.Services;
using ElmahCore;
using Microsoft.AspNetCore.Authorization;
using HelpDeskSystem.ClaimManagement;

namespace HelpDeskSystem.Controllers
{
    [Authorize]
    public class TicketsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public TicketsController(ApplicationDbContext context, 
            IConfiguration configuration, IWebHostEnvironment webHostEnvironment
            ,IMapper mapper)
        {
            _context = context;
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
            _mapper = mapper;
        }

        // GET: Tickets
        [Permission("TICKETS:VIEW")]
        public async Task<IActionResult> Index(TicketViewModel VM)
        {
            var alltickets = _context.Tickets
                 .Include(t => t.CreatedBy)
                 .Include(t => t.SubCategory)
                 .Include(t => t.Priority)
                 .Include(t => t.Status)
                 .Include(t => t.TicketComments)
                 .OrderBy(x => x.CreatedOn)
             .AsQueryable();
            if (VM != null && !string.IsNullOrEmpty(VM.Title))
            {
                alltickets = alltickets.Where(x => x.Title.Contains(VM.Title));
            }
            if (VM != null && !string.IsNullOrEmpty(VM.CreatedById))
            {
                alltickets = alltickets.Where(x => x.CreatedById == VM.CreatedById);
            }
            if (VM != null && VM.StatusId > 0)
            {
                alltickets = alltickets.Where(x => x.StatusId == VM.StatusId);
            }
            if (VM != null && VM.PriorityId > 0)
            {
                alltickets = alltickets.Where(x => x.PriorityId == VM.PriorityId);
            }
            if (VM != null && VM.CategoryId > 0)
            {
                alltickets = alltickets.Where(x => x.SubCategory.CategoryId == VM.CategoryId);
            }

            VM.Tickets = await alltickets.ToListAsync();

            VM.MainDuration = await _context.SystemSettings
                .Where(x => x.Code == "TICKETRESOLUTIONDAYS")
                .FirstOrDefaultAsync();

            ViewData["PriorityId"] = new SelectList(_context.SystemCodeDetails.Include(x => x.SystemCode).Where(x => x.SystemCode.Code == "Priority"), "Id", "Description");
            ViewData["CategoryId"] = new SelectList(_context.TicketCategories, "Id", "Name");
            ViewData["CreatedById"] = new SelectList(_context.Users, "Id", "FullName");
            ViewData["StatusId"] = new SelectList(_context.SystemCodeDetails.Include(x => x.SystemCode).Where(x => x.SystemCode.Code == "RESOLUTIONSTATUS"), "Id", "Description");

            return View(VM);
        }

        public async Task<IActionResult> AssignedTickets(TicketViewModel VM)
        {
            //var assignedstatus = await _context.SystemCodeDetails
            //   .Include(x => x.SystemCode)
            //   .Where(x => x.SystemCode.Code == "RESOLUTIONSTATUS" && x.Code == "Assigned")
            //   .FirstOrDefaultAsync();

            VM.Tickets = await _context.Tickets
                .Include(t => t.CreatedBy)
                .Include(t => t.SubCategory)
                .Include(t => t.Priority)
                .Include(t => t.Status)
                .Include(t => t.TicketComments)
                .Where(t => t.Status.Code == "Assigned")
                .OrderBy(x => x.CreatedOn)
                .ToListAsync();

            var alltickets = _context.Tickets
                .Include(t => t.CreatedBy)
                .Include(t => t.SubCategory)
                .Include(t => t.Priority)
                .Include(t => t.Status)
                .Include(t => t.TicketComments)
                .Where(t => t.Status.Code == "Assigned")
                .OrderBy(x => x.CreatedOn)
            .AsQueryable();
            if (VM != null && !string.IsNullOrEmpty(VM.Title))
            {
                alltickets = alltickets.Where(x => x.Title.Contains(VM.Title));
            }
            if (VM != null && !string.IsNullOrEmpty(VM.CreatedById))
            {
                alltickets = alltickets.Where(x => x.CreatedById == VM.CreatedById);
            }
            if (VM != null && VM.StatusId > 0)
            {
                alltickets = alltickets.Where(x => x.StatusId == VM.StatusId);
            }
            if (VM != null && VM.PriorityId > 0)
            {
                alltickets = alltickets.Where(x => x.PriorityId == VM.PriorityId);
            }
            if (VM != null && VM.CategoryId > 0)
            {
                alltickets = alltickets.Where(x => x.SubCategory.CategoryId == VM.CategoryId);
            }

            VM.Tickets = await alltickets.ToListAsync();

            ViewData["PriorityId"] = new SelectList(_context.SystemCodeDetails.Include(x => x.SystemCode).Where(x => x.SystemCode.Code == "Priority"), "Id", "Description");
            ViewData["CategoryId"] = new SelectList(_context.TicketCategories, "Id", "Name");
            ViewData["CreatedById"] = new SelectList(_context.Users, "Id", "FullName");
            ViewData["StatusId"] = new SelectList(_context.SystemCodeDetails.Include(x => x.SystemCode).Where(x => x.SystemCode.Code == "RESOLUTIONSTATUS"), "Id", "Description");

            return View(VM);
        }

        public async Task<IActionResult> ClosedTickets(TicketViewModel VM)
        {
            var closedstatus = await _context.SystemCodeDetails
               .Include(x => x.SystemCode)
               .Where(x => x.SystemCode.Code == "RESOLUTIONSTATUS" && x.Code == "Closed")
               .FirstOrDefaultAsync();

            var alltickets = _context.Tickets
                 .Include(t => t.CreatedBy)
                 .Include(t => t.SubCategory)
                 .Include(t => t.Priority)
                 .Include(t => t.Status)
                 .Include(t => t.TicketComments)
                 .Where(t => t.Status.Id==closedstatus.Id)
                 .OrderBy(x => x.CreatedOn)
             .AsQueryable();
            if (VM != null && !string.IsNullOrEmpty(VM.Title))
            {
                alltickets = alltickets.Where(x => x.Title.Contains(VM.Title));
            }
            if (VM != null && !string.IsNullOrEmpty(VM.CreatedById))
            {
                alltickets = alltickets.Where(x => x.CreatedById == VM.CreatedById);
            }
            if (VM != null && VM.StatusId > 0)
            {
                alltickets = alltickets.Where(x => x.StatusId == VM.StatusId);
            }
            if (VM != null && VM.PriorityId > 0)
            {
                alltickets = alltickets.Where(x => x.PriorityId == VM.PriorityId);
            }
            if (VM != null && VM.CategoryId > 0)
            {
                alltickets = alltickets.Where(x => x.SubCategory.CategoryId == VM.CategoryId);
            }

            VM.Tickets = await alltickets.ToListAsync();

            ViewData["PriorityId"] = new SelectList(_context.SystemCodeDetails.Include(x => x.SystemCode).Where(x => x.SystemCode.Code == "Priority"), "Id", "Description");
            ViewData["CategoryId"] = new SelectList(_context.TicketCategories, "Id", "Name");
            ViewData["CreatedById"] = new SelectList(_context.Users, "Id", "FullName");
            ViewData["StatusId"] = new SelectList(_context.SystemCodeDetails.Include(x => x.SystemCode).Where(x => x.SystemCode.Code == "RESOLUTIONSTATUS"), "Id", "Description");

            return View(VM);
        }

        public async Task<IActionResult> ResolvedTickets(TicketViewModel VM)
        {
            var resolvedstatus = await _context.SystemCodeDetails
               .Include(x => x.SystemCode)
               .Where(x => x.SystemCode.Code == "RESOLUTIONSTATUS" && x.Code == "REsolved")
               .FirstOrDefaultAsync();

            var alltickets = _context.Tickets
                .Include(t => t.CreatedBy)
                .Include(t => t.SubCategory)
                .Include(t => t.Priority)
                .Include(t => t.Status)
                .Include(t => t.TicketComments)
                .Where(t => t.Status.Id == resolvedstatus.Id)
                .OrderBy(x => x.CreatedOn)
            .AsQueryable();
            if (VM != null && !string.IsNullOrEmpty(VM.Title))
            {
                alltickets = alltickets.Where(x => x.Title.Contains(VM.Title));
            }
            if (VM != null && !string.IsNullOrEmpty(VM.CreatedById))
            {
                alltickets = alltickets.Where(x => x.CreatedById == VM.CreatedById);
            }
            if (VM != null && VM.StatusId > 0)
            {
                alltickets = alltickets.Where(x => x.StatusId == VM.StatusId);
            }
            if (VM != null && VM.PriorityId > 0)
            {
                alltickets = alltickets.Where(x => x.PriorityId == VM.PriorityId);
            }
            if (VM != null && VM.CategoryId > 0)
            {
                alltickets = alltickets.Where(x => x.SubCategory.CategoryId == VM.CategoryId);
            }

            VM.Tickets = await alltickets.ToListAsync();

            ViewData["PriorityId"] = new SelectList(_context.SystemCodeDetails.Include(x => x.SystemCode).Where(x => x.SystemCode.Code == "Priority"), "Id", "Description");
            ViewData["CategoryId"] = new SelectList(_context.TicketCategories, "Id", "Name");
            ViewData["CreatedById"] = new SelectList(_context.Users, "Id", "FullName");
            ViewData["StatusId"] = new SelectList(_context.SystemCodeDetails.Include(x => x.SystemCode).Where(x => x.SystemCode.Code == "RESOLUTIONSTATUS"), "Id", "Description");

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
                .Include(t => t.AssignedTo)
                .FirstOrDefaultAsync(m => m.Id == id);

            VM.TicketComments = await _context.Comments
                .Include(x => x.CreatedBy)
                .Include(x => x.Ticket)
                .Where(x => x.TicketId == id)
                .ToListAsync();
            VM.TicketResolutions = await _context.TicketResolutions
           .Include(x => x.CreatedBy)
           .Include(x => x.Ticket)
           .Include(x => x.Status)
           .Where(x => x.TicketId == id)
           .ToListAsync();

            if (VM.TicketDetails == null)
            {
                return NotFound();
            }

            return View(VM);
        }
        public async Task<IActionResult> ReOpen(int? id, TicketViewModel VM)
        {
            ViewData["StatusId"] = new SelectList(_context.SystemCodeDetails.Include(x => x.SystemCode).Where(x => x.SystemCode.Code == "RESOLUTIONSTATUS"), "Id", "Description");


            if (id == null)
            {
                return NotFound();
            }

            VM.TicketDetails = await _context.Tickets
                .Include(t => t.CreatedBy)
                .Include(t => t.SubCategory)
                .Include(t => t.Status)
                .Include(t => t.Priority)
                .Include(t => t.AssignedTo)
                .FirstOrDefaultAsync(m => m.Id == id);

            VM.TicketComments = await _context.Comments
                .Include(x => x.CreatedBy)
                .Include(x => x.Ticket)
                .Where(x => x.TicketId == id)
                .ToListAsync();

            VM.TicketResolutions = await _context.TicketResolutions
               .Include(x => x.CreatedBy)
               .Include(x => x.Ticket)
               .Include(x => x.Status)
               .Where(x => x.TicketId == id)
               .ToListAsync();

            if (VM.TicketDetails == null)
            {
                return NotFound();
            }

            return View(VM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignedConfirmed(int id, TicketViewModel VM)
        {
            try
            {
                var reassignedstatus = await _context.SystemCodeDetails
                        .Include(x => x.SystemCode)
                        .Where(x => x.SystemCode.Code == "RESOLUTIONSTATUS" && x.Code == "Assigned")
                        .FirstOrDefaultAsync();

                var UserId = User.GetUserId();
                TicketResolution resolution = new();
                resolution.TicketId = id;
                resolution.StatusId = reassignedstatus.Id;
                resolution.CreatedOn = DateTime.Now;
                resolution.CreatedById = UserId;
                resolution.Description = "Ticket Assigned";
                _context.Add(resolution);

                var ticket = await _context.Tickets
                    .Where(x => x.Id == id)
                    .FirstOrDefaultAsync();
                ticket.StatusId = reassignedstatus.Id;
                ticket.AssignedToId = VM.AssignedToId;
                ticket.AssignedOn = DateTime.Now;
                _context.Update(ticket);

                await _context.SaveChangesAsync(UserId);

                TempData["MESSEGE"] = "Ticket Resolved Successfully";

                return RedirectToAction("Resolve", new { id = id });
            }
            catch (Exception ex)
            {
                ElmahExtensions.RaiseError(ex);
                TempData["ERROR"] = "Ticket Could not Resolved Successfully"+ex.Message;
                return View(VM);

            }
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReOpenConfirmed(int id, TicketViewModel VM)
        {
            var closeStaus = await _context.SystemCodeDetails
                .Include(x => x.SystemCode)
                .Where(x => x.SystemCode.Code == "RESOLUTIONSTATUS" && x.Code == "ReOpened")
                .FirstOrDefaultAsync();

            var UserId = User.GetUserId();
            TicketResolution resolution = new();
            resolution.TicketId = id;
            resolution.StatusId = closeStaus.Id;
            resolution.CreatedOn = DateTime.Now;
            resolution.CreatedById = UserId;
            resolution.Description = "Ticket Re-Opened";
            _context.Add(resolution);

            var ticket = await _context.Tickets
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
            ticket.StatusId = closeStaus.Id;
            _context.Update(ticket);

            await _context.SaveChangesAsync(UserId);
           
            TempData["MESSEGE"] = "Ticket Re-Opened Successfully";

            return RedirectToAction("Resolve", new { id = id });
        }

        public async Task<IActionResult> TicketAssignment(int? id, TicketViewModel VM)
        {
            ViewData["StatusId"] = new SelectList(_context.SystemCodeDetails.Include(x => x.SystemCode).Where(x => x.SystemCode.Code == "RESOLUTIONSTATUS"), "Id", "Description");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "FullName");


            if (id == null)
            {
                return NotFound();
            }

            VM.TicketDetails = await _context.Tickets
                .Include(t => t.CreatedBy)
                .Include(t => t.SubCategory)
                .Include(t => t.Status)
                .Include(t => t.Priority)
                .Include(t => t.AssignedTo)
                .FirstOrDefaultAsync(m => m.Id == id);

            VM.TicketComments = await _context.Comments
                .Include(x => x.CreatedBy)
                .Include(x => x.Ticket)
                .Where(x => x.TicketId == id)
                .ToListAsync();

            VM.TicketResolutions = await _context.TicketResolutions
               .Include(x => x.CreatedBy)
               .Include(x => x.Ticket)
               .Include(x => x.Status)
               .Where(x => x.TicketId == id)
               .ToListAsync();

            if (VM.TicketDetails == null)
            {
                return NotFound();
            }

            return View(VM);
        }

        public async Task<IActionResult> Resolve(int? id, TicketViewModel VM)
        {
            ViewData["StatusId"] = new SelectList(_context.SystemCodeDetails.Include(x => x.SystemCode).Where(x => x.SystemCode.Code == "RESOLUTIONSTATUS"), "Id", "Description");


            if (id == null)
            {
                return NotFound();
            }

            VM.TicketDetails = await _context.Tickets
                .Include(t => t.CreatedBy)
                .Include(t => t.SubCategory)
                .Include(t => t.Status)
                .Include(t => t.Priority)
                .Include(t => t.AssignedTo)
                .FirstOrDefaultAsync(m => m.Id == id);

            VM.TicketComments = await _context.Comments
                .Include(x => x.CreatedBy)
                .Include(x => x.Ticket)
                .Where(x => x.TicketId == id)
                .ToListAsync();

            VM.TicketResolutions = await _context.TicketResolutions
               .Include(x => x.CreatedBy)
               .Include(x => x.Ticket)
               .Include(x => x.Status)
               .Where(x => x.TicketId == id)
               .ToListAsync();

            if (VM.TicketDetails == null)
            {
                return NotFound();
            }

            return View(VM);
        }
        // GET: Tickets/Create
        [Permission($"tickets:{nameof(Create)}")]
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
            try
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
                Ticket ticketdetails = new();

                var ticket = _mapper.Map(ticketVM, ticketdetails);

                /*
                Ticket ticket = new();
                ticket.Id = ticketVM.Id;
                ticket.Title = ticketVM.Title;
                ticket.Description = ticketVM.Description;
                ticket.PriorityId = ticketVM.PriorityId;
                ticket.SubCategoryId = ticketVM.SubCategoryId;
                ticket.Attachment = ticketVM.Attachment;
                */


                ticket.StatusId = pendingstatus.Id;
                var UserId = User.GetUserId();
                ticket.CreatedOn = DateTime.Now;
                ticket.CreatedById = UserId;
                _context.Add(ticket);
                await _context.SaveChangesAsync(UserId);


                TempData["MESSEGE"] = "Ticket Created Successfully";

                return RedirectToAction(nameof(Index));
                return View(ticket);
            }
            catch (Exception ex)
            {
                ElmahExtensions.RaiseError(ex);
                TempData["ERROR"] = "Ticket Couldn't Be Created "+ex.Message;
                return View(ticketVM);

            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(int id, TicketViewModel VM)
        {
            try
            {
                var UserId = User.GetUserId();
                Comment comment = new();
                comment.TicketId = id;
                comment.CreatedOn = DateTime.Now;
                comment.CreatedById = UserId;
                comment.Description = VM.CommentsDescription;
                _context.Add(comment);
                await _context.SaveChangesAsync(UserId);

                TempData["MESSEGE"] = "Comment Details Created Successfully";

                return RedirectToAction(nameof(Details), new { id = id });
            }
            catch (Exception ex)
            {
                ElmahExtensions.RaiseError(ex);
                TempData["ERROR"] = "Comment Details Created Successfully";
                return View(VM);

            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ClosedConfirmed(int id, TicketViewModel VM)
        {
            var closeStaus = await _context.SystemCodeDetails
                .Include(x => x.SystemCode)
                .Where(x => x.SystemCode.Code == "RESOLUTIONSTATUS" && x.Code == "Closed")
                .FirstOrDefaultAsync();

            var UserId = User.GetUserId();
            TicketResolution resolution = new();
            resolution.TicketId = id;
            resolution.StatusId = closeStaus.Id;
            resolution.CreatedOn = DateTime.Now;
            resolution.CreatedById = UserId;
            resolution.Description = "Ticket Closed";
            _context.Add(resolution);

            var ticket = await _context.Tickets
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
            ticket.StatusId = closeStaus.Id;
            _context.Update(ticket);

            await _context.SaveChangesAsync(UserId);
         
            TempData["MESSEGE"] = "Ticket Close Successfully";

            return RedirectToAction("Resolve", new { id = id });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResolveConfirmed(int id, TicketViewModel VM)
        {
            var UserId = User.GetUserId();
            TicketResolution resolution = new();
            resolution.TicketId = id;
            resolution.StatusId = VM.StatusId;
            resolution.CreatedOn = DateTime.Now;
            resolution.CreatedById = UserId;
            resolution.Description = VM.CommentsDescription;
            _context.Add(resolution);

            var ticket = await _context.Tickets
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
            ticket.StatusId = VM.StatusId;
            _context.Update(ticket);

            await _context.SaveChangesAsync(UserId);
         
            TempData["MESSEGE"] = "Ticket Resolution Details Created Successfully";

            return RedirectToAction("Resolve", new { id = id });
        }
        // GET: Tickets/Edit/5
        [Permission($"tickets:{nameof(Edit)}")]

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
                var UserId = User.GetUserId();


                _context.Update(ticket);
                await _context.SaveChangesAsync(UserId);
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
        [Permission($"tickets:{nameof(Delete)}")]

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
            var UserId = User.GetUserId();
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket != null)
            {
                _context.Tickets.Remove(ticket);
                TempData["MESSEGE"] = "Ticket Deleted Successfully";

            }

            await _context.SaveChangesAsync(UserId);
            return RedirectToAction(nameof(Index));
        }

        private bool TicketExists(int id)
        {
            return _context.Tickets.Any(e => e.Id == id);
        }
    }
}
