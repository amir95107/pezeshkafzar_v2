using DataLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Pezeshkafzar_v2.Repositories;
using Pezeshkafzar_v2.ViewModels;
using System.Data;
using ControllerBase = Pezeshkafzar_v2.Controllers.ControllerBase;

namespace Pezeshkafzar_v2.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly UserManager<Users> UserManager;
        private readonly ISession _session;

        public OrdersController(
            IOrderRepository orderRepository,
            IProductRepository productRepository,
            IUserRepository userRepository,
            UserManager<Users> userManager,
            IHttpContextAccessor accessor)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            UserManager = userManager;
            _session = accessor.HttpContext.Session;
        }

        // GET: Admin/Orders
        public async Task<IActionResult> Index()
        {
            return View();
        }

        // GET: Admin/Orders/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Orders orders = await _orderRepository.FindAsync(id.Value);
            if (orders == null)
            {
                return NotFound();
            }
            return View(orders);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.UserID = new SelectList(await UserManager.GetUsersInRoleAsync("Customer"), "UserId", "PhoneNumber");
            ViewBag.Products = await _productRepository.GetAllProductsAsync(2000,0);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Orders order)
        {
            List<ShowOrderViewModel> list = await getListOrder();

            Random rnd = new Random();
            var newRnd = rnd.Next(10000, 100000);
            var userId = order.UserId;
            var user = await UserManager.FindByIdAsync(userId.ToString());

            order.Date = DateTime.Now;
            order.Payable = list.Sum(l => l.PriceAfterDiscount * l.Count);
            order.IsFinaly = false;
            order.PaymentWay = 2;
            order.UseDiscountCode = false;
            order.IsFinaly = false;
            order.DeliveryID = Guid.NewGuid();
            order.TraceCode = (userId + user.UserName.Substring(9) + newRnd).ToString();

            foreach (var item in list)
            {
                order.OrderDetails.Add(new OrderDetails()
                {
                    OrderID = order.Id,
                    ProductID = item.ProductID,
                    Count = item.Count,
                    Price = item.PriceAfterDiscount,
                    Condition = 2,
                    TotalDiscount = item.Price - item.PriceAfterDiscount
                }
                );

                var product = await _productRepository.FindAsync(item.ProductID);
                product.Stock--;
                _productRepository.Modify(product);
            }

            await _orderRepository.AddAsync(order);
            await _orderRepository.SaveChangesAsync();
            _session.Remove("AdminShopCart");

            return RedirectToAction("Index");
        }


        // GET: Admin/Orders/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Orders orders = await _orderRepository.FindAsync(id.Value);
            if (orders == null)
            {
                return NotFound();
            }
            ViewBag.UserID = new SelectList(await UserManager.GetUsersInRoleAsync("Customer"), "UserID", "UserName", orders.UserId);
            return View(orders);
        }

        // POST: Admin/Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Orders orders)
        {
            if (ModelState.IsValid)
            {
                _orderRepository.Modify(orders);
                await _orderRepository.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.UserID = new SelectList(await UserManager.GetUsersInRoleAsync("Customer"), "UserID", "UserName", orders.UserId);
            return View(orders);
        }

        // GET: Admin/Orders/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Orders orders = await _orderRepository.FindAsync(id.Value);
            if (orders == null)
            {
                return NotFound();
            }
            return PartialView(orders);
        }

        // POST: Admin/Orders/Delete/5
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var od = await _orderRepository.GetOrderDetailsAsync(id);
            if (od.Count == 0)
                throw new Exception("DeleteConfirmed Error");

            foreach (var item in od)
            {
                od.Remove(item);
            }

            _orderRepository.Remove(od.First().Orders);
            await _orderRepository.SaveChangesAsync();
            
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ShowDetails(Guid id)
        {
            ViewBag.isFinally = await _orderRepository.IsOrderFinalAsync(id);
            var orderDetails = await _orderRepository.GetOrderDetailsAsync(id);
            return PartialView(orderDetails);
        }

        public async Task<IActionResult> ShowOredrDetails(Guid id)
        {
            ViewBag.isFinally = await _orderRepository.IsOrderFinalAsync(id);
            var orderDetails = await _orderRepository.GetOrderDetailsAsync(id);
            return PartialView(orderDetails);
        }

        public async Task<IActionResult> OrderTable()
        {
            var orders = await _orderRepository.GetOrdersAsync();
            return PartialView(orders.OrderByDescending(o=>o.Date));
        }

        public async Task<IActionResult> FinalizeOrder(Guid id)
        {
            Orders order = await _orderRepository.GetWithChildrenAsync(id);
            order.IsFinaly = true;
            
            foreach (var item in order.OrderDetails)
            {
                var product = await _productRepository.FindAsync(id);
                product.Stock--;
                _productRepository.Modify(product);
            }

            _orderRepository.Modify(order);
            await _orderRepository.SaveChangesAsync();
            return PartialView("OrderTable", await _orderRepository.GetOrdersAsync());
        }


        public async Task<IActionResult> SetOrderDetailsCondition(Guid id, int num, Guid orderId)
        {
            var orderDetails = await _orderRepository.GetWithChildrenAsync(orderId);
            orderDetails.OrderDetails.FirstOrDefault(x=>x.Id == id).Condition = num;
            _orderRepository.Modify(orderDetails);
            await _orderRepository.SaveChangesAsync();

            await SetOrderIsSent(orderId);

            return PartialView("OrderTable", await _orderRepository.GetOrdersAsync());
        }

        public async Task<IActionResult> SetOrderIsSent(Guid id)
        {
            var order = await _orderRepository.GetWithChildrenAsync(id);
            int sum = 0;
            foreach (var item in order.OrderDetails)
            {
                if (item.Condition == 1)
                {
                    sum++;
                }
            }
            if (sum == order.OrderDetails.Count())
            {
                order.IsSent = true;
                await _orderRepository.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return PartialView("OrderTable", await _orderRepository.GetOrdersAsync());
        }

        //public async Task<IActionResult> LoggedCarts(string range)
        //{
        //    var users = await UserManager.GetUsersInRoleAsync("Customer");
        //    ViewBag.Users = users;
        //    var logged = db.LoggedCart;
        //    var time = DateTime.Now;
        //    if (range == "day")
        //    {
        //        time = DateTime.Now.AddDays(-1);
        //    }
        //    else if (range == "week")
        //    {
        //        time = DateTime.Now.AddDays(-7);
        //    }
        //    else if (range == "month")
        //    {
        //        time = DateTime.Now.AddMonths(-1);
        //    }
        //    else
        //    {
        //        time = DateTime.Now.AddYears(-100);
        //    }
        //    if (range == null) range = "";
        //    ViewBag.Range = range;

        //    return View(logged.Where(l => l.Date > time && !l.Url.Contains("localhost") && l.UserName != "admin").ToList());
        //}

        //public void removeLoggs()
        //{
        //    var logs = db.LoggedCart.ToList();
        //    foreach (var log in logs)
        //    {
        //        db.LoggedCart.Remove(log);
        //    }
        //    db.SaveChanges();
        //}

        public async Task<List<ShowOrderViewModel>> getListOrder()
        {
            List<ShowOrderViewModel> list = new List<ShowOrderViewModel>();

            var cart = _session.GetString("AdminShopCart");
            if (cart != null)
            {
                List<ShopCartItem> listShop = JsonConvert.DeserializeObject<List<ShopCartItem>>(cart);

                if(listShop != null)
                {
                    var products = await _productRepository.GetAllAsync(listShop.Select(x => x.ProductID).ToList());
                    foreach (var item in listShop)
                    {
                        var product = products.Where(p => p.Id == item.ProductID).Select(p => new
                        {
                            p.ImageName,
                            p.Title,
                            p.Price,
                            p.PriceAfterDiscount,
                            p.SefUrl
                        }).Single();
                        list.Add(new ShowOrderViewModel()
                        {
                            Count = item.Count,
                            ProductID = item.ProductID,
                            Price = product.Price,
                            PriceAfterDiscount = product.PriceAfterDiscount,
                            Discount = item.Count * product.Price - item.Count * product.PriceAfterDiscount,
                            ImageName = product.ImageName,
                            Title = product.Title,
                            Sum = item.Count * product.Price,
                            SefUrl = product.SefUrl
                        });
                    }
                }
            }
            return list;
        }

        public async Task<IActionResult> AdminOrderList()
        {
            List<Products> products = await _productRepository.GetAllProductsAsync(2000,0);
            ViewBag.Products = products;
            List<ShopCartItem> listShop = JsonConvert.DeserializeObject<List<ShopCartItem>>(_session.GetString("AdminShopCart"));
            return PartialView(listShop);
        }

        public void CommandOrder(Guid id, int count)
        {
            List<ShopCartItem> listShop = JsonConvert.DeserializeObject<List<ShopCartItem>>(_session.GetString("AdminShopCart"));
            int index = listShop.FindIndex(p => p.ProductID == id);
            if (count == 0)
            {
                listShop.RemoveAt(index);
            }
            else
            {
                listShop[index].Count = count;
            }
            _session.SetString("AdminShopCart",JsonConvert.SerializeObject(listShop));
        }


        //[HttpPost]
        //public async bool UserInformation(Guid userId, string fullName, string telephone)
        //{
        //    var user = await 
        //    if (user != null)
        //    {
        //        UserInfo userInfo1 = new UserInfo()
        //        {
        //            UserId = userId,
        //            FullName = fullName,
        //            Telephone = telephone,
        //        };
        //        db.UserInfo.Add(userInfo1);
        //        user.IsUserInfoCompleted = true;
        //        db.SaveChanges();
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        //[HttpPost]
        //public void AddAddress(int userId,string address)
        //{
        //    db.Addresses.Add(new Addresses() { 
        //        UserID=userId,
        //        City="تهران",
        //        State="تهران",
        //        Address=address,
        //        IsDefault=true,
        //        PostalCode="1234"
        //    });
        //    db.SaveChanges();
        //}

        //public bool IsUserInfoComplete(int id) {
        //    var user = db.Users.Find(id);
        //    if (user.IsUserInfoCompleted)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        //public bool HasAddress(int id)
        //{
        //    var hasAddress = db.Addresses.Any(a=>a.UserID==id);
        //    return hasAddress;
        //}
    }
}
