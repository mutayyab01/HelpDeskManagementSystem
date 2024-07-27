using HelpDeskSystem.Data;
using HelpDeskSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HelpDeskSystem.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;
        public UsersController(ApplicationDbContext context, SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }
        // GET: UsersController
        public async Task<ActionResult> Index()
        {
            var users = await _context.Users.ToListAsync();
            return View(users);
        }

        // GET: UsersController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UsersController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UsersController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ApplicationUser user)
        {
            try
            {
                var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                ApplicationUser Registereduser = new ApplicationUser();
                Registereduser.Email = user.Email;
                Registereduser.EmailConfirmed = user.EmailConfirmed;
                Registereduser.FirstName = user.FirstName;
                Registereduser.MiddleName = user.MiddleName;
                Registereduser.LastName = user.LastName;
                Registereduser.City = user.City;
                Registereduser.Country = user.Country;
                Registereduser.UserName = user.UserName;
                Registereduser.NormalizedUserName = user.NormalizedUserName;
                Registereduser.Gender = user.Gender;
                Registereduser.PhoneNumber = user.PhoneNumber;
                Registereduser.PhoneNumberConfirmed = user.PhoneNumberConfirmed;
                var result = await _userManager.CreateAsync(Registereduser, user.PasswordHash);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));

                }
                else
                {

                    return View();
                }
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        // GET: UsersController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UsersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UsersController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UsersController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
