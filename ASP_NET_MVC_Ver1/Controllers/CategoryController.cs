using ASP_NET_MVC_Ver1.Areas.Identity.Data;
using ASP_NET_MVC_Ver1.Enum;
using ASP_NET_MVC_Ver1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASP_NET_MVC_Ver1.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }
        // [Authorize(Roles = "Admin,Manager,Doctor,Nurse,Parent,Children")]
        public IActionResult Index()
        {
            IEnumerable<Category> objCatlist = _context.Categories;
            return View(objCatlist);
        }
    }
}
