using AspNetCoreIdentity.Web.Models;
using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentity.Web.ViewModel
{
    public class UserEditViewModel
    {
        [Required(ErrorMessage = "Kullanıcı Adı Boş Olamaz")]
        [Display(Name = "Kullanıcı Adı")]
        public string UserName { get; set; }

        [EmailAddress(ErrorMessage = "Mail Format Yanlış")]
        [Required(ErrorMessage = "Mail alanı boş bırakılamaz")]
        [Display(Name = "E Mail Adresi")]
        public string Mail { get; set; }

        [Required(ErrorMessage = "Telefon Boş Olamaz")]
        [Display(Name = "Telefon")]
        public string Phone { get; set; }

        [Display(Name = "Dogum Tarihi")]
        [DataType(DataType.Date)]
        public DateTime? Birthdate { get; set; }

        [Display(Name = "Şehir")]
        public string? City { get; set; }

        [Display(Name = "Profil Resmi")]
        public IFormFile? Photo { get; set; }

        [Display(Name = "Cinsiyet")]
        public Gender? Gender { get; set; }
    }
}
