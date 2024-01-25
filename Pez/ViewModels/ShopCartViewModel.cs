namespace Pezeshkafzar_v2.ViewModels
{
    public class ShopCartItem
    {
        public Guid ProductID { get; set; }
        public int Count { get; set; }
    }

    public class ShopCartItemViewModel
    {
        public Guid ProductID { get; set; }
        public string Title { get; set; }
        public string ImageName { get; set; }
        public decimal Count { get; set; }
        public decimal PriceAfterDiscount { get; set; }
        public string SefUrl { get; set; }

        public int Sum { get; set; }
    }

    public class ShowOrderViewModel
    {
        public Guid ProductID { get; set; }
        public string Title { get; set; }
        public string ImageName { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }
        public decimal PriceAfterDiscount { get; set; }
        public decimal Discount { get; set; }
        public decimal Sum { get; set; }
        public string SefUrl { get; set; }

    }

    public class DiscountViewModel
    {
        public int Percent { get; set; }
        public string code { get; set; }
    }

    public class PaymentViewModel
    {
        public string merchant_id { get; set; }
        public int amount { get; set; }
        public string description { get; set; }
        public string callback_url { get; set; }
    }

    public class SendingWaysViewModel
    {
        public int Title { get; set; }
        public int Price { get; set; }
    }
}
