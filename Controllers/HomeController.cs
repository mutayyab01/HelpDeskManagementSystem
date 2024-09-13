using HelpDeskSystem.ClaimManagement;
using HelpDeskSystem.Data;
using HelpDeskSystem.Models;
using HelpDeskSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace HelpDeskSystem.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;

        }
        [Permission("dash:view")]
        public async Task<IActionResult> Index(TicketDashboardViewModel VM)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return this.Redirect("~/Identity/account/login");
            }
            else
            {
                //  var alluserpermission = User.FindFirst(c => c.Type == "UserPermission")?.Value ?? "";
               VM.TicketsSummary = await _context.TicketsSummaryView.FirstOrDefaultAsync();
               VM.TicketsPriority = await _context.TicketsPriorityView.FirstOrDefaultAsync();

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
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
