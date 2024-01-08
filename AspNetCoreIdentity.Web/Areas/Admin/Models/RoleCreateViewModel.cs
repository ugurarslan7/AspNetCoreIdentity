using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentity.Web.Areas.Admin.Models
{
    public class RoleCreateViewModel
    {
        [Required(ErrorMessage = "Rol ismi Boş Olamaz")]
        [Display(Name = "Rol İsmi")]
        public string Name { get; set; }
    }
}
