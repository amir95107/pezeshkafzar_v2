using DataLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Pezeshkafzar_v2.Repositories;
using Pezeshkafzar_v2.Utilities;

namespace Pezeshkafzar_v2.Areas.Admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = "admin")]
    public class DiscountsController : Controller
    {
        private readonly IDiscountRepository _discountRepository;
        private readonly UserManager<Users> _userManager;
        private readonly IUserRepository _userRepository;


        public DiscountsController(IDiscountRepository discountRepository,
            UserManager<Users> userManager,
            IUserRepository userRepository)
        {
            _discountRepository = discountRepository;
            _userManager = userManager;
            _userRepository = userRepository;
        }

        // GET: Admin/Discounts
        public async Task<IActionResult> Index()
        {
            return View(await _discountRepository.GetDiscountsWithUserAsync());
        }

        // GET: Admin/Discounts/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Discounts discounts = await _discountRepository.FindAsync(id.Value);
            if (discounts == null)
            {
                return NotFound();
            }
            return View(discounts);
        }

        // GET: Admin/Discounts/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.UserId = new SelectList(await _userManager.GetUsersInRoleAsync("Customer"), "UserId", "PhoneNumber");
            return View();
        }

        // POST: Admin/Discounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Discounts discounts)
        {
            if (ModelState.IsValid)
            {
                await _discountRepository.AddAsync(discounts);
                await _discountRepository.SaveChangesAsync();
                var user = await _userRepository.FindAsync(discounts.UserId);
                if (user != null)
                {
                    var name = user.UserInfo.FirstOrDefault()?.FirstName + user.UserInfo.FirstOrDefault()?.LastName;
                    SendSMS.SendDiscountMessage(user.PhoneNumber, name, discounts.DiscountCode, discounts.MaxDiscountValue.ToString(), discounts.ExpireDate.ToString("yyyy/MM/dd"));

                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.UserId = new SelectList(await _userManager.GetUsersInRoleAsync("Customer"), "UserId", "PhoneNumber", discounts.UserId);
                    return View(discounts);
                }
            }

            ViewBag.UserId = new SelectList(await _userManager.GetUsersInRoleAsync("Customer"), "UserId", "PhoneNumber", discounts.UserId);
            return View(discounts);
        }        

        // GET: Admin/Discounts/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Discounts discounts = await _discountRepository.FindAsync(id.Value);
            if (discounts == null)
            {
                return NotFound();
            }
            ViewBag.UserId = new SelectList(await _userManager.GetUsersInRoleAsync("Customer"), "UserId", "PhoneNumber", discounts.UserId);
            return View(discounts);
        }

        // POST: Admin/Discounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Discounts discounts)
        {
            if (ModelState.IsValid)
            {
                _discountRepository.Modify(discounts);
                await _discountRepository.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(await _userManager.GetUsersInRoleAsync("Customer"), "UserId", "PhoneNumber", discounts.UserId);
            return View(discounts);
        }

        // GET: Admin/Discounts/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Discounts discounts = await _discountRepository.FindAsync(id.Value);
            if (discounts == null)
            {
                return NotFound();
            }
            return View(discounts);
        }

        // POST: Admin/Discounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            Discounts discounts = await _discountRepository.FindAsync(id);
            _discountRepository.Remove(discounts);
            await _discountRepository.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
