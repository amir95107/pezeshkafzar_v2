using Microsoft.AspNetCore.Mvc;

namespace Pezeshkafzar_v2.Controllers
{
    public class ControllerBase : Controller
    {
        public string GetCookie(string key)
        {
            return Request.Cookies["Key"].ToString(); 
        }

        public void SetCookie(string key, string value, int? expireTime)
        {
            CookieOptions option = new CookieOptions();

            if (expireTime.HasValue)
                option.Expires = DateTime.Now.AddDays(expireTime.Value);
            else
                option.Expires = DateTime.Now.AddDays(1);

            Response.Cookies.Append(key, value, option);
        }

        public void RemoveCookie(string key)
        {
            Response.Cookies.Delete(key);
        }
    }
}
