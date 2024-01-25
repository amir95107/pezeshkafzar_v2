using Microsoft.AspNetCore.Mvc;

namespace Pezeshkafzar_v2.Controllers
{
    [Area("error")]
    public class ErrorController : Controller
    {
        [HttpGet("server-error")]
        public IActionResult Error500()
        {
            return View();
        }

        [HttpGet("page-not-found")]
        public IActionResult Error404()
        {
            return View();
        }
    }
}
