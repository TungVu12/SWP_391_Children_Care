using ASP_NET_MVC_Ver1.Areas.Identity.Data;
using ASP_NET_MVC_Ver1.Common;
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
    public class ExaminationApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _uid;
        bool isAdmin = false;
        bool isDoctor = false;
        bool isParent = false;
        string idUser;

        public ExaminationApiController(ApplicationDbContext context, UserManager<ApplicationUser> uid)
        {
            _uid = uid;
            _context = context;
        }
        [HttpGet]
        [Authorize(Roles = "Admin,Manager,Doctor,Parent")]
        public IActionResult Index()
        {
            try
            {
                isAdmin = User.IsInRole(Roles.Admin.ToString()) ? true : false;
                isDoctor = User.IsInRole(Roles.Doctor.ToString()) ? true : false;
                isParent = User.IsInRole(Roles.Parent.ToString()) ? true : false;
                idUser = _uid.GetUserId(HttpContext.User);

                var draw = Request.Query["draw"].FirstOrDefault();
                var start = Request.Query["start"].FirstOrDefault();
                var length = Request.Query["length"].FirstOrDefault();
                var sortColumn = Request.Query["columns[" + Request.Query["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Query["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Query["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                var customerData = from tempcustomer in _context.Examination
                                   join usersTable in _uid.Users on tempcustomer.parent_id equals usersTable.Id into tempTable
                                   from tb1 in tempTable.DefaultIfEmpty()
                                   join usersTable2 in _uid.Users on tempcustomer.doctor_id equals usersTable2.Id into tempTable2
                                   from tb2 in tempTable2.DefaultIfEmpty()
                                   join childrenTable in _context.Childrens on tempcustomer.child_id equals childrenTable.Id.ToString() into tempTable3
                                   from tb3 in tempTable3.DefaultIfEmpty()
                                   join categoryTable in _context.Categories on tempcustomer.category equals categoryTable.Id.ToString() into tempTable4
                                   from tb4 in tempTable4.DefaultIfEmpty()
                                   select new
                                   {
                                       tempcustomer.Id,
                                       DoctorName = tb2.FirstName + " " + tb2.LastName,
                                       ParentName = tb1.FirstName + " " + tb1.LastName,
                                       ChildFullName = tb3.FullName,
                                       CategoryTitle = tb4.Title,
                                       tempcustomer.RegistrationDate,
                                       tempcustomer.doctor_id,
                                       tempcustomer.parent_id
                                   };


                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    if (sortColumn == "parentName")
                    {
                        customerData = sortColumnDirection == "desc" ?
                            customerData.OrderByDescending(x => x.parent_id) :
                            customerData.OrderBy(x => x.ParentName);
                    }
                    else if (sortColumn == "doctorName")
                    {
                        customerData = sortColumnDirection == "desc" ?
                            customerData.OrderByDescending(x => x.doctor_id) :
                            customerData.OrderBy(x => x.DoctorName);
                    } 
                    else if (sortColumn == "childFullName")
                    {
                        customerData = sortColumnDirection == "desc" ?
                            customerData.OrderByDescending(x => x.ChildFullName) :
                            customerData.OrderBy(x => x.ChildFullName);
                    }
                    else if (sortColumn == "categoryTitle")
                    {
                        customerData = sortColumnDirection == "desc" ?
                            customerData.OrderByDescending(x => x.CategoryTitle) :
                            customerData.OrderBy(x => x.CategoryTitle);
                    }
                    else if (sortColumn == "registrationDate")
                    {
                        customerData = sortColumnDirection == "desc" ?
                            customerData.OrderByDescending(x => x.RegistrationDate) :
                            customerData.OrderBy(x => x.RegistrationDate);
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
                    customerData = customerData.Where(m => m.ChildFullName.Contains(searchValue));
                }
                if (isAdmin)
                {
                    customerData = customerData;
                }
                else if (isDoctor && !isAdmin)
                {
                    customerData = customerData.Where(c => c.doctor_id == idUser);

                }
                else if (isParent && !isAdmin)
                {
                    customerData = customerData.Where(c => c.parent_id == idUser);
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
