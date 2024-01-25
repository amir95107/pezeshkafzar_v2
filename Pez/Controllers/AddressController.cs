using DataLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pezeshkafzar_v2.Repositories;
using System.Data;
using System.Net;

namespace Pezeshkafzar_v2.Controllers
{
    [Authorize]
    public class AddressController : ControllerBase
    {
        private readonly IAddressRepository _addressRepository;
        private static Guid CurrentUserId;


        public AddressController(IHttpContextAccessor accessor, IAddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
            CurrentUserId = accessor.HttpContext.User.Identity.IsAuthenticated ? Guid.Parse(accessor.HttpContext.User.Claims.FirstOrDefault().Value) : Guid.Empty;
        }
        // GET: Address
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> AddressesList()
        {
            var addresses = await _addressRepository.GetUserAddressesAsync(CurrentUserId);
            if (addresses.Any() && !addresses.Any(a => a.IsDefault))
            {
                var firstAddress = addresses.FirstOrDefault();
                firstAddress.IsDefault = true;
                _addressRepository.Modify(firstAddress);
                await _addressRepository.SaveChangesAsync();
            }
            return PartialView(addresses.ToList());
        }

        // GET: Address/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return Redirect("/PageNotFound");
            }
            Addresses addresses = await _addressRepository.FindAsync(id.Value);

            if (addresses == null || addresses.UserId != CurrentUserId)
            {
                return Redirect("/PageNotFound");
            }
            return View(addresses);
        }

        // GET: Address/Create
        public IActionResult Create()
        {
            //ViewBag.UserID = new SelectList(db.Users, "UserID", "UserName");
            return View();
        }

        // POST: Address/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormCollection addressInput, string returnUrl)
        {
            var address = new Addresses
            {
                Address = addressInput["Address"],
                City = addressInput["City"],
                PostalCode = addressInput["PostalCode"],
                State = addressInput["State"]
            };
            if (!(string.IsNullOrWhiteSpace(address.Address) || string.IsNullOrWhiteSpace(address.City) || string.IsNullOrWhiteSpace(address.State) || string.IsNullOrWhiteSpace(address.PostalCode)))
            {
                var userAddresses = await _addressRepository.GetUserAddressesAsync(CurrentUserId);

                foreach (var item in userAddresses)
                {
                    item.IsDefault = false;
                    _addressRepository.Modify(item);
                }
                address.IsDefault = true;
                address.UserId = CurrentUserId;

                await _addressRepository.AddAsync(new Addresses
                {
                    Address=address.Address,
                    City = address.City,
                    State = address.State,
                    PostalCode = address.PostalCode,
                    IsDefault=address.IsDefault,
                    Lat = address.Lat,
                    Long = address.Long,
                    UserId = address.UserId
                });
                try
                {
                    await _addressRepository.SaveChangesAsync();
                }
                catch (Exception ex)
                {

                    throw;
                }

                if (returnUrl != null)
                {
                    return Redirect(returnUrl);
                }
                return RedirectToAction("Index");
            }

            TempData["AddressError"] = "یک یا چند فیلد نیاز به بازبینی مجدد دارند.";
            //ViewBag.UserID = new SelectList(db.Users, "UserID", "UserName", addresses.UserID);
            return View(address);
        }

        public async Task<List<Addresses>> SetDefaultAddress(Guid id)
        {
            var userAddresses = await _addressRepository.GetUserAddressesAsync(CurrentUserId);
            foreach (var item in userAddresses)
            {
                item.IsDefault = false;
                _addressRepository.Modify(item);
            }

            var address = userAddresses.First(x => x.Id == id);
            address.IsDefault = true;
            address.UserId = CurrentUserId;

            await _addressRepository.SaveChangesAsync();

            return userAddresses;
        }

        [HttpPost]
        public async Task<IActionResult> SetDefaultAddressWithReturn(Guid id)
        {
            return PartialView("AddressesList", await SetDefaultAddress(id));
        }

        // GET: Address/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return Redirect("/PageNotFound");
            }
            Addresses addresses = await _addressRepository.FindAsync(id.Value);
            if (addresses == null || addresses.UserId != CurrentUserId)
            {
                return Redirect("/PageNotFound");
            }
            //ViewBag.UserID = new SelectList(db.Users, "UserID", "UserName", addresses.UserID);
            return View(addresses);
        }

        // POST: Address/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AddressesDto addresses, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                _addressRepository.Modify(new Addresses
                {
                    Id = addresses.Id.Value,                
                    UserId=addresses.UserId,
                    Address =addresses.Address,
                    City = addresses.City,
                    IsDefault = addresses.IsDefault,
                    Lat = addresses.Lat,
                    Long = addresses.Long,
                    PostalCode = addresses.PostalCode,
                    State = addresses.State,
                    ModifiedAt = DateTime.Now
                });
                //if (db.Addresses.Find(addresses.AddressID).IsDefault && addresses.IsDefault == false)
                //{
                //    addresses.IsDefault = true;
                //}
                //if (addresses.IsDefault)
                //{
                //    SetDefaultAddress(addresses.AddressID);
                //}

                await _addressRepository.SaveChangesAsync();
                if (returnUrl != null)
                {
                    return Redirect(returnUrl);
                }
                return RedirectToAction("Index");
            }
            //ViewBag.UserID = new SelectList(db.Users, "UserID", "UserName", addresses.UserID);
            return View(addresses);
        }

        // GET: Address/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return Redirect("/PageNotFound");
            }
            Addresses addresses = await _addressRepository.FindAsync(id.Value);
            if (addresses == null || addresses.UserId != CurrentUserId)
            {
                return Redirect("/PageNotFound");
            }
            //ViewBag.UserID = new SelectList(db.Users, "UserID", "UserName", addresses.UserID);
            return View(addresses);
        }

        // POST: Address/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var userAddresses = await _addressRepository.GetUserAddressesAsync(CurrentUserId);
            Addresses address = userAddresses.First(x => x.Id == id);
            address.RemovedAt = DateTime.Now;
            _addressRepository.Modify(address);
            if (userAddresses.Where(a => a.Id != id).Any() && !userAddresses.Where(a => a.Id  != id).Any(a => a.IsDefault))
            {
                var firstAddress = userAddresses.First();
                firstAddress.IsDefault = true;
                _addressRepository.Modify(firstAddress);
            }
            await _addressRepository.SaveChangesAsync();
            return PartialView("AddressesList", userAddresses.Where(a => a.Id != id).ToList());
        }
    }
}
