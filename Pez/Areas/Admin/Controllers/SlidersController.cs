using DataLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pezeshkafzar_v2.Repositories;
using System.Net;
using static Pezeshkafzar_v2.Utilities.ImageExtentions;

namespace MyEshop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class SlidersController : Controller
    {
        private readonly ISliderRepository _sliderRepository;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public SlidersController(ISliderRepository sliderRepository,
            IWebHostEnvironment hostingEnvironment)
        {
            _sliderRepository = sliderRepository;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: Admin/Sliders
        public async Task<IActionResult> Index()
        {
            return View(await _sliderRepository.GetAllAsync());
        }

        // GET: Admin/Sliders/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Slider slider = await _sliderRepository.FindAsync(id.Value);
            if (slider == null)
            {
                return NotFound();
            }
            return View(slider);
        }

        // GET: Admin/Sliders/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Sliders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Slider slider, IFormFile imgUp)
        {
            if (true)
            {
                if (imgUp == null)
                {
                    ModelState.AddModelError("ImageName", "لطفا تصویر را انتخاب کنید");
                    return View(slider);
                }
                if (imgUp != null && imgUp.IsImage())
                {

                    slider.ImageName = slider.ImageName + "_" + Guid.NewGuid().ToString() + Path.GetExtension(imgUp.FileName);
                    string imagesPath = Path.Combine(_hostingEnvironment.WebRootPath, "images", "slider");
                    imgUp.UploadAsync(imagesPath + "/" + slider.ImageName);
                }
                await _sliderRepository.AddAsync(slider);
                await _sliderRepository.SaveChangesAsync();
                return RedirectToAction("Index");
            }
        }

        // GET: Admin/Sliders/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Slider slider = await _sliderRepository.FindAsync(id.Value);
            if (slider == null)
            {
                return NotFound();
            }
            return View(slider);
        }

        // POST: Admin/Sliders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Slider slider, IFormFile imgUp)
        {
            if (true)
            {
                if (imgUp != null && imgUp.IsImage())
                {
                    slider.ImageName = slider.ImageName + "_" + Guid.NewGuid().ToString() + Path.GetExtension(imgUp.FileName);
                    string imagesPath = Path.Combine(_hostingEnvironment.WebRootPath, "images", "slider");
                    System.IO.File.Delete(imagesPath + slider.ImageName);

                    imgUp.UploadAsync(imagesPath + "/" + slider.ImageName);
                }
                _sliderRepository.Modify(slider);
                return RedirectToAction("Index");
            }

        }

        // GET: Admin/Sliders/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Slider slider = await _sliderRepository.FindAsync(id.Value);
            if (slider == null)
            {
                return NotFound();
            }
            return View(slider);
        }

        // POST: Admin/Sliders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            Slider slider = await _sliderRepository.FindAsync(id);
            string imagesPath = Path.Combine(_hostingEnvironment.WebRootPath, "images", "slider");

            System.IO.File.Delete(Path.Combine(imagesPath, slider.ImageName));
            _sliderRepository.Remove(slider);
            await _sliderRepository.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
