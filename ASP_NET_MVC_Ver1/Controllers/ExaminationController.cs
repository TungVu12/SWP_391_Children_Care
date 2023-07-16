using ASP_NET_MVC_Ver1.Areas.Identity.Data;
using ASP_NET_MVC_Ver1.Enum;
using ASP_NET_MVC_Ver1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ASP_NET_MVC_Ver1.Controllers
{
    public class ExaminationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _uid;
        bool isAdmin = false;
        bool isDoctor = false;
        bool isParent = false;
        string idUser;

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

        public ExaminationController(ApplicationDbContext context, UserManager<ApplicationUser> uid)
        {
            _context = context;
            _uid = uid;
        }
        [Authorize(Roles = "Admin,Doctor,Parent")]
        public async Task<IActionResult> Index()
        {

            List<Examination> examinations = _context.Examination.ToList();
            if (isAdmin)
            {
                examinations = examinations;
            }
            else if (isDoctor && !isAdmin)
            {
                examinations = examinations.Where(c => c.doctor_id == idUser).ToList();

            }
            else if (isParent && !isAdmin)
            {
                examinations = examinations.Where(c => c.parent_id == idUser).ToList();
            }

            List<Children> ChildrenLst = _context.Childrens.ToList();
            List<Category> CategoryLst = _context.Categories.ToList();
            var getData = await _uid.GetUsersInRoleAsync(Roles.Parent.ToString());
            var DoctorL = await _uid.GetUsersInRoleAsync(Roles.Doctor.ToString());
            var ParentLst = getData.Where(c => c.Email != "admin@gmail.com").ToList();
            var DoctorLst = DoctorL.Where(c => c.Email != "admin@gmail.com").ToList();

            var joinTable = from examination in examinations
                            join children in ChildrenLst on examination.child_id.ToString() equals children.Id.ToString()
                            join parent in ParentLst on examination.parent_id.ToString() equals parent.Id.ToString()
                            join doctor in DoctorLst on examination.doctor_id.ToString() equals doctor.Id.ToString()
                            join category in CategoryLst on examination.category.ToString() equals category.Id.ToString()
                            select new Examination
                            {
                                // Gán các thuộc tính khác của Reservation
                                Id = examination.Id,
                                parent_id = parent.FirstName + parent.LastName,
                                child_id = children.FullName,
                                doctor_id = doctor.FirstName + doctor.LastName,
                                category = category.Title,
                                Detail = examination.Detail,
                                r_content = examination.r_content,
                                RegistrationDate = examination.RegistrationDate,
                                reservation_id = examination.reservation_id,
                                Desciption = examination.Desciption,
                                // Gán tên của Children vào thuộc tính ChildrenName
                                // Hoặc có thể tạo một thuộc tính mới trong Reservation để lưu trữ tên của Children
                            };


            List<Examination> joinExaminations = joinTable.ToList();



            var result = new List<Examination>();
            result = joinExaminations.ToList();
            return View(result);
        }
        [Authorize(Roles = "Admin,Doctor")]
        public async Task<IActionResult> Create()
        {
            Examination examination = new Examination();
            examination.RegistrationDate = DateTime.Now;

            var getData = await _uid.GetUsersInRoleAsync(Roles.Doctor.ToString());
            var filterDoctor = getData.Where(c => c.Email != "admin@gmail.com").ToList();
            List<SelectListItem> lstDoctor = new List<SelectListItem>();
            foreach (var doctor in filterDoctor)
            {
                if (isDoctor && !isAdmin)
                {
                    if (doctor.Id.ToString() == idUser)
                    {
                        lstDoctor.Add(new SelectListItem { Text = doctor.Id.ToString(), Value = doctor.FirstName.ToString() + " " + doctor.LastName.ToString() });
                    }
                }
                else
                {
                    if (doctor.Id.ToString() != null)
                    {
                        lstDoctor.Add(new SelectListItem { Text = doctor.Id.ToString(), Value = doctor.FirstName.ToString() + " " + doctor.LastName.ToString() });

                    }
                }
            }

            var getDataParent = await _uid.GetUsersInRoleAsync(Roles.Parent.ToString());
            var filterParent = getDataParent.Where(c => c.Email != "admin@gmail.com").ToList();
            List<SelectListItem> lstParents = new List<SelectListItem>();
            foreach (var parent in filterParent)
            {
                if (parent.Id.ToString() != null)
                {
                    lstParents.Add(new SelectListItem { Text = parent.Id.ToString(), Value = parent.FirstName.ToString() + " " + parent.LastName.ToString() + " - " + parent.Email.ToString() });

                }
            }
            List<Category> categoryLst = _context.Categories.ToList();
            List<SelectListItem> category = new List<SelectListItem>();
            foreach (var item in categoryLst)
            {
                if (item.Id.ToString() != null)
                {
                    category.Add(new SelectListItem { Text = item.Id.ToString(), Value = item.Title.ToString() });
                }
            }
            ViewBag.category_lst = category;

            ViewBag.parent_lst = lstParents;
            ViewBag.doctor_lst = lstDoctor;

            return View(examination);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager,Doctor,Nurse,Parent")]
        public IActionResult Create(Examination empobj)
        {
            if (ModelState.IsValid)
            {
                Guid resId = new Guid(empobj.reservation_id);
                var empfromdb = _context.Reservations.Find(resId);
                empfromdb.examination_status = StatusExamination.Done;
                empfromdb.status = StatusSending.Approved;
                _context.Examination.Add(empobj);
                _context.Reservations.Update(empfromdb);
                _context.SaveChanges();

                TempData["ResultOk"] = "Thêm thông tin bệnh án thành công !";
                return RedirectToAction("Index");
            }

            return View(empobj);
        }
        [Authorize(Roles = "Admin,Manager,Doctor,Nurse,Parent")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var empfromdb = _context.Examination.Find(id);


            var getData = await _uid.GetUsersInRoleAsync(Roles.Doctor.ToString());
            var filterDoctor = getData.Where(c => c.Email != "admin@gmail.com").ToList();
            List<SelectListItem> lstDoctor = new List<SelectListItem>();
            foreach (var doctor in filterDoctor)
            {
                if (doctor.Id.ToString() == empfromdb.doctor_id.ToString())
                {
                    lstDoctor.Add(new SelectListItem { Text = doctor.Id.ToString(), Value = doctor.FirstName.ToString() + " " + doctor.LastName.ToString() });

                }
            }

            var getDataParent = await _uid.GetUsersInRoleAsync(Roles.Parent.ToString());
            var filterParent = getDataParent.Where(c => c.Email != "admin@gmail.com").ToList();
            List<SelectListItem> lstParents = new List<SelectListItem>();
            foreach (var parent in filterParent)
            {
                if (parent.Id.ToString() == empfromdb.parent_id.ToString())
                {
                    lstParents.Add(new SelectListItem { Text = parent.Id.ToString(), Value = parent.FirstName.ToString() + " " + parent.LastName.ToString() + " - " + parent.Email.ToString() });

                }
            }
            List<Category> categoryLst = _context.Categories.ToList();
            List<SelectListItem> category = new List<SelectListItem>();
            foreach (var item in categoryLst)
            {
                if (item.Id.ToString() == empfromdb.category.ToString())
                {
                    category.Add(new SelectListItem { Text = item.Id.ToString(), Value = item.Title.ToString() });
                }
            }


            List<Children> childrens = _context.Childrens.ToList();
            List<SelectListItem> lstChildrens = new List<SelectListItem>();
            foreach (var child in childrens)
            {
                if (child.Id.ToString() == empfromdb.child_id)
                {
                    lstChildrens.Add(new SelectListItem { Text = child.Id.ToString(), Value = child.FullName.ToString() });
                }
            }

            List<SelectListItem> slots = new List<SelectListItem>();
            List<Reservation> reservations = _context.Reservations.ToList();
            foreach (var reservation in reservations)
            {
                if (reservation.Id.ToString() == empfromdb.reservation_id)
                {
                    slots.Add(new SelectListItem { Text = reservation.Id.ToString(), Value = "Slot " + reservation.slot.ToString() + " Date: " + reservation.booking_date?.ToString("MM/dd/yyyy") });
                }
            }


            ViewBag.lstSlot = slots;
            ViewBag.child_lst = lstChildrens;
            ViewBag.category_lst = category;

            ViewBag.parent_lst = lstParents;
            ViewBag.doctor_lst = lstDoctor;


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
