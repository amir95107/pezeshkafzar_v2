using Newtonsoft.Json;
using static ZarinPal.Class.Payment;

namespace Pezeshkafzar_v2.Utilities
{
    public class SendSMS
    {
        private static string PrivateKey = "e142a0200c3a40609f306a696ad8df5b";
        private static string SenderNumber = "50004001692968";
        public static string[] SendVerificationCode(string mobile)
        {
            Random rnd = new Random();
            int code = rnd.Next(1000, 10000);
            Uri apiBaseAddress = new Uri("https://console.melipayamak.com");
            string message = $"کد یک بار مصرف : {code}";
            using (HttpClient client = new HttpClient() { BaseAddress = apiBaseAddress })
            {
                // You may need to Install-Package Microsoft.AspNet.WebApi.Client
                var result = client.PostAsJsonAsync($"api/send/simple/{PrivateKey}",
                    new { from = SenderNumber, to = mobile, text = message }).Result;
                var response = result.Content.ReadAsStringAsync().Result;

                return [code.ToString(), response];
            }
        }
        public static string[] SendLoginVerificationCode(string mobile, string userName)
        {
            Random rnd = new Random();
            int code = rnd.Next(1000, 10000);
            Uri apiBaseAddress = new Uri("https://console.melipayamak.com");
            using (HttpClient client = new HttpClient() { BaseAddress = apiBaseAddress })
            {
                var result = client.PostAsJsonAsync($"api/send/shared/{PrivateKey}",
                    new { bodyId = 48025, to = mobile, args = new[] { userName, code.ToString() } }).Result;
                var response = result.Content.ReadAsStringAsync().Result;
                string[] arr = [code.ToString(), response];
                return arr;
            }
        }

        public static string SendSuccessOrderMessage(string mobile,string name,string traceCode)
        {
            Uri apiBaseAddress = new Uri("https://console.melipayamak.com");
            using (HttpClient client = new HttpClient() { BaseAddress = apiBaseAddress })
            {
                var result = client.PostAsJsonAsync("api/send/shared/e142a0200c3a40609f306a696ad8df5b",
                    new { bodyId = 48174, to = mobile, args = new[] { name, traceCode } }).Result;
                var response = result.Content.ReadAsStringAsync().Result;
                return response;
            }
        }

        public static string SendDiscountMessage(string mobile, string name, string discountCode,string maxValue,string date)
        {
            Uri apiBaseAddress = new Uri("https://console.melipayamak.com");
            using (HttpClient client = new HttpClient() { BaseAddress = apiBaseAddress })
            {
                var result = client.PostAsJsonAsync("api/send/shared/e142a0200c3a40609f306a696ad8df5b",
                    new { bodyId = 48170, to = mobile, args = new[] { name, discountCode,maxValue,date } }).Result;
                var response = result.Content.ReadAsStringAsync().Result;
                return response;
            }
        }

        public static string[] SendSellerVerificationCode(string mobile)
        {
            Random rnd = new Random();
            int code = rnd.Next(10000, 100000);
            Uri apiBaseAddress = new Uri("https://console.melipayamak.com");
            using (HttpClient client = new HttpClient() { BaseAddress = apiBaseAddress })
            {
                var result = client.PostAsJsonAsync("api/send/shared/e142a0200c3a40609f306a696ad8df5b",
                    new { bodyId = 49167, to = mobile, args = new[] { code.ToString() } }).Result;
                var response = result.Content.ReadAsStringAsync().Result;
                string[] arr = new string[] { code.ToString(), response };
                return arr;
            }
        }

        
        public static string[] SendOtp(string mobile)
        {
            Uri apiBaseAddress = new Uri("https://console.melipayamak.com");
            using (HttpClient client = new HttpClient() { BaseAddress = apiBaseAddress })
            {
                // You may need to Install-Package Microsoft.AspNet.WebApi.Client
                var result = client.PostAsJsonAsync("api/send/otp/e142a0200c3a40609f306a696ad8df5b",
                    new { to = mobile }).Result;
                var response = result.Content.ReadAsStringAsync().Result;
                var decodedResponse = JsonConvert.DeserializeObject<OtpResponse>(response);
                return [decodedResponse.Code, decodedResponse.Status];
            }
        }

        public static string[] ConfirmSeller(string mobile, string name)
        {
            Uri apiBaseAddress = new Uri("https://console.melipayamak.com");
            using (HttpClient client = new HttpClient() { BaseAddress = apiBaseAddress })
            {
                var result = client.PostAsJsonAsync("api/send/shared/e142a0200c3a40609f306a696ad8df5b",
                    new { bodyId = 49177, to = mobile, args = new[] { name } }).Result;
                var response = result.Content.ReadAsStringAsync().Result;
                string[] arr = new string[] { response };
                return arr;
            }
        }
    }

    
}

public class OtpResponse
{
    public string Code { get; set; }
    public string Status { get; set; }
}