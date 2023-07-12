using ASP_NET_MVC_Ver1.Areas.Identity.Data;
using ASP_NET_MVC_Ver1.Common;
using ASP_NET_MVC_Ver1.Enum;
using ASP_NET_MVC_Ver1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ASP_NET_MVC_Ver1.Controllers
{
    public class ChildrenController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _uid;
        bool isAdmin = false;
        bool isDoctor = false;
        bool isParent = false;
        string idUser;
        //private readonly UserManager<IdentityUser> _userManager;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            isAdmin = User.IsInRole(Roles.Admin.ToString());
            isDoctor = User.IsInRole(Roles.Doctor.ToString());
            isParent = User.IsInRole(Roles.Parent.ToString());
            idUser = _uid.GetUserId(HttpContext.User);
            ViewBag.Admin = isAdmin;
            ViewBag.Doctor = isDoctor;
            ViewBag.Parent = isParent;
            base.OnActionExecuting(context);
        }

        public ChildrenController(ApplicationDbContext context, UserManager<ApplicationUser> uid)
        {
            _uid = uid;
            _context = context;
        }


        [Authorize(Roles = "Admin,Manager,Doctor,Nurse,Parent")]
        public IActionResult Index()
        {
            IEnumerable<Children> objCatlist = _context.Childrens;
            List<Children> filterChildren = new List<Children>();

            if(isAdmin || isDoctor)
            {
                filterChildren = objCatlist.ToList();
            }
            else
            {
                filterChildren = objCatlist.Where(c => c.parent_id == idUser).ToList();
            }
            return View(filterChildren);
        }
        [Authorize(Roles = "Admin,Manager,Doctor,Nurse,Parent")]
        public async Task<IActionResult> Create()
        {
            //test
            //IEnumerable<User> parents = _context.Users;
            if (isAdmin || isDoctor)
            {
                var getData = await _uid.GetUsersInRoleAsync(Roles.Parent.ToString());
                var filterParent = getData.Where(c => c.Email != "admin@gmail.com").ToList();
                List<SelectListItem> listForSelect = new List<SelectListItem>();
                foreach (var parent in filterParent)
                {
                    if (parent.Id.ToString() != null)
                    {
                        listForSelect.Add(new SelectListItem { Text = parent.Id.ToString(), Value = parent.FirstName.ToString() + " " + parent.LastName.ToString() });

                    }
                }
                ViewBag.parent_lst = listForSelect;

            }
            else
            {
                var resultModel = _uid.GetUserAsync(HttpContext.User);
                ViewBag.parentName = resultModel.Result.FirstName + resultModel.Result.LastName;


            }

            //var filter = parents.ToList();
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
                if (isParent && !isAdmin)
                {
                    empobj.parent_id = _uid.GetUserId(HttpContext.User);
                }
                empobj.CreateDate= DateTime.Now;
                _context.Childrens.Add(empobj);
                _context.SaveChanges();
                TempData["ResultOk"] = "Thêm thông tin bé thành công !";
                return RedirectToAction("Index");
            }
            else
            {
                if(isAdmin || isDoctor)
                {
                    //
                }
                else
                {
                    var resultModel = _uid.GetUserAsync(HttpContext.User);
                    ViewBag.parentName = resultModel.Result.FirstName + resultModel.Result.LastName;
                }
            }

            return View(empobj);
        }
        [Authorize(Roles = "Admin,Manager,Doctor,Nurse,Parent,Children")]
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
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
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
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
        public IActionResult DeleteEmp(Guid? id)
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
