using DataLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Pezeshkafzar_v2.Repositories;
using Pezeshkafzar_v2.Utilities;
using Pezeshkafzar_v2.ViewModels;

namespace Pezeshkafzar_v2.Controllers
{
    public class ShopCartController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IDeliverWaysRepository _deliverWaysRepository;
        private readonly IAddressRepository _addressRepository;
        private readonly UserManager<Users> UserManager;
        private Guid CurrentUserId;
        private readonly ISession Session;

        public ShopCartController(IOrderRepository orderRepository,
            IHttpContextAccessor accessor,
            IProductRepository productRepository,
            IAddressRepository addressRepository,
            UserManager<Users> userManager,
            IDeliverWaysRepository deliverWaysRepository)
        {
            _orderRepository = orderRepository;
            Session = accessor.HttpContext.Session;
            CurrentUserId = accessor.HttpContext.User.Identity.IsAuthenticated ? Guid.Parse(accessor.HttpContext.User.Claims.FirstOrDefault().Value) : Guid.Empty;
            _productRepository = productRepository;
            UserManager = userManager;
            _deliverWaysRepository = deliverWaysRepository;
            _addressRepository = addressRepository;
        }

        // GET: ShopCart

        public async Task<IActionResult> ShowCart()
        {
            return PartialView(await getListOrderAsync());
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<List<ShowOrderViewModel>> getListOrderAsync()
        {
            List<ShowOrderViewModel> list = new List<ShowOrderViewModel>();
            var shopCartSession = Session.GetString("ShopCart");


            if (shopCartSession != null)
            {
                List<ShopCartItem> listShop = JsonConvert.DeserializeObject<List<ShopCartItem>>(shopCartSession);

                if (listShop.Count > 0)
                {
                    var products = await _productRepository.GetAllAsync(listShop.Select(x => x.ProductID));
                    foreach (var item in listShop)
                    {
                        var product = products.Where(p => p.Id == item.ProductID).Select(p => new
                        {
                            p.ImageName,
                            p.PriceAfterDiscount,
                            p.Price,
                            p.Title,
                            p.SefUrl
                        }).First();
                        list.Add(new ShowOrderViewModel()
                        {
                            Count = item.Count,
                            ProductID = item.ProductID,
                            Title = product.Title,
                            ImageName = product.ImageName,
                            PriceAfterDiscount = product.PriceAfterDiscount,
                            Price=product.Price,
                            SefUrl = product.SefUrl,
                            Sum = product.Price*item.Count,
                            Discount = (product.Price - product.PriceAfterDiscount)*item.Count
                        });
                    }
                }
            }

            return list;
        }

        public async Task<IActionResult> OrderingLevel()
        {
            bool shopCart = false, shipping = false, userInfo = false, factor = false, payment = false;
            var shopCartSession = Session.GetString("ShopCart");

            if (shopCartSession != null)
            {
                shopCart = true;
                if (User.Identity.IsAuthenticated)
                {
                    var user = await UserManager.FindByIdAsync(CurrentUserId.ToString());
                    if (user.IsUserInfoCompleted)
                    {
                        userInfo = true;
                        var shippingSession = Session.GetString("Shipping");
                        if (shippingSession != null)
                        {
                            shipping = true;
                            if (Request.Path == "/ShopCart/Factor")
                            {
                                factor = true;
                            }
                            if (Request.Path.ToString().Contains("Pay"))
                            {
                                factor = true;
                                payment = true;
                            }
                        }
                    }
                }

            }
            bool[] list = [shopCart, userInfo, shipping, factor, payment];
            string[] _list = ["سبد خرید", "تکمیل اطلاعات", "شیوه ارسال", "فاکتور", "پرداخت"];
            string[] url = ["/ShopCart", "/Account/UserInformation", "/ShopCart/Delivery", "/ShopCart/Factor", "#"];
            ViewBag.Level = list;
            ViewBag.ListName = _list;
            ViewBag.Url = url;
            return PartialView();
        }

        public async Task<IActionResult> Order()
        {
            if (string.IsNullOrWhiteSpace(Request.Query["noShippingWaySelected"]))
            {
                ViewBag.NoShippingWaySelected = "لطفا یکی از روشهای ارسال را انتخاب کنید";
            }
            ViewBag.shipping = Session.GetInt32("Shipping");
            ViewBag.DiscountPercent = Session.GetInt32("Discount");
            List<DeliveryWays> dList = await _deliverWaysRepository.GetAllWays();
            ViewBag.DList = dList;

            #region OrderingLevels
            bool shopCart = false, shipping = false, userInfo = false, factor = false, payment = false;
            var shopCartSession = Session.GetString("ShopCart");

            if (shopCartSession != null)
            {
                shopCart = true;
                if (User.Identity.IsAuthenticated)
                {
                    var user = await UserManager.FindByIdAsync(CurrentUserId.ToString());
                    if (user.IsUserInfoCompleted)
                    {
                        userInfo = true;
                        var shippingSession = Session.GetString("Shipping");
                        if (shippingSession != null)
                        {
                            shipping = true;
                            if (Request.Path == "/ShopCart/Factor")
                            {
                                factor = true;
                            }
                            if (Request.Path.ToString().Contains("Pay"))
                            {
                                factor = true;
                                payment = true;
                            }
                        }
                    }
                }

            }
            bool[] list = [shopCart, userInfo, shipping, factor, payment];
            string[] _list = ["سبد خرید", "تکمیل اطلاعات", "شیوه ارسال", "فاکتور", "پرداخت"];
            string[] url = ["/ShopCart", "/Account/UserInformation", "/ShopCart/Delivery", "/ShopCart/Factor", "#"];
            ViewBag.Level = list;
            ViewBag.ListName = _list;
            ViewBag.Url = url;
            #endregion

            return PartialView(await getListOrderAsync());
        }

        public async Task<ActionResult> CommandOrder(Guid id, int count)
        {
            var shopCartSession = Session.GetString("ShopCart");
            List<ShopCartItem> listShop = shopCartSession?.Deserialize<List<ShopCartItem>>();
            int index = listShop.FindIndex(p => p.ProductID == id);
            if (count == 0)
            {
                listShop.RemoveAt(index);
            }
            else
            {
                listShop[index].Count = count;
            }
            Session.SetString("ShopCart", JsonConvert.SerializeObject(listShop));

            #region OrderingLevels
            bool shopCart = false, shipping = false, userInfo = false, factor = false, payment = false;

            if (shopCartSession != null)
            {
                shopCart = true;
                if (User.Identity.IsAuthenticated)
                {
                    var user = await UserManager.FindByIdAsync(CurrentUserId.ToString());
                    if (user.IsUserInfoCompleted)
                    {
                        userInfo = true;
                        var shippingSession = Session.GetString("Shipping");
                        if (shippingSession != null)
                        {
                            shipping = true;
                            if (Request.Path == "/ShopCart/Factor")
                            {
                                factor = true;
                            }
                            if (Request.Path.ToString().Contains("Pay"))
                            {
                                factor = true;
                                payment = true;
                            }
                        }
                    }
                }

            }
            bool[] list = [shopCart, userInfo, shipping, factor, payment];
            string[] _list = ["سبد خرید", "تکمیل اطلاعات", "شیوه ارسال", "فاکتور", "پرداخت"];
            string[] url = ["/ShopCart", "/Account/UserInformation", "/ShopCart/Delivery", "/ShopCart/Factor", "#"];
            ViewBag.Level = list;
            ViewBag.ListName = _list;
            ViewBag.Url = url;
            #endregion

            return PartialView("Order", await getListOrderAsync());
        }

        [Authorize]
        public async Task<ActionResult> Factor()
        {
            var shopCartSession = Session.GetString("ShopCart");
            List<ShopCartItem> listShop = JsonConvert.DeserializeObject<List<ShopCartItem>>(shopCartSession);
            if (shopCartSession == null)
            {
                return RedirectToAction("Index");
            }

            var shippingSession = Session.GetString("Shipping");
            DeliveryWays Shipping = !string.IsNullOrEmpty(shippingSession) ? JsonConvert.DeserializeObject<DeliveryWays>(shippingSession) : null;

            //if (discount == null) discount = 0;
            if (Shipping != null)
            {
                var discount = Session.GetString("Discount")?.Deserialize<Discounts>();
                int shipping = int.Parse(Shipping.Price.ToString());
                var user = await UserManager.FindByIdAsync(CurrentUserId.ToString());
                var isComplete = user.IsUserInfoCompleted;
                ViewBag.shipping = Shipping;
                ViewBag.DiscountPercent = (discount == null) ? 0 : discount.DiscountPercent;
                if (isComplete)
                {
                    var address = await _addressRepository.GetUserAddressesAsync(CurrentUserId);
                    if (address.Any(a => a.IsDefault))
                    {
                        var listDetails = await getListOrderAsync();
                        var payable = listDetails.Sum(l => l.Sum) - listDetails.Sum(l => l.Discount);
                        var decreaseFromPayable = (discount == null) ? 0 : Math.Min(discount.DiscountPercent * payable / 100, discount.MaxDiscountValue);
                        var payableAfterDiscountCode = payable - decreaseFromPayable;
                        bool isUsed = false;
                        var useDiscount = Session.GetString("UseDiscount")?.Deserialize<bool>();
                        if (useDiscount ?? false)
                        {
                            isUsed = true;
                        }

                        Orders order = new Orders()
                        {
                            Date = DateTime.Now,
                            Payable = payableAfterDiscountCode + shipping,
                            IsFinaly = false,
                            PaymentWay = 2,
                            UseDiscountCode = isUsed,
                            TraceCode = DateTime.Now.ToString("hhMMss") + User.Identity.Name.Substring(9) + CurrentUserId.ToString(),
                            IsSent = false,
                            DeliveryID = Shipping.Id,
                            DeliveryPrice = shipping,
                            UserId=CurrentUserId
                        };
                        await _orderRepository.AddAsync(order);
                        await _orderRepository.SaveChangesAsync();

                        foreach (var item in listDetails)
                        {
                            order.OrderDetails.Add(new OrderDetails()
                            {
                                Count = item.Count,
                                ProductID = item.ProductID,
                                Price = item.PriceAfterDiscount,
                                OrderID = order.Id,
                                TotalDiscount = (item.Price - item.PriceAfterDiscount) * item.Count,
                                Condition = 2
                            });
                        }
                        
                        try
                        {
                            await _orderRepository.SaveChangesAsync();
                        }
                        catch (Exception ex)
                        {

                            throw;
                        }
                        ViewBag.OrderId = order.Id;
                        ViewBag.DefaultAddress = address.FirstOrDefault(a => a.IsDefault).Address.ToString();

                        string body = $"شخصی با شماره {order.Users.PhoneNumber} یک سفارش به مبلغ  {order.Payable} تومن، ثبت کرده.";
                        try
                        {
                            if (order.Users.PhoneNumber != "09120624426")
                            {
                                SendEmail.Send("a.janmohammadi@gmail.com", "", "سفارش جدید", body);
                            }
                        }
                        catch { }

                        return View(getListOrderAsync());
                    }
                    else
                    {
                        return Redirect("/ShopCart/Delivery");
                    }
                }
                else
                {
                    return RedirectToAction("UserInformation", "Account", new { area = "" });
                }
            }
            else
            {

                return RedirectToAction("Delivery", await _deliverWaysRepository.GetAllWays());
            }
        }

        [Authorize]
        public async Task<IActionResult> Delivery()
        {
            if (string.IsNullOrWhiteSpace(Session.GetString("ShopCart")))
            {
                return RedirectToAction("Index");
            }
            else
            {
                var user = await UserManager.FindByIdAsync(CurrentUserId.ToString());
                if (user.IsUserInfoCompleted)
                {
                    var addresses = await _addressRepository.GetUserAddressesAsync(CurrentUserId);

                    if (string.IsNullOrEmpty(Session.GetString("Shipping")) || !addresses.Any(a => a.IsDefault))
                    {
                        ViewBag.EmptyShipping = "لطفا یکی از شیوه های ارسال را انتخاب کنید";
                    }

                    var deliveryWays = await _deliverWaysRepository.GetAllWays();
                    var pay = await Payable();
                    int payable = pay[0];
                    bool moreThanMil = false;
                    if (payable > 1000000)
                    {
                        moreThanMil = true;
                    }
                    ViewBag.MoreThanMillion = moreThanMil;
                    ViewBag.EmptyShipping = "لطفا یکی از شیوه های ارسال را انتخاب کنید";
                    ViewBag.DeliveryWays = deliveryWays;
                    return View(addresses);
                    //else
                    //{
                    //    return RedirectToAction("Factor");
                    //}
                }
                else
                {
                    return Redirect("/account/userinformation?ReturnUrl=/ShopCart/Delivery");
                }
            }
        }

        public async Task<string> ShowDeliveryDescription(Guid id)
        {
            var delivery = await _deliverWaysRepository.FindAsync(id);
            return delivery?.Description ?? "";
        }

        public async Task<int[]> Payable()
        {
            List<ShowOrderViewModel> list = await getListOrderAsync();
            int discount = 0;
            if (!string.IsNullOrEmpty(Session.GetString("Discount")))
            {
                discount = (int)(Session.GetString("Discount")?.Deserialize<int>());
            }
            DeliveryWays ship = Session.GetString("Shipping")?.Deserialize<DeliveryWays>();
            var totalDiscount = list.Sum(l => l.Price - l.PriceAfterDiscount) + discount;

            var sum = (int)(((100 - discount) / 100) * list.Sum(l => l.PriceAfterDiscount) + (ship != null ? ship.Price : 0));
            int[] arr = new int[] { sum, discount };
            return arr;
        }


        public async Task<IActionResult> PayWithCardNo(Guid id)
        {
            Orders order = await _orderRepository.FindAsync(id);
            int[] arr = await Payable();
            if (!order.IsFinaly)
            {

                order.PaymentWay = 2;
                _orderRepository.Modify(order);
                await _orderRepository.SaveChangesAsync();

                ViewBag.IsFinally = false;
                string body = $"شخصی با شماره {order.Users.PhoneNumber} یک سفارش به مبلغ  {order.Payable} تومن، به روش کارت به کارت، ثبت کرده.";
                SendEmail.Send("a.janmohammadi@gmail.com", "", "سفارش جدید", body);
            }
            else
            {
                ViewBag.IsFinally = true;
            }
            ViewBag.Data = arr;
            return View();
        }

        //[Authorize]
        //[HttpPost]
        //public async dynamic DiscountCodeChecker(string dCode)
        //{
        //    var user = await UserManager.FindByIdAsync(CurrentUserId.ToString());
        //    bool isUsed = false;

        //    if (db.Discounts.Any(d => d.UserID == user.UserID && d.DiscountCode == dCode && d.IsUsed == false && d.ExpireDate > DateTime.Now))
        //    {
        //        Discounts discounts = db.Discounts.FirstOrDefault(d => d.UserID == user.UserID && d.DiscountCode == dCode && d.IsUsed == false && d.ExpireDate > DateTime.Now);
        //        isUsed = true;
        //        Session["UseDiscount"] = isUsed;
        //        Session["Discount"] = discounts;
        //        ViewBag.DiscountPercent = Session["Discount"];

        //        return PartialView("Order", getListOrderAsync());
        //    }
        //    else
        //    {
        //        ViewBag.DiscountIsNotCorrect = "وجود ندارد!";
        //    }
        //    return "";

        //}

        public async Task<IActionResult> Shipping(Guid id)
        {
            var dw = await _deliverWaysRepository.FindAsync(id);
            var shCost = dw.Price;


            Session.SetString("Shipping", dw.Serilize());
            ViewBag.Shipping = dw;
            List<DeliveryWays> dList = await _deliverWaysRepository.GetAllWays();
            ViewBag.DList = dList;
            return PartialView("Order",await getListOrderAsync());
        }

        [Authorize]
        public async void DeleteOrders(Guid id)
        {
            var order = await _orderRepository.GetWithChildrenAsync(id);
            if (order != null && order.UserId == CurrentUserId)
            {
                var od = order.OrderDetails.Where(o => o.OrderID == id);
                
                order.RemovedAt = DateTime.Now;
                _orderRepository.Modify(order);
                await _orderRepository.SaveChangesAsync();
            }
        }

        //[HttpPost]
        //[Authorize]
        //public async Task<ActionResult> DeleteAccountOrders(int id)
        //{
        //    DeleteOrders(id);
        //    int userId = GetUserIdWithUserName();
        //    return PartialView("AccountOrders", db.Orders.Where(o => o.UserID == userId).ToList());
        //}

        [Authorize]
        public async Task<ActionResult> AccountOrders()
        {
            return PartialView(await _orderRepository.GetUserOrderAsync(CurrentUserId));
        }

        //[HttpPost]
        //public void LogAddtoCart(int productId, string url)
        //{
        //    LoggedCart shopCart = new LoggedCart()
        //    {
        //        ProductID = productId,
        //        UserName = User.Identity.Name,
        //        Date = DateTime.Now,
        //        Url = url
        //    };

        //    try
        //    {
        //        if (shopCart.UserName != null && shopCart.UserName != "admin")
        //        {
        //            db.LoggedCart.Add(shopCart);
        //            db.SaveChanges();
        //        }
        //    }
        //    catch { }

        //}
    }
}