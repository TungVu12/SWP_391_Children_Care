using ASP_NET_MVC_Ver1.Areas.Identity.Data;
using ASP_NET_MVC_Ver1.Models;
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

        public IActionResult Index()
        {
            try
            {
                var draw = Request.Query["draw"].FirstOrDefault();
                var start = Request.Query["start"].FirstOrDefault();
                var length = Request.Query["length"].FirstOrDefault();
                var sortColumn = Request.Query["columns[" + Request.Query["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Query["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Query["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                //var customerData = (from tempcustomer in _context.Childrens select tempcustomer);
                var customerData = from user in _uid.Users
                                   join userRole in _context.UserRoles on user.Id equals userRole.UserId into tempTable
                                   from leftJoinData in tempTable.DefaultIfEmpty()
                                   select new UserRolesViewModel
                                   {
                                       UserId = user.Id,
                                       Email = user.Email,
                                       FirstName = user.FirstName,
                                       LastName = user.LastName,
                                       Roles = leftJoinData.RoleId != null ?
                                           new List<string> { _context.Roles.FirstOrDefault(x => string.Equals(x.Id, leftJoinData.RoleId)).Name } :
                                           new List<string>()
                                   };


                //if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                //{
                //    if (sortColumnDirection == "desc")
                //    {
                //        customerData = customerData.OrderByDescending(x => EF.Property<object>(x, sortColumn));
                //    }
                //    else
                //    {
                //        customerData = customerData.OrderBy(x => EF.Property<object>(x, sortColumn));
                //    }
                //}

                if (!string.IsNullOrEmpty(searchValue))
                {
                    customerData = customerData.Where(m => m.FirstName.Contains(searchValue) || m.LastName.Contains(searchValue));
                }

                recordsTotal = customerData.Count();
                int sttCounter = skip + 1;
                var data = customerData.Skip(skip).Take(pageSize).ToList();
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);

            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
