using ASP_NET_MVC_Ver1.Areas.Identity.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ASP_NET_MVC_Ver1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRolesApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _uid;

        public UserRolesApiController(ApplicationDbContext context, UserManager<ApplicationUser> uid)
        {
            _uid = uid;
            _context = context;
        }
    }
}
