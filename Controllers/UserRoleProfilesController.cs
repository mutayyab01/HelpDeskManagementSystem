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
using HelpDeskSystem.Services;
using HelpDeskSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using HelpDeskSystem.ClaimManagement;

namespace HelpDeskSystem.Controllers
{
    [Authorize]
    public class UserRoleProfilesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserRoleProfilesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: UserRoleProfiles
        [Permission("userprofile:view")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.UserRoleProfiles
                .Include(u => u.CreatedBy).Include(u => u.ModifiedBy)
                .Include(u => u.Role)
                .Include(u => u.Task);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: UserRoleProfiles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userRoleProfile = await _context.UserRoleProfiles
                .Include(u => u.CreatedBy)
                .Include(u => u.ModifiedBy)
                .Include(u => u.Role)
                .Include(u => u.Task)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userRoleProfile == null)
            {
                return NotFound();
            }

            return View(userRoleProfile);
        }



        public async Task<IActionResult> UserRights(string id)
        {
            ProfileViewModel VM = new();
            VM.RoleId = id;

            var allroles = await _context.Roles.OrderBy(x => x.Name).ToListAsync();
            ViewData["RoleId"] = new SelectList(allroles, "Id", "Name", id);

            VM.SystemTasks = await _context.SystemTasks
                .Include(x => x.Parent)
                .Include("ChildTasks.ChildTasks.ChildTasks")
                .OrderBy(x => x.OrderNo)
                .Where(x => x.Parent == null)
                .ToListAsync();

            VM.RightIdsAssigned = await _context.UserRoleProfiles
                .Where(x => x.RoleId == id).Select(x => x.TaskId)
                .ToListAsync();

            return View(VM);
        }

        [HttpPost]
        public async Task<IActionResult> UserRights(ProfileViewModel VM)
        {
            try
            {
                var allprofiles = _context.UserRoleProfiles.Where(x => x.RoleId == VM.RoleId).ToList();
                _context.UserRoleProfiles.RemoveRange(allprofiles);
                foreach (var TaskId in VM.Ids)
                {
                    var rightprofile = new UserRoleProfile
                    {
                        TaskId = TaskId,
                        RoleId = VM.RoleId,
                        CreatedOn = DateTime.Now,
                        CreatedById = User.GetUserId()
                    };
                    _context.UserRoleProfiles.Add(rightprofile);
                    TempData["MESSEGE"] = "Role Right Assigned Successfully";

                }
                await _context.SaveChangesAsync(User.GetUserId());

            }
            catch (Exception ex)
            {
                TempData["ERROR"] = "There Was an Issue Assigning Right to the Role" + ex.Message;
               
            }


            var allroles = await _context.Roles.OrderBy(x => x.Name).ToListAsync();
            ViewData["RoleId"] = new SelectList(allroles, "Id", "Name", VM.RoleId);

            VM.SystemTasks = await _context.SystemTasks
                .Include(x => x.Parent)
                .Include("ChildTasks.ChildTasks.ChildTasks")
                .OrderBy(x => x.OrderNo)
                .Where(x => x.Parent == null)
                .ToListAsync();

            VM.RightIdsAssigned = await _context.UserRoleProfiles
                .Where(x => x.RoleId == VM.RoleId).Select(x => x.TaskId)
                .ToListAsync();

            return View(VM);
        }



        // GET: UserRoleProfiles/Create
        public IActionResult Create()
        {

            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Name");
            ViewData["TaskId"] = new SelectList(_context.SystemTasks, "Id", "Name");
            return View();
        }

        // POST: UserRoleProfiles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserRoleProfile userRoleProfile)
        {

            var UserId = User.GetUserId();
            userRoleProfile.CreatedOn = DateTime.Now;
            userRoleProfile.CreatedById = UserId;

            _context.Add(userRoleProfile);
            await _context.SaveChangesAsync(UserId);
            return RedirectToAction(nameof(Index));

            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Name", userRoleProfile.RoleId);
            ViewData["TaskId"] = new SelectList(_context.SystemTasks, "Id", "Name", userRoleProfile.TaskId);
            return View(userRoleProfile);
        }

        // GET: UserRoleProfiles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userRoleProfile = await _context.UserRoleProfiles.FindAsync(id);
            if (userRoleProfile == null)
            {
                return NotFound();
            }

            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Name", userRoleProfile.RoleId);
            ViewData["TaskId"] = new SelectList(_context.SystemTasks, "Id", "Name", userRoleProfile.TaskId);
            return View(userRoleProfile);
        }

        // POST: UserRoleProfiles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UserRoleProfile userRoleProfile)
        {
            if (id != userRoleProfile.Id)
            {
                return NotFound();
            }


            try
            {

                var UserId = User.GetUserId();
                userRoleProfile.ModifiedOn = DateTime.Now;
                userRoleProfile.ModifiedById = UserId;

                _context.Update(userRoleProfile);
                await _context.SaveChangesAsync(UserId);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserRoleProfileExists(userRoleProfile.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));


            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Name", userRoleProfile.RoleId);
            ViewData["TaskId"] = new SelectList(_context.SystemTasks, "Id", "Name", userRoleProfile.TaskId);
            return View(userRoleProfile);
        }

        // GET: UserRoleProfiles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userRoleProfile = await _context.UserRoleProfiles
                .Include(u => u.CreatedBy)
                .Include(u => u.ModifiedBy)
                .Include(u => u.Role)
                .Include(u => u.Task)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userRoleProfile == null)
            {
                return NotFound();
            }

            return View(userRoleProfile);
        }

        // POST: UserRoleProfiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var UserId = User.GetUserId();
            var userRoleProfile = await _context.UserRoleProfiles.FindAsync(id);
            if (userRoleProfile != null)
            {
                _context.UserRoleProfiles.Remove(userRoleProfile);
            }

            await _context.SaveChangesAsync(UserId);
            return RedirectToAction(nameof(Index));
        }

        private bool UserRoleProfileExists(int id)
        {
            return _context.UserRoleProfiles.Any(e => e.Id == id);
        }
    }
}
