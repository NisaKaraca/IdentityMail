using Microsoft.AspNetCore.Mvc;

namespace IdentityMail.Web.ViewComponents.Admin
{
    public class _AdminSidebarViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
