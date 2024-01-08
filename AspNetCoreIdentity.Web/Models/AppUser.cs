using Microsoft.AspNetCore.Identity;

namespace AspNetCoreIdentity.Web.Models
{
    public class AppUser : IdentityUser
    {
        public string? City { get; set; }
        public string? Photo { get; set; }

        public DateTime? Birthdate { get; set; }

        public Gender? Gender { get; set; }
    }
}
