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

        public async Task<IActionResult> Index([FromQuery] int page = 1, [FromQuery] int take=20)
        {
            //var users = await _userRepository.GetAllUsersAsync(take, skip);
            var users = await UserManager.Users.Include(x => x.UserInfo).Skip(take * (page - 1)).Take(take).ToListAsync();
            ViewBag.PageCount = (users.Count) / take + 1;
            ViewBag.PageNumber = page;
            ViewBag.Take = take;
            return View(users);
        }

        public async Task<IActionResult> Edit([FromRoute] Guid id)
        {
            var user = await _userRepository.FindAsync(id);
            ViewBag.Roles = await RoleManager.Roles.Select(x => x.Name).ToListAsync();
            ViewBag.CurrentRole = await UserManager.GetRolesAsync(user);
            ViewBag.Mobile = user.PhoneNumber;
            return View(new EditUserRoleViewModel
            {
                UserId = user.Id
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
                    ModelState.AddModelError(userRole.Role, "نقش انتخاب نشده.");
                    return View(userRole);
                }

                return RedirectToAction("index");
            }

            ModelState.AddModelError(userRole.Role, "نقش انتخاب نشده.");
            return View(userRole);
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var user = await _userRepository.FindAsync(id);
            if (user is null)
            {
                ViewBag.Error = "کاربر یافت نشد.";
                return RedirectToAction("Index");
            }

            user.IsActive = !user.IsActive;
            _userRepository.Modify(user);
            await _userRepository.SaveChangesAsync();
            return RedirectToAction("Index");
        }



    }
}