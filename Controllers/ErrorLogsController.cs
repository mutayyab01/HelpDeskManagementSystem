using HelpDeskSystem.ClaimManagement;
using Microsoft.AspNetCore.Mvc;

namespace HelpDeskSystem.Controllers
{
    public class ErrorLogsController : Controller
    {
        [Permission("errorlogs:view")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
