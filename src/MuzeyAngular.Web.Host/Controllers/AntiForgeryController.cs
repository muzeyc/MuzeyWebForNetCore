using Microsoft.AspNetCore.Antiforgery;
using MuzeyAngular.Controllers;

namespace MuzeyAngular.Web.Host.Controllers
{
    public class AntiForgeryController : MuzeyAngularControllerBase
    {
        private readonly IAntiforgery _antiforgery;

        public AntiForgeryController(IAntiforgery antiforgery)
        {
            _antiforgery = antiforgery;
        }

        public void GetToken()
        {
            _antiforgery.SetCookieTokenAndHeader(HttpContext);
        }
    }
}
