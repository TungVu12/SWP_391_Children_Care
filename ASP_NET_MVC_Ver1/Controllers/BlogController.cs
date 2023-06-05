using Microsoft.AspNetCore.Mvc;

namespace ASP_NET_MVC_Ver1.Controllers
{
    public class BlogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        //public IActionResult Blogs()
        //{
        //    return View();
        //}

        public IActionResult BlogsMXH()
        {
            return View();
        }
    }
}
