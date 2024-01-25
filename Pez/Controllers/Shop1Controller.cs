using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Pezeshkafzar_v2.ViewModels;

namespace Pezeshkafzar_v2.Controllers
{
    [Area("shop")]
    public class Shop1Controller : ControllerBase
    {
        // GET: api/Shop
        private readonly ISession Session;
        public Shop1Controller(IHttpContextAccessor accessor) => Session = accessor.HttpContext.Session;
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

        // GET: api/Shop/5
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


    }
}
