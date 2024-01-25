using Newtonsoft.Json;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Pezeshkafzar_v2.Utilities
{
    public static class StringExtensions
    {
        public static DateTime ToDateTime(this string persianDate)
        {
            if (string.IsNullOrEmpty(persianDate))
            {
                return DateTime.MinValue;
            }

            string[] array = persianDate.Split('T');
            if (array.Length == 0)
            {
                return DateTime.MinValue;
            }

            string[] array2 = array[0].Split('/');
            if (array2.Length != 3)
            {
                return DateTime.MinValue;
            }

            string[] array3 = ((array.Length > 1) ? array[1].Split('/') : new string[3] { "0", "0", "0" });
            if (array3.Length != 3)
            {
                return DateTime.MinValue;
            }

            return DateTime.Parse(new DateTime(int.Parse(array2[0]), int.Parse(array2[1]), int.Parse(array2[2]), int.Parse(array3[0]), int.Parse(array3[1]), int.Parse(array3[2]), new PersianCalendar()).ToString(CultureInfo.CreateSpecificCulture("en-US")));
        }

        public static string ToFaNumeric(this string str)
        {
            return str.Replace("0", "۰").Replace("1", "۱").Replace("2", "۲")
                .Replace("3", "۳")
                .Replace("4", "۴")
                .Replace("5", "۵")
                .Replace("6", "۶")
                .Replace("7", "۷")
                .Replace("8", "۸")
                .Replace("9", "۹");
        }

        public static string ToEnNumeric(this string str)
        {
            return str.Replace("۰", "0").Replace("۱", "1").Replace("۲", "2")
                .Replace("۳", "3")
                .Replace("۴", "4")
                .Replace("۵", "5")
                .Replace("۶", "6")
                .Replace("۷", "7")
                .Replace("۸", "8")
                .Replace("۹", "9")
                .Replace("٠", "0")
                .Replace("١", "1")
                .Replace("٢", "2")
                .Replace("٣", "3")
                .Replace("٤", "4")
                .Replace("٥", "5")
                .Replace("٦", "6")
                .Replace("٧", "7")
                .Replace("٨", "8")
                .Replace("٩", "9");
        }

        public static string FixPersianChars(this string str)
        {
            return str.Replace("ﮎ", "ک").Replace("ﮏ", "ک").Replace("ﮐ", "ک")
                .Replace("ﮑ", "ک")
                .Replace("ك", "ک")
                .Replace("ي", "ی")
                .Replace(" ", " ")
                .Replace("\u200c", " ")
                .Replace("ھ", "ه");
        }

        public static string? NullIfEmpty(this string? str)
        {
            return (str != null && str.Length == 0) ? null : str;
        }

        public static T ToEnum<T>(this string value)
        {
            Enum.TryParse(typeof(T), value, ignoreCase: true, out object result);
            return (T)result;
        }

        public static bool HasValue(this string value, bool ignoreWhiteSpace = true)
        {
            return ignoreWhiteSpace ? (!string.IsNullOrWhiteSpace(value)) : (!string.IsNullOrEmpty(value));
        }

        public static T? Deserialize<T>(this string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }

        public static string Serilize<T>(this T value)
        {
            return JsonConvert.SerializeObject(value);
        }

        public static bool IsValidEmail(this string strIn)
        {
            if (string.IsNullOrWhiteSpace(strIn))
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(strIn, "^([\\w\\.\\-]+)@([\\w\\-]+)((\\.(\\w){2,3})+)$", RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250.0));
            }
            catch
            {
                return false;
            }
        }

        public static bool IsValidMobile(this string input)
        {
            return !string.IsNullOrWhiteSpace(input) && Regex.IsMatch(input, "(^09\\d{9}$)|(^\\+989\\d{9}$)|(^9\\d{9}$)|(^989\\d{9}$)");
        }

        public static string UrlCase(this string input)
        {
            return Regex.Replace(input, "([a-z])([A-Z])", "$1-$2").ToLower();
        }

        public static bool IsValidNationalCode(this string input)
        {
            return !string.IsNullOrWhiteSpace(input) && Regex.IsMatch(input, "^(\\d{10})$");
        }

        public static string RandomString(int length, bool upperCase = true, bool lowerCase = true, bool number = false, bool special = false, string selectiveChars = "")
        {
            Random Random = new Random();

            if (!upperCase && !lowerCase && !number && !special && string.IsNullOrWhiteSpace(selectiveChars))
            {
                return new string(' ', length);
            }

            string text = selectiveChars;
            text += (upperCase ? "ABCDEFGHIJKLMNOPQRSTUVWXYZ" : string.Empty);
            text += (lowerCase ? "abcdefghijklmnopqrstuvwxyz" : string.Empty);
            text += (number ? "0123456789" : string.Empty);
            text += (special ? "~!@#$%^&*()+=" : string.Empty);
            return new string((from s in Enumerable.Repeat(text, length)
                               select s[Random.Next(s.Length)]).ToArray());
        }
    }
}
