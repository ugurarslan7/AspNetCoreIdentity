using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentity.Web.ViewModel
{
    public class PasswordChangeViewModel
    {
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Şifre Boş Olamaz")]
        [Display(Name = "Eski Şifre")]
        [MinLength(8)]
        public string PasswordOld { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Yeni Şifre Boş Olamaz")]
        [Display(Name = "Yeni Şifre")]
        [MinLength(8)]
        public string PasswordNew { get; set; }

        [DataType(DataType.Password)]
        [Compare(nameof(PasswordNew), ErrorMessage = "Şifreler aynı değildir !")]
        [Required(ErrorMessage = "Şifre Tekrar Alanı Boş Olamaz")]
        [Display(Name = "Yeni Şifre Tekrar")]
        [MinLength(8)]
        public string PasswordConfirm { get; set; }
    }
}
