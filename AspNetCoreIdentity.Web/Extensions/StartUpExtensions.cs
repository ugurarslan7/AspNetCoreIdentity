using AspNetCoreIdentity.Web.CustomValidations;
using AspNetCoreIdentity.Web.Localization;
using AspNetCoreIdentity.Web.Models;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreIdentity.Web.Extensions
{
    public static class StartUpExtensions
    {
        public static void AddIdentityWithExtension(this IServiceCollection services)
        {

            services.Configure<DataProtectionTokenProviderOptions>(opt =>
            {
                opt.TokenLifespan = TimeSpan.FromHours(2);//şifre sıfırlama link token ömrü
            });

            services.AddIdentity<AppUser, AppRole>(option =>
            {
                option.User.RequireUniqueEmail = true;          //Burada daha fazla ayar yapılabilir
                //option.User.AllowedUserNameCharacters = "abcdefghijklmnoprstuvyzwq0123456789_.-@"; //username kısmında hangi karakterlere izin vericeksin
                option.Password.RequiredLength = 8;
                option.Password.RequireNonAlphanumeric = true;

                option.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
                option.Lockout.MaxFailedAccessAttempts = 3;

            }).AddPasswordValidator<PasswordValidator>().
            AddUserValidator<UserValidator>().
            AddErrorDescriber<LocalizationIdentityErrorDescriber>().
            AddEntityFrameworkStores<AppDbContext>().
            AddDefaultTokenProviders();
        }
    }
}
