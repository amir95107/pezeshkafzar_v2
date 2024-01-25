using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Pezeshkafzar_v2.ViewModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Pezeshkafzar_v2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopController : ControllerBase
    {
        private readonly ISession Session;
        public ShopController(IHttpContextAccessor accessor) => Session = accessor.HttpContext.Session;

        // GET: api/<ShopController>
        [HttpGet]
        public int Get()
        {
            List<ShopCartItem> list = new List<ShopCartItem>();
            var shopCartSession = Session.GetString("ShopCart");

            if (shopCartSession != null)
            {
                list = JsonConvert.DeserializeObject<List<ShopCartItem>>(shopCartSession);
            }
            return list != null ? list.Sum(l => l.Count) : 0;
        }

        // GET api/<ShopController>/5
        [HttpGet("{id}")]
        public int Get(Guid id)
        {
            List<ShopCartItem> list = new List<ShopCartItem>();
            var shopCartSession = Session.GetString("ShopCart");

            if (shopCartSession != null)
            {
                list = JsonConvert.DeserializeObject<List<ShopCartItem>>(shopCartSession);
            }
            if (list.Any(p => p.ProductID == id))
            {
                int index = list.FindIndex(p => p.ProductID == id);
                list[index].Count++;
            }
            else
            {
                list.Add(new ShopCartItem()
                {
                    ProductID = id,
                    Count = 1
                });
            }

            Session.SetString("ShopCart", JsonConvert.SerializeObject(list));
            return Get();
        }

        // POST api/<ShopController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ShopController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ShopController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
