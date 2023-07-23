using ASP_NET_MVC_Ver1.Areas.Identity.Data;
using ASP_NET_MVC_Ver1.Enum;
using ASP_NET_MVC_Ver1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;

namespace ASP_NET_MVC_Ver1.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;
        string idUser;
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
            Category category = new Category();
            idUser = _userManager.GetUserId(HttpContext.User);
            category.CreateId = idUser;
            category.Description = "";
            return View(category);
        }
        public IActionResult Viewing(Guid Id)
        {
            {
                if (Id == null)
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

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager,Doctor,Nurse,Parent")]
        public IActionResult Create(Category empobj)
        {
            idUser = _userManager.GetUserId(HttpContext.User);
            empobj.CreateId = idUser;
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
        public IActionResult Edit(Guid Id)
        {
            if (Id == null)
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
        public IActionResult Delete(Guid Id)
        {
            if (Id == null)
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
        [Authorize(Roles = "Admin,Manager,Doctor,Nurse,Parent,Children")]
        public IActionResult DeleteEmp(Guid Id)
        {
            var deleterecord = _context.Categories.Find(Id);
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

