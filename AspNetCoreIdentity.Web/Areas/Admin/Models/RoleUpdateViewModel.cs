using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentity.Web.Areas.Admin.Models
{
    public class RoleUpdateViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Rol İsmi Boş Olamaz")]
        [Display(Name = "Rol İsmi")]
        public string Name { get; set; }
    }
}
