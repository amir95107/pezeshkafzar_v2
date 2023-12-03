using System.ComponentModel.DataAnnotations;

namespace Test2.Models
{
    public class RegisterViewModel
    {
        [Display(Name = "نام کامل")]
        public string FullName { get; set; }

        [Display(Name = "موبایل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Mobile { get; set; }

        [Display(Name = "تلفن")]
        public string Telephone { get; set; }

        [Display(Name = "نام فروشگاه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string StoreName { get; set; }


        [Display(Name = "آدرس فروشگاه")]
        [MaxLength(800)]
        public string StoreAddress { get; set; }

        [Display(Name = "ایمیل")]
        public string Email { get; set; }

        [Display(Name = "کلمه عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "تکرار کلمه عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "کلمه های عبور مغایرت دارند")]
        public string RePassword { get; set; }
    }
}
