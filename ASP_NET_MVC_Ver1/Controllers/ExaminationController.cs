using ASP_NET_MVC_Ver1.Areas.Identity.Data;
using ASP_NET_MVC_Ver1.Enum;
using ASP_NET_MVC_Ver1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASP_NET_MVC_Ver1.Controllers
{
    public class ExaminationController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExaminationController(ApplicationDbContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "Admin,Manager,Doctor,Nurse,Parent")]
        public IActionResult Index()
        {
            IEnumerable<Examination> objCatlist = _context.Examination;
            return View(objCatlist);
        }
        [Authorize(Roles = "Admin,Manager,Doctor,Nurse,Parent")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager,Doctor,Nurse,Parent")]
        public IActionResult Create(Examination empobj)
        {
            if (ModelState.IsValid)
            {
                _context.Examination.Add(empobj);
                _context.SaveChanges();
                TempData["ResultOk"] = "Thêm thông tin bệnh án thành công !";
                return RedirectToAction("Index");
            }

            return View(empobj);
        }
        [Authorize(Roles = "Admin,Manager,Doctor,Nurse,Parent")]
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var empfromdb = _context.Examination.Find(id);

            if (empfromdb == null)
            {
                return NotFound();
            }
            return View(empfromdb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager,Doctor,Nurse,Parent")]
        public IActionResult Edit(Examination empobj)
        {
            if (ModelState.IsValid)
            {
                _context.Examination.Update(empobj);
                _context.SaveChanges();
                TempData["ResultOk"] = "Thông tin hồ sơ bệnh án cập nhật thành công !";
                return RedirectToAction("Index");
            }

            return View(empobj);
        }
    }

}
