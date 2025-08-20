using AutoMapper;
using HelpDeskSystem.ClaimManagement;
using HelpDeskSystem.Data;
using HelpDeskSystem.Interfaces;
using HelpDeskSystem.Models;
using HelpDeskSystem.Services;
using HelpDeskSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HelpDeskSystem.Controllers
{
    public class ExcelExportsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IExportService _exportService;

        public ExcelExportsController(ApplicationDbContext context,
            IConfiguration configuration, IMapper mapper, IExportService exportService)
        {
            _context = context;
            _configuration = configuration;
            _mapper = mapper;
            _exportService = exportService;
        }
        // GET: Tickets
        [Permission("TICKETS:VIEW")]
        public async Task<IActionResult> ExportRecentTicketsList(TicketViewModel VM)
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

            var data = await alltickets
                .Select(x => new
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    CreatedOn = x.CreatedOn,
                    UserName = x.CreatedBy.FullName,
                    Status = x.Status.Description,
                    Priority = x.Priority.Description,
                    Category = x.SubCategory.Category.Name,
                    SubCategory = x.SubCategory.Name,
                    TicketComment = x.TicketComments.Count,
                }).ToListAsync();
            return _exportService.ExportToExcel(data, "RecentTickets List");
        }

        [Permission("TICKETS:VIEW")]
        public async Task<IActionResult> ExportAssignedTicketsList(TicketViewModel VM)
        {
            var assignedSatatus = await _context.SystemCodeDetails
                .Include(x => x.SystemCode)
                .Where(x => x.SystemCode.Code == "RESOLUTIONSTATUS" && x.Code == "Assigned")
                .FirstOrDefaultAsync();

            var alltickets = _context.Tickets
                .Include(t => t.CreatedBy)
                .Include(t => t.SubCategory)
                .Include(t => t.Priority)
                .Include(t => t.Status)
                .Include(t => t.TicketComments)
                .OrderBy(x => x.CreatedOn)
                .Where(t => t.StatusId == assignedSatatus.Id)
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

            var data = await alltickets
                .Select(x => new
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    CreatedOn = x.CreatedOn,
                    UserName = x.CreatedBy.FullName,
                    Status = x.Status.Description,
                    Priority = x.Priority.Description,
                    Category = x.SubCategory.Category.Name,
                    SubCategory = x.SubCategory.Name,
                    TicketComment = x.TicketComments.Count,
                }).ToListAsync();
            return _exportService.ExportToExcel(data, "AssignedTickets List");
        }

        [Permission("TICKETS:VIEW")]
        public async Task<IActionResult> ExportClosedTicketsList(TicketViewModel VM)
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
                .OrderBy(x => x.CreatedOn)
                .Where(t => t.StatusId == closedstatus.Id)
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

            var data = await alltickets
                .Select(x => new
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    CreatedOn = x.CreatedOn,
                    UserName = x.CreatedBy.FullName,
                    Status = x.Status.Description,
                    Priority = x.Priority.Description,
                    Category = x.SubCategory.Category.Name,
                    SubCategory = x.SubCategory.Name,
                    TicketComment = x.TicketComments.Count,
                }).ToListAsync();
            return _exportService.ExportToExcel(data, "ClosedTickets List");
        }
        [Permission("TICKETS:VIEW")]
        public async Task<IActionResult> ExportResolvedTicketsList(TicketViewModel VM)
        {
            var resolvedStatus = await _context.SystemCodeDetails
              .Include(x => x.SystemCode)
              .Where(x => x.SystemCode.Code == "RESOLUTIONSTATUS" && x.Code == "Resolved")
              .FirstOrDefaultAsync();

            var alltickets = _context.Tickets
                .Include(t => t.CreatedBy)
                .Include(t => t.SubCategory)
                .Include(t => t.Priority)
                .Include(t => t.Status)
                .Include(t => t.TicketComments)
                .OrderBy(x => x.CreatedOn)
                .Where(t => t.StatusId == resolvedStatus.Id)
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

            var data = await alltickets
                .Select(x => new
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    CreatedOn = x.CreatedOn,
                    UserName = x.CreatedBy.FullName,
                    Status = x.Status.Description,
                    Priority = x.Priority.Description,
                    Category = x.SubCategory.Category.Name,
                    SubCategory = x.SubCategory.Name,
                    TicketComment = x.TicketComments.Count,
                }).ToListAsync();
            return _exportService.ExportToExcel(data, "ResolvedTickets List");
        }

        // GET: Comments
        [Permission("comments:view")]
        public async Task<IActionResult> ExportTicketComments(CommentViewModel vm)
        {
            var allComments = _context.Comments
                   .Include(x => x.CreatedBy)
                   .Include(x => x.Ticket)
                   .AsQueryable();
            if (vm != null)
            {
                if (!string.IsNullOrEmpty(vm.Description))
                {
                    allComments = allComments.Where(x => x.Description.Contains(vm.Description));
                }
                if (!string.IsNullOrEmpty(vm.CreatedById))
                {
                    allComments = allComments.Where(x => x.CreatedById == vm.CreatedById);
                }
            }
            var comments = await allComments
                .Select(x => new
                {
                    Id = x.Id,
                    Ticket = x.Ticket.Title,
                    Comment = x.Description,
                    CreatedBy = x.CreatedBy.FullName,
                    CreatedOn = x.CreatedOn
                }).ToArrayAsync();

            return _exportService.ExportToExcel(comments, "TicketComments List");
        }
        // GET: TicketCategories
        [Permission("categories:view")]
        public async Task<IActionResult> ExportTicketCategories(TicketCategoryViewModel VM)
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

            var TicketCategories = await ticketCategories
                .Select(x => new
                {
                    Id = x.Id,
                    CategoryCode = x.Code,
                    CategoryName = x.Name,
                    CreatedBy = x.CreatedBy.FullName,
                    CreatedOn = x.CreatedOn,

                }).ToListAsync();

            return _exportService.ExportToExcel(TicketCategories, "TicketCategories List");
        }
        // GET: TicketSubCategories
        [Permission("subcategories:view")]
        public async Task<IActionResult> ExportTicketSubCategories(int id, TicketSubCategoriesVM VM)
        {
            IQueryable<TicketSubCategory> ticketSubCategories = null;

            if (id == 0)
            {
                ticketSubCategories = _context.TicketSubCategories
                .Include(t => t.Category)
                .Include(t => t.CreatedBy)
                .Include(t => t.ModifiedBy)
                .AsQueryable();
            }
            else
            {
                ticketSubCategories = _context.TicketSubCategories
                   .Include(t => t.Category)
                   .Include(t => t.CreatedBy)
                   .Include(t => t.ModifiedBy)
                   .Where(x => x.CategoryId == id)
                   .AsQueryable();
            }

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
            var TicketSubCategories = await ticketSubCategories
                .Select(x => new
                {
                    Id = x.Id,
                    SubCategoryCode = x.Code,
                    SubCategoryName = x.Name,
                    CategoryName = x.Category.Name,
                    CreatedBy = x.CreatedBy.FullName,
                    CreatedOn = x.CreatedOn,

                }).ToListAsync();

            return _exportService.ExportToExcel(TicketSubCategories, "TicketSubCategories List");

        }
        // GET: AuditTrails
        [Permission("audit:view")]
        public async Task<IActionResult> ExportAuditTrails(AuditTrailViewModel VM)
        {
            var AuditTrails = await _context.AuditTrails
                .Include(a => a.User)
                .OrderBy(x => x.Id)
                .Select(x => new
                {
                    Id = x.Id,
                    Module = x.Module,
                    Action = x.Action,
                    PrimaryKey = x.PrimaryKey,
                    NewValues = x.NewValues,
                    OldValues = x.OldValues,
                    AffectedColumns = x.AffectedColumns,
                    AffectedTable = x.AffectedTable,
                    CreatedBy = x.User.FullName,
                    CreatedOn = x.TimeStamp,

                }).ToListAsync();

            return _exportService.ExportToExcel(AuditTrails, "AuditTrails List");
        }
        // GET: UsersController
        [Permission("users:view")]
        public async Task<ActionResult> ExportUsers(ApplicationUserViewModel VM)
        {
            var ApplicationUsers = await _context.Users
                .Include(x => x.Role)
                .Include(x => x.Gender)
                .Select(x => new
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    MiddleName = x.MiddleName,
                    LastName = x.LastName,
                    FullName = x.FullName,
                    Gender = x.Gender.Description,
                    RoleName = x.Role.Name,
                    EmailAddress = x.Email,
                    City = x.City,
                    Country = x.Country,
                    PhoneNumber = x.PhoneNumber,
                    UserName = x.UserName,
                }).ToListAsync();

            return _exportService.ExportToExcel(ApplicationUsers, "Users List");
        }
        // GET: SystemCodes
        [Permission("systemcodes:view")]
        public async Task<IActionResult> ExportSystemCode(SystemCodeViewModel VM)
        {
            var systemCodes = _context.SystemCodes
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

            var allSystemCodesDetails = await systemCodes
                .Select(x => new
                {
                    Id = x.Id,
                    Code = x.Code,
                    Description = x.Description,
                    CreatedBy = x.CreatedBy.FullName,
                    CreatedOn = x.CreatedOn,
                }).ToListAsync();

            return _exportService.ExportToExcel(allSystemCodesDetails, "SystemCodes List");
        }
        // GET: SystemCodeDetails
        [Permission("systemcodedetails:view")]
        public async Task<IActionResult> ExportSystemCodeDetails(SystemCodeDetailViewModel VM)
        {
            var systemCodeDetails = _context.SystemCodeDetails
                .Include(s => s.SystemCode)
                .Include(s => s.CreatedBy)
                .AsQueryable();

            if (VM != null)
            {
                if (VM != null && !string.IsNullOrEmpty(VM.Code))
                {
                    systemCodeDetails = systemCodeDetails.Where(x => x.Code.Contains(VM.Code));
                }
                if (VM != null && !string.IsNullOrEmpty(VM.CreatedById))
                {
                    systemCodeDetails = systemCodeDetails.Where(x => x.CreatedById == VM.CreatedById);
                }
                if (VM != null && !string.IsNullOrEmpty(VM.Description))
                {
                    systemCodeDetails = systemCodeDetails.Where(x => x.Description.Contains(VM.Description));
                }
                if (VM != null && VM.SystemCodeId > 0)
                {
                    systemCodeDetails = systemCodeDetails.Where(x => x.SystemCodeId == VM.SystemCodeId);
                }
            }

            var allSystemCodeDetails = await systemCodeDetails
                .Select(x => new
                {
                    Id = x.Id,
                    Code = x.Code,
                    Description = x.Description,
                    SystemCode = x.SystemCode.Description,
                    CreatedBy = x.CreatedBy.FullName,
                    CreatedOn = x.CreatedOn,
                }).ToListAsync();

            return _exportService.ExportToExcel(allSystemCodeDetails, "SystemCodeDetails List");
        }
        [Permission("systemrole:view")]
        public async Task<IActionResult> ExportSystemRoles()
        {
            var roles = await _context.Roles
                .Select(x => new
                {
                    RoleId = x.Id,
                    RoleName = x.Name,
                    NormalizedRoleName = x.NormalizedName,
                }).ToListAsync();

            return _exportService.ExportToExcel(roles, "SystemRoles List");
        }
        // GET: Departments
        [Permission("departments:view")]
        public async Task<IActionResult> ExportDepartments()
        {
            var allDepartments = await _context.Departments
                .Include(d => d.CreatedBy)
                .Include(d => d.ModifiedBy)
                .Select(x => new
                {
                    Id = x.Id,
                    DepartmentCode = x.Code,
                    DepartmentName = x.Name,
                    CreatedBy = x.CreatedBy.FullName,
                    CreatedOn = x.CreatedOn,
                }).ToListAsync();

            return _exportService.ExportToExcel(allDepartments, "Departments List");
        }


    }
}
