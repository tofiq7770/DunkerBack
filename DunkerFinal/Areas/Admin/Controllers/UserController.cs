using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Service.ViewModels.UserVMS;

namespace DunkerFinal.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            var roles = await _roleManager.Roles.ToListAsync();

            // Ensure roles are available
            if (roles == null)
            {
                return View("Error"); // Handle the error as needed
            }

            var userRoles = new List<UserVM>();
            foreach (var user in users)
            {
                var userRolesStr = string.Join(", ", await _userManager.GetRolesAsync(user));
                var userRoleNames = await _userManager.GetRolesAsync(user);

                // Filter out roles that the user does not have
                var availableRoles = roles
                    .Where(role => !userRoleNames.Contains(role.Name))
                    .Select(role => new { role.Name })
                    .ToList();

                userRoles.Add(new UserVM
                {
                    UserName = user.UserName,
                    FullName = user.FullName,
                    Email = user.Email,
                    Roles = userRolesStr,
                    UserId = user.Id
                });

                // Pass the available roles for this user to ViewBag
                ViewBag.AvailableRoles = new SelectList(availableRoles, "Name", "Name");
            }

            ViewBag.Roles = new SelectList(roles, "Name", "Name"); // Ensure all roles are available
            return View(userRoles);
        }


        [HttpGet]
        public async Task<IActionResult> AddRole()
        {

            ViewBag.Users = new SelectList(await _userManager.Users.ToListAsync(), "Id", "UserName");
            ViewBag.Roles = new SelectList(await _roleManager.Roles.ToListAsync(), "Id", "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRole(AddRoleVM request)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            var role = await _roleManager.FindByIdAsync(request.RoleId);

            if (user != null && role != null)
            {
                await _userManager.AddToRoleAsync(user, role.Name);
            }

            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveRole(RemoveRoleVM request)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);

            var role = await _roleManager.FindByNameAsync(request.RoleName);

            if (user != null && role != null)
            {
                await _userManager.RemoveFromRoleAsync(user, request.RoleName);
            }

            return RedirectToAction("Index");
        }

    }
}
