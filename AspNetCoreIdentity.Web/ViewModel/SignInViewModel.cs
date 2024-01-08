using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentity.Web.ViewModel
{
    public class SignInViewModel
    {
        public SignInViewModel()
        {
                
        }
        public SignInViewModel(string email, string password)
        {
            Email = email;
            Password = password;
        }

        [EmailAddress(ErrorMessage = "Mail Format Yanlış")]
        [Required(ErrorMessage = "Mail alanı boş bırakılamaz")]
        [Display(Name = "E Mail Adresi")]
        public string Email { get; set; }
        
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Şifre Boş Olamaz")]
        [Display(Name = "Şifre")]
        public string Password { get; set; }

        [Display(Name = "Beni Hatırla")]
        public bool RememberMe { get; set; }
    }
}
