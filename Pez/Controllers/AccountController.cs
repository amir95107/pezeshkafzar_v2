using DataLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using Pezeshkafzar_v2.Repositories;
using Pezeshkafzar_v2.Utilities;
using Pezeshkafzar_v2.ViewModels;
using System.Net;
using System.Text;
using WebApplication100.Models;

namespace Pezeshkafzar_v2.Controllers
{
    public class AccountController : ControllerBase
    {
        private Guid CurrentUserId;
        private readonly IUserRepository _userRepository; 
        private readonly IOrderRepository _orderRepository;
        private readonly ISession _session;
        private readonly UserManager<Users> _userManager;
        private readonly SignInManager<Users> _signInManager;

        public AccountController(IHttpContextAccessor accessor,
            IUserRepository userRepository,
            IOrderRepository orderRepository,
            UserManager<Users> userManager,
            SignInManager<Users> signInManager)
        {
            _userRepository = userRepository;
            _orderRepository = orderRepository;
            _session = accessor.HttpContext.Session;
            _userManager = userManager;
            _signInManager = signInManager;
            CurrentUserId = accessor.HttpContext.User.Identity.IsAuthenticated ? Guid.Parse(accessor.HttpContext.User.Claims.FirstOrDefault().Value) : Guid.Empty;
        }

        public IActionResult AccessDenied()
        {
            TempData["Title"] = "Access denied";
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            Users user = await _userRepository.FindAsync(CurrentUserId);
            return View(user);
        }

        public async Task<IActionResult> GetListOrder()
        {
            var orders = await _orderRepository.GetUserOrderAsync(CurrentUserId);
            if (orders.Any(o => o.IsFinaly && !o.IsSent))
            {
                List<string> traceode = orders.Where(o => o.IsFinaly && !o.IsSent).Select(o => o.TraceCode).ToList();
                //ViewBag.Order = traceode;
                return Json(traceode);
            }
            return null;
        }

        // GET: Account
        [Route("Register")]
        public ActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/");
            }
            return View();
        }



        [HttpPost]
        [Route("Register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(IFormCollection register, string returnUrl = "/")
        {
            if (ModelState.IsValid)
            {
                //#region googleRecaptcha
                ////google recaptcha
                //string urlToPost = "https://www.google.com/recaptcha/api/siteverify";
                //string secretKey = "6LfwqHQaAAAAAMM2R1tFwQdmak_N9stfEGYKofVu"; // change this
                //string gRecaptchaResponse = form["g-recaptcha-response"];

                //var postData = "secret=" + secretKey + "&response=" + gRecaptchaResponse;

                //// send post data
                //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlToPost);
                //request.Method = "POST";
                //request.ContentLength = postData.Length;
                //request.ContentType = "application/x-www-form-urlencoded";

                //using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                //{
                //    streamWriter.Write(postData);
                //}

                //// receive the response now
                //string reacpchatresult = string.Empty;
                //using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                //{
                //    using (var reader = new StreamReader(response.GetResponseStream()))
                //    {
                //        reacpchatresult = reader.ReadToEnd();
                //    }
                //}

                //// validate the response from Google reCaptcha
                //var captChaesponse = JsonConvert.DeserializeObject<reCaptchaResponse>(reacpchatresult);
                //if (!captChaesponse.Success)
                //{
                //    ViewBag.Message = "لطفا عبارت امنیتی را تایید کنید.";
                //    return View();
                //}
                ////google recaptcha
                //#endregion

                //if (!register.Mobile.IsValidMobile())
                //{
                //    TempData["RegisterError"] = "فرمت شماره موبایل اشتباه وارد شده است .";
                //    return View(register);
                //}


                if (await _userManager.FindByNameAsync(register["Mobile"]) == null)
                {
                    Random rnd = new Random();
                    int num = rnd.Next(100, 100000);
                    returnUrl ??= Url.Content("~/");

                    var user = new Users { UserName = register["Mobile"], PhoneNumber = register["Mobile"] };
                    var result = await _userManager.CreateAsync(user, register["Password"]);
                    if (result.Succeeded)
                    {
                        //_logger.LogInformation("User created a new account with password.");

                        //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        //code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                        //var callbackUrl = Url.Page(
                        //    "/Account/ConfirmEmail",
                        //    pageHandler: null,
                        //    values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        //    protocol: Request.Scheme);

                        //await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        //    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                        //send verivication code
                        var verifyCode = SendSMS.SendOtp(register["Mobile"]);
                        //string[] verifyCode = new string[] { "123456", "ارسال موفق بود" };
                        _session.SetString($"OTP_{register["Mobile"]}", verifyCode[0]);

                        if (verifyCode[1].Contains("ارسال موفق بود"))
                        {
                            //_session.SetString($"VerifyOtp_{user.Id}", verifyCode[0]);
                            return View("SuccessVerifyRegister", user);
                        }
                        TempData["RegisterError"] = "خطایی در ارسال پیامک رخ داده. لطفا بعدا تلاش کنید.";


                        if (_userManager.Options.SignIn.RequireConfirmedAccount)
                        {
                            return RedirectToPage("RegisterConfirmation", new { mobile = register["Mobile"], returnUrl = returnUrl });
                        }
                        else
                        {
                            await _signInManager.SignInAsync(user, isPersistent: false);
                            return LocalRedirect(returnUrl);
                        }
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }




                }
                else
                {
                    TempData["RegisterError"] = "شما قبلا ثبت نام کرده اید.";
                    return View(new RegisterViewModel
                    {
                        Mobile = register["Mobile"],
                    });
                }


            }
            TempData["RegisterError"] = "یکی از فیلدها به اشتباه پر شده است.";
            return View(new RegisterViewModel
            {
                Mobile = register["Mobile"],
            });
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel login, FormCollection form, string ReturnUrl = "/")
        {

            if (ModelState.IsValid)
            {
                //google recaptcha
                #region googleRecaptcha
                string urlToPost = "https://www.google.com/recaptcha/api/siteverify";
                string secretKey = "6LfwqHQaAAAAAMM2R1tFwQdmak_N9stfEGYKofVu"; // change this
                string gRecaptchaResponse = form["g-recaptcha-response"];

                var postData = "secret=" + secretKey + "&response=" + gRecaptchaResponse;

                // send post data
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlToPost);
                request.Method = "POST";
                request.ContentLength = postData.Length;
                request.ContentType = "application/x-www-form-urlencoded";

                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(postData);
                }

                // receive the response now
                string result = string.Empty;
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        result = reader.ReadToEnd();
                    }
                }

                // validate the response from Google reCaptcha
                var captChaesponse = JsonConvert.DeserializeObject<reCaptchaResponse>(result);
                if (!captChaesponse.Success)
                {
                    ViewBag.Message = "لطفا عبارت امنیتی را تایید کنید.";
                    return View();
                }
                #endregion
                //google recaptcha
                var Result = await _signInManager.PasswordSignInAsync(login.Mobile, login.Password, login.RememberMe, lockoutOnFailure: false);
                if (Result.Succeeded)
                {
                    //_logger.LogInformation("User logged in.");
                    return LocalRedirect(ReturnUrl);
                }
                if (Result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = ReturnUrl, RememberMe = login.RememberMe });
                }
                if (Result.IsLockedOut)
                {
                    //_logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                }
            }

            return View(login);
        }

        [Route("Login")]
        public ActionResult UserLogin()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Login")]
        public async Task<IActionResult> UserLogin(IFormCollection Input, string ReturnUrl = "/")
        {
            var userLogin = new UserLoginWithMobileViewModel
            {
                Mobile = Input["Mobile"],
                Password = Input["Password"]
            };
            if (ModelState.IsValid)
            {
                ReturnUrl ??= Url.Content("~/");

                var result = await _signInManager.PasswordSignInAsync(userLogin.Mobile, userLogin.Password, userLogin.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    CurrentUserId = Guid.Parse(HttpContext.User.Claims.FirstOrDefault().Value);
                    //await _userManager.AddLoginAsync(new Users { Id=CurrentUserId},new UserLoginInfo("","",""));
                    //_logger.LogInformation("User logged in.");
                    return LocalRedirect(ReturnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = ReturnUrl, RememberMe = userLogin.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    //_logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(userLogin);
                }
            }
            ModelState.AddModelError("Mobile", "موبایل وارد شده در فرمت درستی نیست.");
            return View(userLogin);
        }

        [Route("login/otp")]
        public async Task<IActionResult> UserLoginWithCode()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/");
            }
            return View();
        }


        [HttpPost]
        [Route("login/otp")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserLoginWithCode(IFormCollection Input, string ReturnUrl)
        {
            var userLogin = new UserLoginWithMobileViewModel
            {
                Mobile = Input["Mobile"]
            };
            if (!userLogin.Mobile.IsValidMobile())
            {
                TempData["LoginError"] = "فرمت شماره موبایل اشتباه وارد شده است .";
                return View();
            }
            Users user = await _userManager.FindByNameAsync(userLogin.Mobile);
            if (user == null)
            {
                TempData["LoginError"] = "شما هنوز ثبت نام نکرده اید.";
                return View(user);
            }
            else
            {
                string name = user.UserName;
                if (name == null)
                {
                    name = "کاربر";
                }
                string[] verify = SendSMS.SendOtp(userLogin.Mobile);
                //string[] verifyCode = new string[] { "123456", "ارسال موفق بود" };

                if (verify[1].Contains("ارسال موفق بود"))
                {
                    _session.SetString($"OTP_{user.Id}", verify[0]);
                    return View("SuccessVerifyCode", user);
                }
                else
                {
                    TempData["LoginError"] = "خطایی در ارسال پیامک رخ داده. لطفا از روشی دیگر برای ورود استفاده کنید و یا بعدا تلاش کنید..";
                    return View(user);
                }
            }
        }

        //public ActionResult ResendRegisterVerificationCode(string mobile)
        //{
        //    var user = db.Users.FirstOrDefault(u => u.Mobile == mobile);
        //    if (mobile.Length != 11 || mobile.Substring(0, 2) != "09")
        //    {
        //        TempData["LoginError"] = "فرمت شماره موبایل اشتباه وارد شده است .";
        //    }
        //    else
        //    {
        //        if (user != null && (bool)!user.IsMobileConfirmed)
        //        {
        //            var userName = user.UserName;
        //            if (userName == null)
        //            {
        //                userName = "کاربر";
        //            }
        //            var verify = SendSMS.SendLoginVerificationCode(mobile, userName);
        //            //string[] verify = new string[] { "123456", "ارسال موفق بود" };
        //            if (verify[1].Contains("ارسال موفق بود"))
        //            {
        //                Session["VerifyCode"] = verify[0];
        //                Session.Timeout = 2;
        //            }
        //        }
        //    }
        //    return View("SuccessVerifyRegister", user);
        //}

        //public ActionResult ResendLoginVerificationCode(string mobile)
        //{
        //    var user = db.Users.FirstOrDefault(u => u.Mobile == mobile);
        //    if (mobile.Length != 11 || mobile.Substring(0, 2) != "09")
        //    {
        //        TempData["LoginError"] = "فرمت شماره موبایل اشتباه وارد شده است .";
        //    }
        //    else
        //    {

        //        if (user != null && (bool)user.IsMobileConfirmed)
        //        {
        //            var userName = user.UserName;
        //            if (userName == null)
        //            {
        //                userName = "کاربر";
        //            }
        //            var verify = SendSMS.SendLoginVerificationCode(mobile, userName);
        //            //string[] verify = new string[] { "123456", "ارسال موفق بود" };
        //            if (verify[1].Contains("ارسال موفق بود"))
        //            {
        //                Session["VerifyCode"] = verify[0];
        //                Session.Timeout = 2;
        //            }
        //        }
        //    }

        //    return View("SuccessVerifyCode", user);
        //}

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyCode(string code, string PhoneNumber, string referPage)
        {
            var refer = referPage.ToLower().Contains("register");
            var goTo = refer ? "SuccessVerifyRegister" : "SuccessVerifyCode";
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(PhoneNumber);
                var theCode = _session.GetString($"OTP_{user.Id}");
                if (theCode == null)
                {
                    TempData["VerifyError"] = "کد تایید وارد شده صحیح نمیباشد.";
                    return View("ResendLoginVerificationCode", PhoneNumber);
                }
                if (theCode.ToString() == code)
                {
                    _session.Remove($"VerifyCode_{PhoneNumber}");

                    if (referPage.ToLower() == "register")
                    {
                        TempData["SuccessRegister"] = "ثبت نام شما با موفقیت انجام شد";
                        return Redirect("/Login?ref=register");
                    }
                    else
                    {
                        //FormsAuthentication.SetAuthCookie(Mobile, true);

                        await _signInManager.SignInAsync(user, true);
                        if (_session.GetString("ShopCart") != null)
                        {
                            return RedirectToAction("Index", "ShopCart");
                        }
                        else
                        {
                            return Redirect("/");
                        }
                    }
                }
                else
                {
                    TempData["VerifyError"] = "کد تایید وارد شده صحیح نمیباشد.";
                    return View(goTo, user);
                }
            }
            else
            {
                TempData["VerifyError"] = "خطایی رخ داده.لطفا بعدا تلاش کنید.";
                return RedirectToAction("Register");
            }
        }

        //public ActionResult ActiveUser(string id)
        //{
        //    var user = db.UserInfo.SingleOrDefault(u => u.ActiveCode == id);
        //    if (user == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    if (user.IsEmailConfirmed)
        //    {
        //        View(user);
        //    }

        //    user.IsEmailConfirmed = true;
        //    user.ActiveCode = Guid.NewGuid().ToString();
        //    db.SaveChanges();
        //    ViewBag.UserName = user.FullName.Split(' ')[0];
        //    return RedirectToAction("Index", user.Users);
        //}

        //public ActionResult ActiveSeller(string id)
        //{
        //    var user = db.Users.SingleOrDefault(u => u.ActiveCode == id);
        //    if (user == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    user.IsActive = true;
        //    user.ActiveCode = Guid.NewGuid().ToString();
        //    db.SaveChanges();
        //    ViewBag.Name = user.UserName;
        //    return View();
        //}

        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            return Redirect("/");
        }

        //[Route("ForgotPassword")]
        //public ActionResult ForgotPassword()
        //{
        //    return View();
        //}


        //[Route("ForgotPassword")]
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult ForgotPassword(ForgotPasswordViewModel forgot)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = db.Users.SingleOrDefault(u => u.Email == forgot.Email);
        //        if (user != null)
        //        {
        //            if (user.IsActive)
        //            {
        //                string bodyEmail =
        //                    PartialToStringClass.RenderPartialView("ManageEmails", "RecoveryPassword", user);
        //                SendEmail.Send(user.Email, "", "بازیابی کلمه عبور", bodyEmail);
        //                return View("SuccesForgotPassword", user);
        //            }
        //            else
        //            {
        //                ModelState.AddModelError("Email", "حساب کاربری شما فعال نیست");
        //            }
        //        }
        //        else
        //        {
        //            ModelState.AddModelError("Email", "کاربری یافت نشد");
        //        }
        //    }
        //    return View();
        //}

        //public ActionResult RecoveryPassword(string id)
        //{
        //    return View();
        //}

        //[HttpPost]
        //public ActionResult RecoveryPassword(string id, RecoveryPasswordViewModel recovery)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = db.Users.SingleOrDefault(u => u.ActiveCode == id);
        //        if (user == null)
        //        {
        //            return HttpNotFound();
        //        }

        //        user.Password = FormsAuthentication.HashPasswordForStoringInConfigFile(recovery.Password, "MD5");
        //        user.ActiveCode = Guid.NewGuid().ToString();
        //        db.SaveChanges();
        //        return Redirect("/Login?recovery=true");
        //    }
        //    return View();
        //}

        [Authorize]
        public async Task<IActionResult> UserInformation()
        {
            var user = await _userRepository.FindAsync(CurrentUserId);
            if (user == null)
                throw new Exception();

            if (user.IsUserInfoCompleted)
            {
                return RedirectToAction("EditUserInfo");
            }
            else
            {
                var rd = "/account/userinformation?previousUrl=shopcart";
                ViewBag.Redirect = rd;
            }
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserInformation(UserInfoViewModel userInfo, string ReturnUrl)
        {
            var user = await _userRepository.FindAsync(CurrentUserId);
            UserInfo userInfo1 = new UserInfo()
            {
                UserId = CurrentUserId,
                FirstName = userInfo.FirstName,
                LastName = userInfo.LastName,
                Email = userInfo.Email,
                Telephone = userInfo.Telephone
            };
            if (!user.UserInfo.Any())
            {
                user.IsUserInfoCompleted = true;
            }
            user.UserInfo.Add(userInfo1);
            _userRepository.Modify(user);
            await _userRepository.SaveChangesAsync();

            var rq = HttpContext.Request.Query["ReturnUrl"];
            if (ReturnUrl != null)
            {
                return Redirect(ReturnUrl);
            }
            return Redirect("/");

        }

        //[Authorize]
        //public ActionResult EditUserInfo()
        //{
        //    string userName = User.Identity.Name;
        //    var isComplete = db.Users.Single(u => u.Mobile == userName).IsUserInfoCompleted;
        //    if (!isComplete)
        //    {
        //        return RedirectToAction("UserInformation");
        //    }
        //    UserInfo ui = db.UserInfo.FirstOrDefault(u => u.Users.Mobile == userName);
        //    return View(ui);
        //}

        //[Authorize]
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult EditUserInfo(UserInfo ui, string returnUrl)
        //{
        //    string userName = User.Identity.Name;
        //    var userInfo = db.UserInfo.Find(ui.UIID);
        //    userInfo.Email = ui.Email;
        //    userInfo.IsEmailConfirmed = ui.IsEmailConfirmed;
        //    userInfo.Telephone = ui.Telephone;
        //    userInfo.FullName = ui.FullName;
        //    userInfo.UserID = ui.UserID;
        //    if (userInfo.Email != null && userInfo.Email.ToLower().Trim() != ui.Email.ToLower().Trim())
        //    {
        //        userInfo.IsEmailConfirmed = false;
        //    }
        //    userInfo.ActiveCode = Guid.NewGuid().ToString();
        //    db.Entry(userInfo).State = EntityState.Modified;
        //    db.SaveChanges();
        //    TempData["SuccessEditUSerInfo"] = "اطلاعات با موفقیت بروزرسانی شد.";
        //    if (returnUrl != null)
        //    {
        //        return Redirect(returnUrl);
        //    }
        //    return View(db.UserInfo.FirstOrDefault(u => u.Users.Mobile == userName));
        //}

        [Authorize]
        public async Task<IActionResult> MyOrders()
            => View(await _orderRepository.GetUserOrderAsync(CurrentUserId));

        //[Authorize]
        //public ActionResult ShowOrderDetails(int id)
        //{
        //    ViewBag.isFinally = db.Orders.Find(id).IsFinaly;
        //    var orderDetails = db.OrderDetails.Where(o => o.OrderID == id).ToList();
        //    return PartialView(orderDetails);
        //}

        //[Authorize]
        //public ActionResult ChangePassword()
        //{
        //    return View();
        //}

        //[Authorize]
        //[HttpPost]
        //public ActionResult ChangePassword(ChangePasswordViewModel change)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = db.Users.Single(u => u.Mobile == User.Identity.Name);
        //        string oldPasswordHash =
        //            FormsAuthentication.HashPasswordForStoringInConfigFile(change.OldPassword, "MD5");
        //        if (user.Password == oldPasswordHash)
        //        {
        //            string hashNewPasword =
        //                FormsAuthentication.HashPasswordForStoringInConfigFile(change.Password, "MD5");
        //            user.Password = hashNewPasword;
        //            db.SaveChanges();
        //            ViewBag.Success = true;
        //        }
        //        else
        //        {
        //            ModelState.AddModelError("OldPassword", "کلمه عبور فعلی درست نمی باشد");
        //        }
        //    }
        //    return View();
        //}


    }
}