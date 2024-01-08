using Microsoft.AspNetCore.Identity;

namespace AspNetCoreIdentity.Web.Localization
{
    public class LocalizationIdentityErrorDescriber : IdentityErrorDescriber
    {
        public override IdentityError DuplicateUserName(string userName)
        {
            return new() { Code = "DuplicateUserName", Description = $" {userName} bu kullanıcı adı başka bir kullanıcı tarafından kullanılıyor !" };
        }

        public override IdentityError DuplicateEmail(string email)
        {
            return new() { Code = "DuplicateEmail", Description = $" {email} mail adresi başka bir kullanıcı tarafından kullanılıyor !" };
        }

        public override IdentityError PasswordTooShort(int length)
        {
            return new() { Code = "PasswordTooShort", Description = "Şifre en az 10 karakterli olmalıdır !" };

        }
    }
}
