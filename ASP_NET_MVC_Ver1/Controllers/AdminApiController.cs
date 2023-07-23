using ASP_NET_MVC_Ver1.Areas.Identity.Data;
using ASP_NET_MVC_Ver1.Enum;
using ASP_NET_MVC_Ver1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASP_NET_MVC_Ver1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _uid;

        public AdminApiController(ApplicationDbContext context, UserManager<ApplicationUser> uid)
        {
            _uid = uid;
            _context = context;
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            try
            {
                // demo
                var draw = Request.Query["draw"].FirstOrDefault();
                var start = Request.Query["start"].FirstOrDefault();
                var length = Request.Query["length"].FirstOrDefault();
                var sortColumn = Request.Query["columns[" + Request.Query["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Query["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Query["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                var customerData = (from tempcustomer in _uid.Users select tempcustomer);


                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    //customerData = customerData.OrderBy(tempcustomer => tempcustomer.GetType().GetProperty(sortColumn).GetValue(tempcustomer, null));
                    //customerData = customerData.OrderBy(sortColumn + " " + sortColumnDirection);
                    if (sortColumn == "firstName")
                    {
                        customerData = sortColumnDirection == "desc" ?
                            customerData.OrderByDescending(x => x.FirstName) :
                            customerData.OrderBy(x => x.FirstName);
                    }
                    else if (sortColumn == "lastName")
                    {
                        customerData = sortColumnDirection == "desc" ?
                            customerData.OrderByDescending(x => x.LastName) :
                            customerData.OrderBy(x => x.LastName);
                    }
                    else if (sortColumn == "email")
                    {
                        customerData = sortColumnDirection == "desc" ?
                            customerData.OrderByDescending(x => x.Email) :
                            customerData.OrderBy(x => x.Email);
                    }
                    else if (sortColumn == "phoneNumber")
                    {
                        customerData = sortColumnDirection == "desc" ?
                            customerData.OrderByDescending(x => x.PhoneNumber) :
                            customerData.OrderBy(x => x.PhoneNumber);
                    }
                    else
                    {
                        customerData = sortColumnDirection == "desc" ?
                            customerData.OrderByDescending(x => EF.Property<object>(x, sortColumn)) :
                            customerData.OrderBy(x => EF.Property<object>(x, sortColumn));
                    }
                }
                if (!string.IsNullOrEmpty(searchValue))
                {
                    customerData = customerData.Where(m => m.FirstName.Contains(searchValue)
                                                           || m.LastName.Contains(searchValue)
                                                           || m.Email.Contains(searchValue)
                    );
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
        [Authorize(Roles = "Admin,Manager,Doctor,Nurse,Parent")]
        [HttpDelete]
        [Route("DeleteEmp")]
        public async Task<IActionResult> DeleteEmp(string id)
        {
            ApplicationUser user = await _uid.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await _uid.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return Ok();
                }
            }
            return Ok();
        }
    }
}
