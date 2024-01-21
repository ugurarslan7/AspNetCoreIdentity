using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreIdentity.Web.Controllers
{
    public class OrderController : Controller
    {
        [Authorize(Policy = "OrderPermissionRead")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
