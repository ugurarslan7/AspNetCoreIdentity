using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentity.Web.ViewModel
{
    public class SignUpViewModel
    {
        public SignUpViewModel()
        {

        }
        public SignUpViewModel(string userName, string mail, string phone, string password)
        {
            UserName = userName;
            Mail = mail;
            Phone = phone;
            Password = password;
        }
        [Required(ErrorMessage ="Kullanıcı Adı Boş Olamaz")]
        [Display(Name = "Kullanıcı Adı")]
        public string UserName { get; set; }

        [EmailAddress(ErrorMessage ="Mail Format Yanlış")]
        [Required(ErrorMessage ="Mail alanı boş bırakılamaz")]
        [Display(Name = "E Mail Adresi")]
        public string Mail { get; set; }

        [Display(Name = "Telefon")]
        public string? Phone { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Şifre Boş Olamaz")]
        [Display(Name = "Şifre")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare(nameof(Password),ErrorMessage ="Şifreler aynı değildir !")]
        [Required(ErrorMessage = "Şifre Boş Olamaz")]
        [Display(Name = "Şifre Tekrar")]
        public string PasswordConfirm { get; set; }
    }
}
