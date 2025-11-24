using AspNetCoreIdentity.Web.BackgroundJobs;
using AspNetCoreIdentity.Web.Extensions;
using AspNetCoreIdentity.Web.Filters;
using AspNetCoreIdentity.Web.Models;
using AspNetCoreIdentity.Web.Services;
using AspNetCoreIdentity.Web.ViewModel;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace AspNetCoreIdentity.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;  // Kullanıcı cookie olusturması ile ilgili önemli işlemler (login-logout- facebook ile giriş gibi)
        private readonly IEmailService _emailService;
        public HomeController(ILogger<HomeController> logger, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IEmailService emailService)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
        }

        [CustomHandleExceptionFilterAttribute(ErrorPage= "ErrorCustom1")]
        public IActionResult Index()
        {
            int a = 45;
            int b= 0;
            int c = a / b;
            return View();
        }

        [CustomHandleExceptionFilterAttribute(ErrorPage = "ErrorCustom2")]
        public IActionResult Privacy()
        {
            int a = 45;
            int b = 0;
            int c = a / b;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var exception = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            ErrorViewModel errorModel = new();
            errorModel.Path = exception.Path;
            errorModel.Message = exception.Error.Message;

            return View(errorModel);
        }


        public IActionResult ErrorCustom1()
        {
            return View();
        }

        public IActionResult ErrorCustom2()
        {
            return View();
        }

        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel signUpViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var identityResult = await _userManager.CreateAsync(new() { UserName = signUpViewModel.UserName, PhoneNumber = signUpViewModel.Phone, Email = signUpViewModel.Mail }, signUpViewModel.Password);
            if (!identityResult.Succeeded)
            {
                ModelState.AddModelErrorList(identityResult.Errors.Select(x => x.Description).ToList());
                return View();
            }

            var exchangeExpireClaim = new Claim("ExchangeExpiredDate", DateTime.Now.AddDays(10).ToString());

            var currentUser = await _userManager.FindByNameAsync(signUpViewModel.UserName);

            var claimResult = await _userManager.AddClaimAsync(currentUser!, exchangeExpireClaim);

            if (!claimResult.Succeeded)
            {
                ModelState.AddModelErrorList(identityResult.Errors.Select(x => x.Description).ToList());
                return View();
            }

            TempData["SuccessMessage"] = "Üyelik işlemi başarıyla gerçekleşmiştir."; //TempData -> Cookie tek seferlik taşınır

            FireAndForgetJobs.EmailsendToUserJob(signUpViewModel.Mail, "Aramıza hoşgeldiniz :)");

            return RedirectToAction(nameof(SignUp));
        }


        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel signInViewModel, string retunrurl = null)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            retunrurl ??= Url.Action("Index", "Member");

            var hasUser = await _userManager.FindByEmailAsync(signInViewModel.Email);
            if (hasUser == null)
            {
                ModelState.AddModelError(string.Empty, "Email veya Şifre yanlış");
                return View();
            }

            var signinResult = await _signInManager.PasswordSignInAsync(hasUser.UserName, signInViewModel.Password, signInViewModel.RememberMe, true);

            if (signinResult.IsLockedOut)
            {
                ModelState.AddModelErrorList(new List<string>() { "10 dakika boyunca giriş yapamazsınız !" });
                return View();
            }

            if (!signinResult.Succeeded)
            {
                ModelState.AddModelErrorList(new List<string>() { "Email veya şifre hatalı !", $"Başarısız giriş sayısı={await _userManager.GetAccessFailedCountAsync(hasUser)}" });
                return View();
            }

            if (hasUser.Birthdate.HasValue)
            {
                await _signInManager.SignInWithClaimsAsync(hasUser, signInViewModel.RememberMe, new[] {new Claim("Birthdate",
                    hasUser.Birthdate.Value.ToString())});
            }

            return Redirect(retunrurl);
        }

        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel model)
        {
            var hasuser = await _userManager.FindByEmailAsync(model.Email);

            if (hasuser == null)
            {
                ModelState.AddModelError(String.Empty, "Böyle bir mail adresi bulunamadı !");
                return View();
            }

            string passwordToken = await _userManager.GeneratePasswordResetTokenAsync(hasuser);
            var passwordResetLink = Url.Action("ResetPassword", "Home", new { userId = hasuser.Id, Token = passwordToken }, HttpContext.Request.Scheme);

            await _emailService.SendResetPasswordEmail(passwordResetLink, hasuser.Email);


            TempData["SuccessMessage"] = "Şifre yenileme linki e-posta adresinize gönderilmiştir.";

            return RedirectToAction(nameof(ForgetPassword));
        }

        public IActionResult ResetPassword(string userId, string token)
        {
            TempData["userId"] = userId;
            TempData["token"] = token;



            return View();

        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            var userID = TempData["userId"];
            var token = TempData["token"];

            if (token == null || userID == null)
            {
                throw new Exception("Bir hata meydana geldi.");
            }

            var hasUser = await _userManager.FindByIdAsync(userID.ToString()!);
            if (hasUser == null)
            {
                ModelState.AddModelError(String.Empty, "Kullanıcı bulunamamıştır.");
                return View();

            }

            var result = await _userManager.ResetPasswordAsync(hasUser, token.ToString()!, model.Password);
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Şifreniz başarı ile yenilenmiştir.";
            }
            else
            {
                ModelState.AddModelErrorList(result.Errors.Select(p => p.Description).ToList());
            }

            return View();

        }
    }
}