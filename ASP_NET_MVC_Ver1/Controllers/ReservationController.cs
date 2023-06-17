using ASP_NET_MVC_Ver1.Areas.Identity.Data;
using ASP_NET_MVC_Ver1.Enum;
using ASP_NET_MVC_Ver1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections;
using System.Collections.Generic;

namespace ASP_NET_MVC_Ver1.Controllers
{
    public class ReservationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _uid;

        public ReservationController(ApplicationDbContext context, UserManager<ApplicationUser> uid)
        {
            _uid = uid;
            _context = context;
        }

        // [Authorize(Roles = "Admin,Manager,Doctor,Nurse,Parent,Children")]
        public IActionResult Index()
        {
            IEnumerable<Reservation> objCatlist = _context.Reservations;
            return View(objCatlist);
        }

        [Authorize(Roles = "Admin,Manager,Doctor,Nurse,Parent,Children")]
        public IActionResult Create()
        {

            //List<Category> cate = dbCate.Categories.ToList();

            // Tạo SelectList
            //SelectList cateList = new SelectList(cate, "ID", "THELOAI_NAME");

            // Set vào ViewBag
            //ViewBag.CategoryList = cateList;

            IEnumerable<User> parents = _context.Users;
            IEnumerable<Children> childrens = _context.Childrens;

            List<Children> cate = _context.Childrens.ToList();
            SelectList cateList = new SelectList(cate, "Id", "LastName");

            foreach (var child in childrens)
            {
                if (child.parent_id.Equals(_uid.GetUserId(HttpContext.User)))
                {
                    Children c = new Children();
                    cate.Add(c);
                    ////new SelectListItem { Text = child.FirstName + " " + child.LastName };
                }
            }
            var result = cateList.Items;

            List<SelectListItem> listForSelect = new List<SelectListItem>();
            foreach (var child in cate)
            {
                if (child.Id.ToString() != null && child.parent_id != null)
                {
                    listForSelect.Add(new SelectListItem { Text = child.Id.ToString(), Value = child.LastName.ToString() });

                }
            }

            ViewBag.customValue = listForSelect;

            ViewBag.CategoryList = cateList;

            ViewBag.Byme = result;
            ViewBag.doctor_id = "08db6dc6-8c42-4fb1-80f8-637a7c9ba002";
            ViewBag.child_id = "08db6dc6-8c42-4fb1-80f8-637a7c9ba002";
            ViewBag.user_id = _uid.GetUserId(HttpContext.User);
            ViewBag.user_name = _uid.GetUserName(HttpContext.User);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager,Doctor,Nurse,Parent,Children")]
        public IActionResult Create(Reservation empobj)
        {

            if (ModelState.IsValid)
            {
                _context.Reservations.Add(empobj);
                _context.SaveChanges();
                TempData["ResultOk"] = "Yêu cầu đặt lịch đã được gửi đi!";
                return RedirectToAction("Index");
            }

            return View(empobj);
        }
    }
}
