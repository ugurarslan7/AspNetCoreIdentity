using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentity.Web.ViewModel
{
    public class ForgetPasswordViewModel
    {
        [EmailAddress(ErrorMessage = "Mail Format Yanlış")]
        [Required(ErrorMessage = "Mail alanı boş bırakılamaz")]
        [Display(Name = "E Mail Adresi")]
        public string Email { get; set; }
    }
}
