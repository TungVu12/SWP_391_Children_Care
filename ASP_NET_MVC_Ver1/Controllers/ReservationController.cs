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
            _uid= uid;
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
            IEnumerable<User> parents = _context.Users;
            IEnumerable<Children> childrens = _context.Childrens;

            List<Children> childrenDemo = new List<Children>();
            List<SelectListItem> list_childs = new List<SelectListItem>();

            foreach (var child in childrens)
            {
                if (child.parent_id.Equals(_uid.GetUserId(HttpContext.User)))
                {
                    Children c = new Children(child.Id, child.FirstName, child.LastName, child.BirdthDate, child.Age, child.Description, child.Address, child.parent_id);
                    childrenDemo.Add(c);
                    ////new SelectListItem { Text = child.FirstName + " " + child.LastName };
                }
            }

            ViewBag.child_info = childrenDemo;
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
