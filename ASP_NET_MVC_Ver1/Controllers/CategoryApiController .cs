using ASP_NET_MVC_Ver1.Areas.Identity.Data;
using ASP_NET_MVC_Ver1.Enum;
using ASP_NET_MVC_Ver1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Linq.Dynamic.Core;


namespace ASP_NET_MVC_Ver1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _uid;
        bool isAdmin = false;
        bool isDoctor = false;
        bool isParent = false;
        string idUser;

        //demo
        public CategoryApiController(ApplicationDbContext context, UserManager<ApplicationUser> uid)
        {
            _uid = uid;
            _context = context;
        }


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
                //var customerData = (from tempcustomer in _context.Childrens select tempcustomer);
                var customerData = from tempcustomer in _context.Categories
                                   join usersTable in _uid.Users on tempcustomer.CreateId equals usersTable.Id into tempTable
                                   from leftJoinData in tempTable.DefaultIfEmpty()
                                   select new
                                   {
                                       // Chọn các trường từ bảng Childrens và OtherTable
                                       tempcustomer.Id,
                                       tempcustomer.Title,
                                       tempcustomer.Content,
                                       //tempcustomer.Description,
                                       // Thêm các trường khác từ OtherTable
                                       leftJoinData.FirstName,
                                       leftJoinData.LastName
                                   };
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    customerData = customerData.OrderBy(sortColumn + " " + sortColumnDirection);
                }
                if (!string.IsNullOrEmpty(searchValue))
                {
                    customerData = customerData.Where(m => m.Title.Contains(searchValue) || m.Content.Contains(searchValue));
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

        [HttpDelete]
        [Route("DeleteEmp")]
        public IActionResult DeleteEmp(Guid id)
        {
            var deleterecord = _context.Categories.Find(id);
            if (deleterecord == null)
            {
                return NotFound();
            }
            _context.Categories.Remove(deleterecord);
            _context.SaveChanges();
            return Ok();
        }
    }
}
