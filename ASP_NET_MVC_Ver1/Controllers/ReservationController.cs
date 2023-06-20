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
        bool isAdmin = false;
        bool isDoctor = false;
        bool isParent = false;
        string idUser;
        //bool isDoctor = User.IsInRole(Roles.Doctor.ToString());

        public ReservationController(ApplicationDbContext context, UserManager<ApplicationUser> uid)
        {
            _uid = uid;
            _context = context;
            //if(User != null)
            //{
            //isAdmin = User.IsInRole(Roles.Admin.ToString());
            //isDoctor = User.IsInRole(Roles.Doctor.ToString());
            //isParent = User.IsInRole(Roles.Parent.ToString());
            //idUser = _uid.GetUserId(HttpContext.User);
            //}
        }

        [Authorize(Roles = "Admin,Manager,Doctor,Nurse,Parent,Children")]
        public IActionResult Index()
        {
            isAdmin = User.IsInRole(Roles.Admin.ToString());
            isDoctor = User.IsInRole(Roles.Doctor.ToString());
            isParent = User.IsInRole(Roles.Parent.ToString());
            idUser = _uid.GetUserId(HttpContext.User);
            IEnumerable<Reservation> objCatlist = _context.Reservations;
            var result = new List<Reservation>();
                result = objCatlist.ToList();
            if (isAdmin)
            {
                return View(result);
            }
            else if (isDoctor)
            {
                var filter = result.Where(c => c.doctor_id == idUser).ToList();
                return View(filter);
                //result = (List<Reservation>)result.Where(c => c.doctor_id == idUser);
            }
            else if (isParent)
            {
                var filter = result.Where(c => c.parent_id == idUser).ToList();
                return View(filter);
            }
            else
            {
                result = new List<Reservation>();
                return View(result);    
            }

        }

        [Authorize(Roles = "Admin,Manager,Doctor,Nurse,Parent,Children")]
        public IActionResult Create()
        {
            //string username = User.Identity.Name;
            //bool isAuthenticated = User.Identity.IsAuthenticated;
           



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

            var reservationSend = new Reservation();
            if(!isAdmin || !isDoctor)
            {
            reservationSend.status = StatusSending.Process;
            }
            return View(reservationSend);
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

        [HttpGet]
        //[Authorize(Roles = "Admin,Manager,Doctor,Nurse,Parent,Children")]
        public IActionResult Approve(Guid? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }
            var empfromdb = _context.Reservations.Find(Id);
            if (empfromdb == null)
            {
                return NotFound();
            }
            return View(empfromdb);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        //[Authorize(Roles = "Admin,Manager,Doctor,Nurse,Parent,Children")]
        public IActionResult Approve(Reservation empobj)
        {
            if (ModelState.IsValid)
            {
                _context.Reservations.Update(empobj);
                _context.SaveChanges();
                TempData["ResultOk"] = "Lịch khám cho bé cập nhật thành công !";
                return RedirectToAction("Index");
            }

            return View(empobj);
        }
    }
}
