using ASP_NET_MVC_Ver1.Areas.Identity.Data;
using ASP_NET_MVC_Ver1.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace ASP_NET_MVC_Ver1.Controllers
{
    [ApiController]
    [Route("api/GetData")]
    public class GetDataAPIController : Controller
    {
        private readonly ApplicationDbContext _context;
        public GetDataAPIController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        [Route("GetChildren")]
        public IActionResult ActionName(string parentId)
        {
            try
            {
                    List<SelectListItem> lstChildrens = new List<SelectListItem>();
                    List<Children> childrens = _context.Childrens.ToList();
                    foreach (var child in childrens)
                    {
                        if (child.Id.ToString() != null && child.parent_id.ToString() == parentId)
                        {
                            lstChildrens.Add(new SelectListItem { Text = child.Id.ToString(), Value = child.FullName.ToString() });
                        }
                    }
                return Ok(lstChildrens);
                
                // Xử lý logic dựa trên giá trị childId từ request

            }
            catch (Exception)
            {
                throw new Exception("Lỗi dữ liệu");
            }

        }
    }
}
