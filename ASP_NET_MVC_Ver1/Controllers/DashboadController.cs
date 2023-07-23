using ASP_NET_MVC_Ver1.Areas.Identity.Data;
using ASP_NET_MVC_Ver1.Enum;
using ASP_NET_MVC_Ver1.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ASP_NET_MVC_Ver1.Controllers
{
    public class DashboadController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _uid;

        public DashboadController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _uid = userManager;
        }
        public async Task<IActionResult> Index()
        // day la ham Task
}
