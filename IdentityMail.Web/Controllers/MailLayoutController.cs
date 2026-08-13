using Microsoft.AspNetCore.Mvc;

namespace IdentityMail.Web.Controllers
{
    public class MailLayoutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
