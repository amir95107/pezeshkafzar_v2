using DataLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Pezeshkafzar_v2.Repositories;
using System.Net;

namespace Pezeshkafzar_v2.Areas.Admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = "admin")]
    public class DeliveryWaysController : Controller
    {
        private readonly IDeliverWaysRepository _deliverWaysRepository;

        public DeliveryWaysController(IDeliverWaysRepository deliverWaysRepository)
        {
            _deliverWaysRepository = deliverWaysRepository;
        }

        // GET: Admin/DeliveryWays
        public async Task<IActionResult> Index()
        {
            return View(await _deliverWaysRepository.GetAllWays());
        }

        // GET: Admin/DeliveryWays/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            DeliveryWays deliveryWays = await _deliverWaysRepository.FindAsync(id.Value);
            if (deliveryWays == null)
            {
                return NotFound();
            }
            return View(deliveryWays);
        }

        // GET: Admin/DeliveryWays/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/DeliveryWays/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DeliveryWaysDto deliveryWays)
        {
            if (ModelState.IsValid)
            {
                await _deliverWaysRepository.AddAsync(new DeliveryWays
                {
                    Description=deliveryWays.Description,
                    Time=string.Empty,
                    Title=deliveryWays.Title,
                    Price=deliveryWays.Price,
                    IsActive=deliveryWays.IsActive,
                    PayByCustomer=deliveryWays.PayByCustomer,
                    Usage=string.Empty
                });
                await _deliverWaysRepository.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(deliveryWays);
        }

        // GET: Admin/DeliveryWays/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            DeliveryWays deliveryWays = await _deliverWaysRepository.FindAsync(id.Value);
            if (deliveryWays == null)
            {
                return NotFound();
            }
            return View(deliveryWays);
        }

        // POST: Admin/DeliveryWays/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DeliveryWays deliveryWays)
        {
            if (ModelState.IsValid)
            {
                _deliverWaysRepository.Modify(deliveryWays);
                await _deliverWaysRepository.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(deliveryWays);
        }

        // GET: Admin/DeliveryWays/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            DeliveryWays deliveryWays = await _deliverWaysRepository.FindAsync(id.Value);
            if (deliveryWays == null)
            {
                return NotFound();
            }
            return View(deliveryWays);
        }

        // POST: Admin/DeliveryWays/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            DeliveryWays deliveryWays = await _deliverWaysRepository.FindAsync(id);
            _deliverWaysRepository.Remove(deliveryWays);
            await _deliverWaysRepository.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
