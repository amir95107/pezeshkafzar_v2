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
    public class FeaturesController : Controller
    {
        private readonly IFeatureRepository _featureRepository;

        public FeaturesController(IFeatureRepository featureRepository)
        {
            _featureRepository = featureRepository;
        }

        // GET: Admin/Features
        public async Task<IActionResult> Index()
        {
            return View(await _featureRepository.GetAllFeaturesAsync());
        }

        // GET: Admin/Features/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var features = await _featureRepository.FindAsync(id.Value);
            if (features == null)
            {
                return NotFound();
            }
            return View(features);
        }

        // GET: Admin/Features/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Features/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Features features)
        {
            if (ModelState.IsValid)
            {
                await _featureRepository.AddAsync(features);
                await _featureRepository.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(features);
        }

        //// GET: Admin/Features/Edit/5
        //public Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Features features = db.Features.Find(id);
        //    if (features == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(features);
        //}

        //// POST: Admin/Features/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public Task<IActionResult> Edit([Bind(Include = "FeatureID,FeatureTitle")] Features features)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(features).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(features);
        //}

        // GET: Admin/Features/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Features features = await _featureRepository.FindAsync(id.Value);
            if (features == null)
            {
                return NotFound();
            }
            return View(features);
        }

        // POST: Admin/Features/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            Features features = await _featureRepository.FindAsync(id);
            features.RemovedAt = DateTime.UtcNow;
            _featureRepository.Modify(features);
            await _featureRepository.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
