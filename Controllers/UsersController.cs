using AutoMapper;
using DocumentFormat.OpenXml.Spreadsheet;
using ElmahCore;
using HelpDeskSystem.ClaimManagement;
using HelpDeskSystem.Data;
using HelpDeskSystem.Data.Migrations;
using HelpDeskSystem.Models;
using HelpDeskSystem.Services;
using HelpDeskSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;
using System.Security.Claims;

namespace HelpDeskSystem.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UsersController(ApplicationDbContext context, SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _mapper = mapper;
        }
        // GET: UsersController
        [Permission("users:view")]
        public async Task<ActionResult> Index(ApplicationUserViewModel VM)
        {
            var allusers = _context.Users
                .Include(x => x.Role)
                .Include(x => x.Gender)
                .AsQueryable();
            if (VM != null && !string.IsNullOrEmpty(VM.RoleId?.Trim()))
            {
                allusers = allusers.Where(x => x.RoleId == VM.RoleId.Trim());
            }
            if (VM != null && !string.IsNullOrEmpty(VM.FirstName?.Trim()))
            {
                allusers = allusers.Where(x => x.FirstName.Contains(VM.FirstName.Trim()));
            }
            if (VM != null && !string.IsNullOrEmpty(VM.MiddleName?.Trim()))
            {
                allusers = allusers.Where(x => x.MiddleName.Contains(VM.MiddleName.Trim()));
            }
            if (VM != null && !string.IsNullOrEmpty(VM.LastName?.Trim()))
            {
                allusers = allusers.Where(x => x.LastName.Contains(VM.LastName.Trim()));
            }
            if (VM != null && !string.IsNullOrEmpty(VM.PhoneNumber?.Trim()))
            {
                allusers = allusers.Where(x => x.PhoneNumber.Contains(VM.PhoneNumber.Trim()));
            }
            if (VM != null && !string.IsNullOrEmpty(VM.Email?.Trim()))
            {
                allusers = allusers.Where(x => x.Email.Contains(VM.Email.Trim()));
            }

            VM.ApplicationUsers = await allusers.ToListAsync();

            ViewData["RoleId"] = new SelectList(_context.Roles.ToList(), "Id", "Name");
            return View(VM);
        }

        // GET: UsersController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UsersController/Create
        [Permission($"users:{nameof(Create)}")]

        public ActionResult Create()
        {
            ViewData["GenderId"] = new SelectList(_context.SystemCodeDetails.Include(x => x.SystemCode).Where(x => x.SystemCode.Code == "GENDER"), "Id", "Code");
            ViewData["RoleId"] = new SelectList(_context.Roles.ToList(), "Id", "Name");

            return View();
        }

        // POST: UsersController/Create
        [Permission($"users:{nameof(Create)}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ApplicationUser user)
        {
            ViewData["GenderId"] = new SelectList(_context.SystemCodeDetails.Include(x => x.SystemCode).Where(x => x.SystemCode.Code == "GENDER"), "Id", "Code", user.GenderId);
            ViewData["RoleId"] = new SelectList(_context.Roles.ToList(), "Id", "Name", user.RoleId);
            try
            {
                var rolename = await _context.Roles.Where(x => x.Id == user.RoleId).FirstOrDefaultAsync();

                var UserId = User.GetUserId();
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
                Registereduser.GenderId = user.GenderId;
                Registereduser.RoleId = user.RoleId;
                Registereduser.PhoneNumber = user.PhoneNumber;
                Registereduser.PhoneNumberConfirmed = user.PhoneNumberConfirmed;
                var result = await _userManager.CreateAsync(Registereduser, user.PasswordHash);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(Registereduser, rolename.Name);
                    TempData["MESSEGE"] = "System User Created Successfully";
                    return RedirectToAction(nameof(Index));

                }
                else
                {
                    TempData["ERROR"] = result.Errors.FirstOrDefault()?.Description ?? "An unknown error occurred.";
                    return View();
                }
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        [Permission($"users:{nameof(ChangePassword)}")]
        public async Task<IActionResult> ChangePassword(string id, ResetPasswordViewModel VM)
        {

            var User = await _context.Users.Where(x => x.Id == id).FirstOrDefaultAsync();
            VM.Id = User.Id;
            VM.Email = User.Email;
            VM.FullName = User.FullName;
            VM.FirstName = User.FirstName;
            VM.MiddleName = User.MiddleName;
            VM.LastName = User.LastName;
            VM.RoleId = User.RoleId;
            VM.GenderId = User.GenderId;
            if (VM.GenderId > 0)
            {
                VM.Gender = await _context.SystemCodeDetails.Where(x => x.Id == VM.GenderId).FirstOrDefaultAsync();
            }
            if (!string.IsNullOrEmpty(VM.RoleId))
            {
                VM.Role = await _context.Roles.Where(x => x.Id == VM.RoleId).FirstOrDefaultAsync();
            }
            return View(VM);
        }

        [Permission($"users:{nameof(ChangePassword)}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmChangePassword(ResetPasswordViewModel VM)
        {
            try
            {
                var user = await _context.Users.Where(x => x.Id == VM.Id).FirstOrDefaultAsync();
                await _userManager.RemovePasswordAsync(user);
                var result = await _userManager.AddPasswordAsync(user, VM.ConfirmPassword);
                if (result.Succeeded)
                {
                    user.LockoutEnabled = true;
                    user.LockoutEnd = null;
                    user.AccessFailedCount = 0;

                    _context.Users.Update(user);
                    await _context.SaveChangesAsync(User.GetUserId());
                    TempData["MESSEGE"] = "Password reset Successfully";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Error"] = "Password Can't reset Successfully";
                    return RedirectToAction("ChangePassword", VM);
                }
            }
            catch (Exception ex)
            {
                ElmahExtensions.RaiseError(ex);
                TempData["Error"] = "Password Details Can't reset Successfully " + ex.Message;
                return RedirectToAction("ChangePassword", VM);
            }
        }

        [Permission($"users:{nameof(ActivateUser)}")]
        public async Task<IActionResult> ActivateUser(string id, ResetPasswordViewModel VM)
        {

            var User = await _context.Users.Where(x => x.Id == id).FirstOrDefaultAsync();
            VM.Id = User.Id;
            VM.Email = User.Email;
            VM.FullName = User.FullName;
            VM.FirstName = User.FirstName;
            VM.MiddleName = User.MiddleName;
            VM.LastName = User.LastName;
            VM.RoleId = User.RoleId;
            VM.GenderId = User.GenderId;
            if (VM.GenderId > 0)
            {
                VM.Gender = await _context.SystemCodeDetails.Where(x => x.Id == VM.GenderId).FirstOrDefaultAsync();
            }
            if (!string.IsNullOrEmpty(VM.RoleId))
            {
                VM.Role = await _context.Roles.Where(x => x.Id == VM.RoleId).FirstOrDefaultAsync();
            }
            return View(VM);
        }

        [Permission($"users:{nameof(ActivateUser)}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmActivateUser(ResetPasswordViewModel VM)
        {
            try
            {
                var user = await _context.Users.Where(x => x.Id == VM.Id).FirstOrDefaultAsync();


                if (user != null)
                {
                    user.LockoutEnabled = true;
                    user.LockoutEnd = null;
                    user.AccessFailedCount = 0;
                    user.IsLocked = false;

                    _context.Users.Update(user);
                    await _context.SaveChangesAsync(User.GetUserId());
                    TempData["MESSEGE"] = "User Account Acticated Successfully";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Error"] = "User Account Can't Acticated Successfully";
                    return RedirectToAction("ActivateUser", VM);
                }
            }
            catch (Exception ex)
            {
                ElmahExtensions.RaiseError(ex);
                TempData["Error"] = "User Account Can't Acticated Successfully " + ex.Message;
                return RedirectToAction("ActivateUser", VM);
            }
        }
        // GET: UsersController/Edit/5

        [Permission($"users:{nameof(DeactivateUser)}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeactivateUser(string id)
        {
            try
            {
                var user = await _context.Users.Where(x => x.Id == id).FirstOrDefaultAsync();


                if (user != null)
                {
                    user.LockoutEnabled = true;
                    user.LockoutEnd = null;
                    user.AccessFailedCount = 0;
                    user.IsLocked = true;

                    _context.Users.Update(user);
                    await _context.SaveChangesAsync(User.GetUserId());
                    TempData["MESSEGE"] = "User Account Deacticated Successfully";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Error"] = "User Account Can't Deacticated Successfully";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ElmahExtensions.RaiseError(ex);
                TempData["Error"] = "User Account Can't Deacticated Successfully " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        [Permission($"users:{nameof(ChangeRole)}")]
        public async Task<IActionResult> ChangeRole(string id, ResetPasswordViewModel VM)
        {
            var User = await _context.Users.Where(x => x.Id == id).FirstOrDefaultAsync();

            var allroles = await _context.Roles.OrderBy(x => x.Name).ToListAsync();
            ViewData["RoleId"] = new SelectList(allroles, "Id", "Name", id);

            return View(User);
        }

        [Permission($"users:{nameof(ChangeRole)}")]
        [HttpPost, ActionName("ChangeRole")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmChangeRole(string id, ApplicationUser user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            try
            {
                var existingUser = await _userManager.FindByIdAsync(id);
                if (existingUser == null)
                {
                    return NotFound();
                }

                var currentRoles = await _userManager.GetRolesAsync(existingUser);
                if (currentRoles.Any())
                {
                    await _userManager.RemoveFromRolesAsync(existingUser, currentRoles);
                }

                var newRole = await _context.Roles
                    .Where(r => r.Id == user.RoleId)
                    .Select(r => r.Name)
                    .FirstOrDefaultAsync();

                if (newRole != null)
                {
                    await _userManager.AddToRoleAsync(existingUser, newRole);

                    existingUser.RoleId = user.RoleId;
                    _context.Users.Update(existingUser);
                    await _context.SaveChangesAsync(User.GetUserId());
                }

                TempData["MESSEGE"] = "User Role Updated Successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ElmahExtensions.RaiseError(ex);
                TempData["Error"] = "An error occurred while changing the role: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        [Permission($"users:{nameof(Edit)}")]
        public async Task<IActionResult> Edit(string id)
        {
            var applicationUserViewModel = new ApplicationUserViewModel();
            if (id == null)
            {
                return NotFound();
            }

            var applicationUser = await _context.Users.FindAsync(id);
            var VM = _mapper.Map<ApplicationUserViewModel>(applicationUser);

            if (applicationUser == null)
            {
                return NotFound();
            }
            ViewData["SelectedCityId"] = VM.CityId;
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Name");
            ViewData["GenderId"] = new SelectList(_context.SystemCodeDetails.Include(x => x.SystemCode).Where(x => x.SystemCode.Code == "Gender"), "Id", "Description");
            return View(VM);
        }

        // POST: UsersController/Edit/5
        [Permission($"users:{nameof(Edit)}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, ApplicationUserViewModel user)
        {
            var existingUser = await _userManager.FindByIdAsync(id);
            if (existingUser == null)
            {
                return NotFound();
            }
            if (id != user.Id)
            {
                return NotFound();
            }
            try
            {
                if (existingUser.Email != user.Email)
                {
                    TempData["Error"] = "You Can't Edit Your Email ";
                    ViewData["CityId"] = new SelectList(_context.Cities.Where(c => c.CountryId == user.CountryId), "Id", "Name", user.CityId);
                    ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Name", user.CountryId);
                    ViewData["GenderId"] = new SelectList(_context.SystemCodeDetails.Include(x => x.SystemCode).Where(x => x.SystemCode.Code == "Gender"), "Id", "Description", user.GenderId);
                    return View();
                }
                // ✅ Only update selected fields
                existingUser.FirstName = user.FirstName;
                existingUser.MiddleName = user.MiddleName;
                existingUser.LastName = user.LastName;
                existingUser.UserName = user.UserName;
                existingUser.NormalizedUserName = user.UserName?.ToUpper();
                existingUser.PhoneNumber = user.PhoneNumber;
                existingUser.GenderId = user.GenderId;
                existingUser.CountryId = user.CountryId;
                existingUser.CityId = user.CityId;
                TempData["MESSEGE"] = "User Details Updated Successfully";
                await _context.SaveChangesAsync(User.GetUserId());
            }
            catch (Exception ex)
            {
                ElmahExtensions.RaiseError(ex);
                TempData["Error"] = "An error occurred while Updating the User: " + ex.Message;
                return RedirectToAction("Index");
            }
            ViewData["CityId"] = new SelectList(_context.Cities.Where(c => c.CountryId == user.CountryId), "Id", "Name", user.CityId);
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Name", user.CountryId);
            ViewData["GenderId"] = new SelectList(_context.SystemCodeDetails.Include(x => x.SystemCode).Where(x => x.SystemCode.Code == "Gender"), "Id", "Description", user.GenderId);
            return RedirectToAction(nameof(Index));
        }

        // GET: UsersController/Delete/5
        [Permission($"users:{nameof(Delete)}")]

        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UsersController/Delete/5
        [Permission($"users:{nameof(Delete)}")]
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
