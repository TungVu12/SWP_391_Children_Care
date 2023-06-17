using ASP_NET_MVC_Ver1.Areas.Identity.Data;
using ASP_NET_MVC_Ver1.Enum;
using ASP_NET_MVC_Ver1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ASP_NET_MVC_Ver1.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;
        public CategoryController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [Authorize(Roles = "Admin,Manager,Doctor,Nurse,Parent,Children")]
        public IActionResult Index()
        {
            IEnumerable<Category> objCatlist = _context.Categories;
            return View(objCatlist);
        }
        [Authorize(Roles = "Admin,Manager,Doctor,Nurse,Parent")]
        public IActionResult Create()
        {
            ViewBag.UserId = _userManager.GetUserId(HttpContext.User);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager,Doctor,Nurse,Parent")]
        public IActionResult Create(Category empobj)
        {
            if (ModelState.IsValid)
            {
                _context.Categories.Add(empobj);
                _context.SaveChanges();
                TempData["ResultOk"] = "Tạo thành công !";
                return RedirectToAction("Index");
            }

            return View(empobj);
        }
        [Authorize(Roles = "Admin,Manager,Doctor,Nurse,Parent")]
        public IActionResult Edit(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }
            var empfromdb = _context.Categories.Find(Id);

            if (empfromdb == null)
            {
                return NotFound();
            }
            return View(empfromdb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager,Doctor,Nurse,Parent")]
        public IActionResult Edit(Category empobj)
        {
            if (ModelState.IsValid)
            {
                _context.Categories.Update(empobj);
                _context.SaveChanges();
                TempData["ResultOk"] = "Cập nhập thành công !";
                return RedirectToAction("Index");
            }
            return View(empobj);
        }
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var empfromdb = _context.Categories.Find(id);

            if (empfromdb == null)
            {
                return NotFound();
            }
            return View(empfromdb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager,Doctor,Nurse,Parent,Children")]
        public IActionResult DeleteEmp(int? c_id)
        {
            var deleterecord = _context.Categories.Find(c_id);
            if (deleterecord == null)
            {
                return NotFound();
            }
            _context.Categories.Remove(deleterecord);
            _context.SaveChanges();
            TempData["ResultOk"] = "Thông tin xoá thành công !";
            return RedirectToAction("Index");
        }

    }
}

