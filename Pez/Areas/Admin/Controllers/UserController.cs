using DataLayer.Models;
using Humanizer.Localisation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Pezeshkafzar_v2.Repositories;
using Pezeshkafzar_v2.ViewModels;

namespace Pezeshkafzar_v2.Areas.Admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = "admin")]
    public class UserController : Controller
    {
        private readonly UserManager<Users> UserManager;
        private readonly IUserRepository _userRepository;
        private readonly RoleManager<IdentityRole<Guid>> RoleManager;

        public UserController(UserManager<Users> userManager, IUserRepository userRepository, RoleManager<IdentityRole<Guid>> roleManager)
        {
            UserManager = userManager;
            _userRepository = userRepository;
            RoleManager = roleManager;
        }

        public async Task<IActionResult> Index([FromQuery] int take, [FromQuery] int skip)
        {
            //var users = await _userRepository.GetAllUsersAsync(take, skip);
            var users = await UserManager.Users.ToListAsync();
            return View(users);
        }

        [Route("edit/{id}")]
        public async Task<IActionResult> Edit([FromRoute] Guid id)
        {
            var user = await _userRepository.FindAsync(id);
            ViewBag.Roles = await RoleManager.Roles.Select(x => x.Name).ToListAsync();
            ViewBag.CurrentRole = await UserManager.GetRolesAsync(user);
            return View(new EditUserRoleViewModel
            {
                UserId = user.Id,
                Mobile= user.PhoneNumber
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditUserRoleViewModel userRole)
        {
            if (ModelState.IsValid)
            {
                var user = await _userRepository.FindAsync(userRole.UserId);
                try
                {
                    await UserManager.AddToRoleAsync(user, userRole.Role);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return View(user);
            }

            ModelState.AddModelError(userRole.Role, "نقش انتخاب نشده.");
            return View(userRole);
        }



    }
}