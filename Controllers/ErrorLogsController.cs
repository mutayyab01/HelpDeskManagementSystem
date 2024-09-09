using Microsoft.AspNetCore.Mvc;

namespace HelpDeskSystem.Controllers
{
    public class ErrorLogsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
