using ASP_NET_MVC_Ver1.Areas.Identity.Data;
using ASP_NET_MVC_Ver1.Enum;
using ASP_NET_MVC_Ver1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ASP_NET_MVC_Ver1.Controllers
{
    public class ChildrenController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _uid;

        public ChildrenController(ApplicationDbContext context, UserManager<ApplicationUser> uid)
        {
            _uid = uid;
            _context = context;
        }
        [Authorize(Roles = "Admin,Manager,Doctor,Nurse,Parent,Children")]
        public IActionResult Index()
        {
            IEnumerable<Children> objCatlist = _context.Childrens;
            return View(objCatlist);
        }
        [Authorize(Roles = "Admin,Manager,Doctor,Nurse,Parent,Children")]
        public IActionResult Create()
        {
            IEnumerable<User> parents = _context.Users;
            ViewBag.user_id = _uid.GetUserId(HttpContext.User);
            ViewBag.user_name = _uid.GetUserName(HttpContext.User);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager,Doctor,Nurse,Parent,Children")]
        public IActionResult Create(Children empobj)
        {
            if (ModelState.IsValid)
            {
                _context.Childrens.Add(empobj);
                _context.SaveChanges();
                TempData["ResultOk"] = "Thêm thông tin bé thành công !";
                return RedirectToAction("Index");
            }

            return View(empobj);
        }
        [Authorize(Roles = "Admin,Manager,Doctor,Nurse,Parent,Children")]
        public IActionResult Edit(Guid? id)
        {
            if (id == null )
            {
                return NotFound();
            }
            var empfromdb = _context.Childrens.Find(id);

            if (empfromdb == null)
            {
                return NotFound();
            }
            return View(empfromdb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager,Doctor,Nurse,Parent,Children")]
        public IActionResult Edit(Children empobj)
        {
            if (ModelState.IsValid)
            {
                _context.Childrens.Update(empobj);
                _context.SaveChanges();
                TempData["ResultOk"] = "Thông tin bé cập nhật thành công !";
                return RedirectToAction("Index");
            }

            return View(empobj);
        }

        [Authorize(Roles = "Admin,Manager,Doctor,Nurse,Parent,Children")]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var empfromdb = _context.Childrens.Find(id);

            if (empfromdb == null)
            {
                return NotFound();
            }
            return View(empfromdb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager,Doctor,Nurse,Parent,Children")]
        public IActionResult DeleteEmp(int? id)
        {
            var deleterecord = _context.Childrens.Find(id);
            if (deleterecord == null)
            {
                return NotFound();
            }
            _context.Childrens.Remove(deleterecord);
            _context.SaveChanges();
            TempData["ResultOk"] = "Thông tin bé xoá thành công !";
            return RedirectToAction("Index");
        }

    }
}
