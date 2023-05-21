using Microsoft.AspNetCore.Mvc;

namespace ASP_NET_MVC_Ver1.Controllers
{
    public class TestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
