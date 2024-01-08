using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentity.Web.ViewModel
{
    public class ResetPasswordViewModel
    {
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Şifre Boş Olamaz")]
        [Display(Name = "Yeni Şifre")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Şifreler aynı değildir !")]
        [Required(ErrorMessage = "Şifre Boş Olamaz")]
        [Display(Name = "Yeni Şifre Tekrar")]
        public string PasswordConfirm { get; set; }
    }
}
