using KEPSAS.TTS.Data;
using KEPSAS.TTS.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KEPSAS.TTS.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.AsNoTracking().ToListAsync();
            var list = new List<UserRowVm>();
            foreach (var u in users)
            {
                var roles = await _userManager.GetRolesAsync(u);
                list.Add(new UserRowVm
                {
                    Id = u.Id,
                    UserName = u.UserName!,
                    Ad = u.Ad,
                    Soyad = u.Soyad,
                    Email = u.Email,
                    IsAdmin = roles.Contains("Admin")
                });
            }
            return View(list.OrderBy(u => u.Ad).ThenBy(u => u.Soyad));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleRole(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user is null) return RedirectToAction(nameof(Index));

            // Rolleri garanti et
            foreach (var r in new[] { "Admin", "User" })
                if (!await _roleManager.RoleExistsAsync(r))
                    await _roleManager.CreateAsync(new IdentityRole(r));

            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains("Admin"))
            {
                await _userManager.RemoveFromRoleAsync(user, "Admin");
                if (!roles.Contains("User"))
                    await _userManager.AddToRoleAsync(user, "User");
            }
            else
            {
                if (roles.Contains("User"))
                    await _userManager.RemoveFromRoleAsync(user, "User");
                await _userManager.AddToRoleAsync(user, "Admin");
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
