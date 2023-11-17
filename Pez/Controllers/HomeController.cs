using DataLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Pezeshkafzar_v2.Repositories;
using System.Net;
using System.Threading;
using WebApplication100.Models;

namespace Pezeshkafzar_v2.Controllers
{

    public class HomeController : ControllerBase
    {
        private readonly IHomeRepository _homeRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUserRepository _userRepository;

        public HomeController(IHomeRepository homeRepository,
            IProductRepository productRepository,
            IUserRepository userRepository)
        {
            _homeRepository = homeRepository;
            _productRepository = productRepository;
            _userRepository = userRepository;
        }

        public async Task<IActionResult> ShowGroups(CancellationToken cancellationToken)
        {
            return PartialView(await _productRepository.GetProductGroupsAsync(false, cancellationToken));
        }

        // GET: Home
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var bList = await _productRepository.GetAllBrandsAsync(cancellationToken);
            ViewBag.Brands = bList;
            return View();
        }

        [Route("AboutUs")]
        public async Task<IActionResult> AboutUs(CancellationToken cancellationToken)
        {
            Page data = await _homeRepository.GetPageDetailAsync(2, cancellationToken);
            ViewBag.PageTitle = data.PageTitle;
            ViewBag.Title = data.HeadTitle;
            ViewBag.Description = data.MetaDescription;
            ViewBag.Content = data.PageContent;
            return View();
        }

        [Route("ContactUs")]
        public async Task<IActionResult> ContactUs(CancellationToken cancellationToken)
        {
            Page data = await _homeRepository.GetPageDetailAsync(1, cancellationToken);
            ViewBag.PageTitle = data.PageTitle;
            ViewBag.Title = data.HeadTitle;
            ViewBag.Description = data.MetaDescription;
            ViewBag.Content = data.PageContent;
            return View();
        }

        [HttpPost]
        [Route("SendMessage")]
        public ActionResult SendMessage(ContactForm contact, FormCollection form)
        {


            if (ModelState.IsValid)
            {
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
                    return View("ContactUs");
                }


                contact.Date = DateTime.Now;
                //if (User.Identity.IsAuthenticated)
                //{
                //    contact.UserID = GetUserIdWithUserName();
                //}
                //db.ContactForm.Add(contact);
                //db.SaveChanges();
                //TempData["Result"] = "ok";
                //TempData["Message"] = "پیام با موفقیت دریافت شد";
                //SendEmail.Send("a.janmohammadi@gmail.com", "", "پیام جدید با موضوع: " + contact.Subject, "نام: " + contact.Subject + "\n متن پیام: " + contact.Text);
                return RedirectToAction("ContactUs");
            }
            else
            {
                TempData["Result"] = "error";
                TempData["Message"] = "یک یا چند فیلد به اشتباه پر شده است";
                return View(contact);
            }
        }

        [Route("Faq")]
        public async Task<IActionResult> Faq(CancellationToken cancellationToken)
        {
            Page data = await _homeRepository.GetPageDetailAsync(3, cancellationToken);
            ViewBag.PageTitle = data.PageTitle;
            ViewBag.Title = data.HeadTitle;
            ViewBag.Description = data.MetaDescription;
            ViewBag.Content = data.PageContent;
            return View(await _homeRepository.GetFaqsAsync(cancellationToken));
        }

        [Route("terms-and-conditions")]
        public async Task<IActionResult> PrivacyAndPolicy(CancellationToken cancellationToken)
        {
            Page data = await _homeRepository.GetPageDetailAsync(4, cancellationToken);
            ViewBag.PageTitle = data.PageTitle;
            ViewBag.Title = data.HeadTitle;
            ViewBag.Description = data.MetaDescription;
            ViewBag.Content = data.PageContent;
            return View();
        }

        public async Task<IActionResult> Header(CancellationToken cancellationToken)
        {
            Guid roleId = Guid.Empty;
            bool isUserInformationComplete = false;
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userRepository.GetUserAsync("", cancellationToken);
                if (user == null)
                {
                    return Redirect("/ServerError");
                }
                roleId = user.Roles.Id;
                isUserInformationComplete = user.IsUserInfoCompleted;
            }
            ViewBag.IsUserInformationComplete = isUserInformationComplete;
            ViewBag.Role = roleId;
            return PartialView();
        }

        public IActionResult Categories()
        {
            return PartialView();
        }

        public async Task<IActionResult> Slider(CancellationToken cancellationToken)
            => PartialView(await _homeRepository.GetSliderListAsync(cancellationToken));

        public async Task<IActionResult> HumanBody()
        {
            return PartialView();
        }


        //public void Lead(string num)
        //{
        //    Lead_Clients lead = new Lead_Clients();
        //    lead.CreateDate = DateTime.Now;
        //    lead.Mobile = num;
        //    db.Lead_Clients.Add(lead);
        //    db.SaveChanges();
        //}

        public async Task<IActionResult> Footer()
        {
            return PartialView();
        }

        [Route("PageNotFound")]
        public async Task<IActionResult> PageNotFound()
        {
            return View();
        }

        [Route("ServerError")]
        public async Task<IActionResult> ServerError()
        {
            return View();
        }

        public async Task<IActionResult> SomeProductGroups(CancellationToken cancellationToken)
        {
            //return PartialView(db.Product_Groups.ToList());
            return PartialView(await _productRepository.GetProductGroupsAsync(false,cancellationToken));
        }

        public async Task<IActionResult> BottomMenu()
        {
            return PartialView();
        }
    }
}

